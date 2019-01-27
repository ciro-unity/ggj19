using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static int gridWidth = 5;
    public static int gridHeight = 10;
    public GameObject cellDebugPrefab;

    public bool[,] grid = new bool[gridWidth, gridHeight];

    public void Start()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                GameObject newCellDebug = Instantiate(cellDebugPrefab);
                newCellDebug.transform.SetParent(transform, false);
                newCellDebug.transform.localPosition = new Vector2(x, y);
            }
        }
    }

    public static Vector2 Round(Vector2 v)
    {
        return new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
    }


    public bool IsInsideGrid(PieceGrid pieceGrid, Vector2Int position)
    {
        foreach (Vector2 block in pieceGrid.blockPositions)
        {
            Debug.Log("isInsideGrid function, block : " + block);
            if (position.x + (int)block.x >= gridWidth || position.x + (int)block.x < 0
                || position.y + (int)block.y <= 0 || position.y + (int)block.y >= gridHeight)
            {
                Debug.Log("Is not inside grid");
                Debug.Log(position.x + (int)block.x <= 0);
                return false;
            }
        }
        return true;
    }

    public bool IsValidPosition(PieceGrid pieceGrid, Vector2Int position)
    {
        if (!IsInsideGrid(pieceGrid, position))
        {
            //Debug.Log("Is not inside grid");
            Debug.Log(position);
            return false;
        }

        foreach (Vector2 block in pieceGrid.blockPositions)
        {
            int vx = position.x + (int)(block.x);
            int vy = position.y + (int)(block.y);
            if (grid[vx, vy] == true)
            {
                return false;
            }
        }
        return true;
    }

    //Deprecated
    public void UpdateGrid(PieceGrid pieceGrid, Vector2Int position, Vector2Int lastPositionVector)
    {
        // Remove old children from grid
        foreach (Vector2 block in pieceGrid.blockPositions)
        {
            int vx = lastPositionVector.x + (int)(block.x);
            int vy = lastPositionVector.y + (int)(block.y);
            Debug.Log("Remove old");
            Debug.Log("grid [" + vx + "," + vy + "] is : " + grid[vx, vy]);
            grid[vx, vy] = false;
        }

        // Add new children to grid
        foreach (Vector2 block in pieceGrid.blockPositions)
        {
            int vx = position.x + (int)(block.x);
            int vy = position.y + (int)(block.y);
            Debug.Log("Add New");
            Debug.Log("grid [" + vx + "," + vy + "] is : " + grid[vx, vy]);
            grid[vx, vy] = true;
        }
    }

    public void UpdateGridV2(PieceGrid pieceGrid, Vector2Int position)
    {
        // Add new children to grid
        foreach (Vector2 block in pieceGrid.blockPositions)
        {
            int vx = position.x + (int)(block.x);
            int vy = position.y + (int)(block.y);
            Debug.Log("Add New");
            Debug.Log("grid [" + vx + "," + vy + "] is : " + grid[vx, vy]);
            grid[vx, vy] = true;
        }
    }
}
