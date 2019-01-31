using UnityEngine;

public class TetrisController : MonoBehaviour
{
    float fallCounter = 0;
    public int playerId;
    private TetrisPiece tetrisPiece;
    private GridManager gridManager;
    private Transform tetrisBlock;
    public Transform spawn;
    Vector2Int position2D;

    private bool isRunning = false;

    // Start is called before the first frame update
    void Awake()
    {
        gridManager = GetComponent<GridManager>();

        //move a bit left and a bit down, to be centered in the parent
        transform.Translate(-(float)GridManager.gridWidth * .5f  * transform.parent.localScale.x,
                            -(float)GridManager.gridHeight * .5f * transform.parent.localScale.y,
                            0f, Space.Self);
    }

    public void StartTetrisMode(GameObject piece)
    {
        isRunning = true;
        tetrisBlock = SpawnBlock(piece.transform);

        position2D = ConvertV3toV2Int(tetrisBlock.transform.parent.localPosition);

        tetrisPiece = tetrisBlock.GetComponent<TetrisPiece>();
        GameInputManager.Instance.SetPlayerInputMode(playerId, 1);
    }

    //Update is called once per frame
    private void Update()
    {
        if (!isRunning)
            return;


        Vector2Int inputVector = GameInputManager.Instance.ReadPlayerTetrisPhaseMovement(playerId);
        //Gets the state rotation of the piece grid
        PieceGrid pieceGrid = tetrisPiece.stateGrids[tetrisPiece.currentState];

        Debug.Log("position2D " + position2D);
        //Blocks move down by pressing the down key but also overtime
        if (inputVector.y == -1 || Time.time - fallCounter >= 1)
        {
            Debug.Log("Move down");

            // Modify position
            if (gridManager.IsValidPosition(pieceGrid, position2D))
            {
                Debug.Log("IsValid");
                //As long as the movement is valid, we increment the position
                tetrisBlock.transform.localPosition += new Vector3(0, -1, 0);
                position2D += new Vector2Int(0, -1);
            }
            else
            {
                Debug.Log("isNotValid");
                //We update the grid once we reach an obstacle
                gridManager.UpdateGrid(pieceGrid, position2D);
                EndTetrisMode();
            }
            fallCounter = Time.time;
        }

        else if (inputVector.x == 1)
        {
            Debug.Log("Move Right");
            Debug.Log("Move Right before " + position2D);
            // Modify position
            position2D = MoveTetrisBlockSideways(pieceGrid, position2D, new Vector3(1, 0, 0));

            Debug.Log("Move Right after " + position2D);
        }

        else if (inputVector.x == -1)
        {
            Debug.Log("Move left");
            // Modify position
            position2D = MoveTetrisBlockSideways(pieceGrid, position2D, new Vector3(-1, 0, 0));
        }

        else if (inputVector.y == 1)
        {
            // Modify rotation
            if (gridManager.IsValidPosition(pieceGrid, position2D))
            {
                Debug.Log("IsValid");
                tetrisBlock.transform.Rotate(0, 0, 90);

                if (tetrisPiece.currentState == tetrisPiece.stateGrids.Length - 1)
                {
                    Debug.Log("Current state == length" + tetrisPiece.currentState + " " + tetrisPiece.stateGrids.Length);
                    tetrisPiece.currentState = 0;
                }
                else if (tetrisPiece.currentState < tetrisPiece.stateGrids.Length - 1)
                {
                    Debug.Log("Current state < length" + tetrisPiece.currentState + " " + tetrisPiece.stateGrids.Length);
                    tetrisPiece.currentState++;
                }
            }
            else
            {
                Debug.Log("isNotValid");
            }

        }

    }

    public Vector2Int ConvertV3toV2Int(Vector3 vector3)
    {
        Vector2Int vector2Int = new Vector2Int
        {
            x = (int)vector3.x,
            y = (int)vector3.y
        };

        return vector2Int;
    }

    public Transform SpawnBlock(Transform tetrisBlock)
    {
        Transform newPiece = Instantiate<Transform>(tetrisBlock);
        newPiece.SetParent(spawn.transform, false);

        return newPiece;
    }

    private Vector2Int MoveTetrisBlockSideways(PieceGrid pieceGrid, Vector2Int position2D, Vector3 moveDirection)
    {
        // Modify position
        position2D.x += (int)moveDirection.x;
        position2D.y += (int)moveDirection.y;
        if (gridManager.IsValidPosition(pieceGrid, position2D))
        {
            Debug.Log("IsValid");
            //As long as the movement is valid, we increment the position
            tetrisBlock.transform.localPosition += moveDirection;

             //position2D += new Vector2Int((int)moveDirection.x, (int)moveDirection.y);
        }
        else
        {
            Debug.Log("isNotValid");

            position2D.x -= (int)moveDirection.x;
            position2D.y -= (int)moveDirection.y;
            //We update the grid once we reach an obstacle
        }

        return position2D;
    }

    private void EndTetrisMode()
    {
        isRunning = false;
        GameInputManager.Instance.SetPlayerInputMode(playerId, 0);
        GameManager.Instance.EndTetrisMode(playerId);
    }
}
