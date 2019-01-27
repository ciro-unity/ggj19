using UnityEngine;

public class TetrisController : MonoBehaviour
{
    float fallCounter = 0;
    public int playerId;
    private TetrisPiece tetrisPiece;
    private GridManager gridManager;
    private Transform tetrisBlock;
    public Transform spawn;

    private bool isRunning = false;

    // Start is called before the first frame update
    void Awake()
    {
        gridManager = GetComponent<GridManager>();
    }

    public void StartTetrisMode(GameObject piece)
    {
        tetrisBlock = SpawnBlock(piece.transform);
        tetrisPiece = tetrisBlock.GetComponent<TetrisPiece>();
        GameInputManager.Instance.SetPlayerInputMode(playerId, 1);

        isRunning = true;
    }

    // Update is called once per frame
    private void Update()
    {
        if(!isRunning)
            return;


        Vector2Int inputVector = GameInputManager.Instance.ReadPlayerTetrisPhaseMovement(playerId);
        //Gets the state rotation of the piece grid
        PieceGrid pieceGrid = tetrisPiece.stateGrids[tetrisPiece.currentState];

        Vector2Int position2D = ConvertV3toV2Int(tetrisBlock.transform.localPosition);
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
            // Modify position
            MoveTetrisBlockSideways(pieceGrid, position2D, new Vector3(1, 0, 0));
        }

        else if (inputVector.x == -1)
        {
            Debug.Log("Move left");
            // Modify position
            MoveTetrisBlockSideways(pieceGrid, position2D, new Vector3(-1, 0, 0));
        }

        else if (inputVector.y == 1)
        {
            // Modify rotation
            if (gridManager.IsValidPosition(pieceGrid, position2D))
            {
                Debug.Log("IsValid");
                tetrisBlock.transform.Rotate(0, 0, 90);

                if (tetrisPiece.currentState == tetrisPiece.stateGrids.Length -1)
                {
                    Debug.Log("Current state == length" + tetrisPiece.currentState + " " + tetrisPiece.stateGrids.Length);
                    tetrisPiece.currentState = 0;
                }
                else if (tetrisPiece.currentState < tetrisPiece.stateGrids.Length -1)
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
        Transform newPiece = Instantiate<Transform>(tetrisBlock, spawn.transform.localPosition, Quaternion.identity);
        newPiece.SetParent(spawn.transform);

        return newPiece;
    }

    private void MoveTetrisBlockSideways(PieceGrid pieceGrid, Vector2Int position2D, Vector3 moveDirection)
    {
        // Modify position
        position2D.x += (int)moveDirection.x;
        position2D.y += (int)moveDirection.y;
        if (gridManager.IsValidPosition(pieceGrid, position2D))
        {
            Debug.Log("IsValid");
            //As long as the movement is valid, we increment the position
            tetrisBlock.transform.localPosition += moveDirection;
        }
        else
        {
            Debug.Log("isNotValid");
            //We update the grid once we reach an obstacle
            tetrisBlock.transform.localPosition = tetrisBlock.transform.localPosition;
        }
    }

    private void EndTetrisMode()
    {
        isRunning = false;
        GameManager.Instance.EndTetrisMode(playerId);
    }
}
