using UnityEngine;

public class TetrisController : MonoBehaviour
{
    float fallCounter = 0;
    public int playerId;
    private TetrisPiece tetrisPiece;
    public GridManager gridManager;
    public Transform tetrisBlock;
    public Transform spawn;

    // Start is called before the first frame update
    void Awake()
    {
        gridManager = GetComponent<GridManager>();
    }

    void Start()
    {
        tetrisBlock = SpawnBlock(tetrisBlock);
        tetrisPiece = tetrisBlock.GetComponent<TetrisPiece>();
        GameInputManager.Instance.SetPlayerInputMode(playerId, 1);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2Int inputVector = GameInputManager.Instance.ReadPlayerTetrisPhaseMovement(playerId);
        Debug.Log(inputVector);
        //Gets the state rotation of the piece grid
        PieceGrid pieceGrid = tetrisPiece.stateGrids[tetrisPiece.currentState];
        //Getting the last position for gridupdate, to remove from the grid positions that were freed by the block
        //and occupy the new positions according to the piece state
        Vector2Int position2D = ConvertV3toV2Int(tetrisBlock.transform.position);
        Vector2Int lastPosition2D = ConvertV3toV2Int(tetrisBlock.transform.position - new Vector3(0, -1, 0));
        //Blocks move down by pressing the down key but also overtime
        if (inputVector.y == -1 || Time.time - fallCounter >= 1)
        {
            Debug.Log("Move down");

            if (gridManager.IsValidPosition(pieceGrid, position2D))
            {
                Debug.Log("IsValid");
                //As long as the movement is valid, we increment the position
                //tetrisBlock.transform.position += new Vector3(0, -1, 0);
                position2D += inputVector;
            }
            else
            {
                Debug.Log("isNotValid");
                //We update the grid once we reach an obstacle
                gridManager.UpdateGridV2(pieceGrid, position2D);
            }

            fallCounter = Time.time;
        }

        else if (inputVector.x == 1)
        {
            // Modify position
            //tetrisBlock.transform.position += new Vector3(1, 0, 0);
            position2D += inputVector;
        }

        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Debug.Log("Move left");
            // Modify position
            if (gridManager.IsValidPosition(pieceGrid, position2D))
            {
                Debug.Log("IsValid");
                //As long as the movement is valid, we increment the position
                tetrisBlock.transform.position += new Vector3(-1, 0, 0);
            }
            else
            {
                Debug.Log("isNotValid");
                //We update the grid once we reach an obstacle
                gridManager.UpdateGridV2(pieceGrid, position2D);
            }
        }

        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // Modify rotation
            tetrisBlock.transform.Rotate(0, 0, 90);
            if(tetrisPiece.currentState != tetrisPiece.stateGrids.Length)
            {
                tetrisPiece.currentState++;
            }
            else
            {
                tetrisPiece.currentState = 0;
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
        Transform newPiece = Instantiate<Transform>(tetrisBlock, spawn.transform.position, Quaternion.identity);
        newPiece.SetParent(spawn.transform);

        return newPiece;
    }
}
