using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static int gridWidth = 2;
    public static int gridHeight = 4;

    public int state = 0;

    public bool[,] grid = new bool[gridWidth, gridHeight];

    public static Vector2 Round(Vector2 v)
    {
        return new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
    }

    public bool IsInsideGrid(PieceGrid pieceGrid, Vector2Int position)
    {
        foreach (Vector2 block in pieceGrid.blockPositions)
        {
            if (position.x + (int)block.x >= gridWidth || position.x + (int)block.x < 0
                || position.y + (int)block.y < 0 || position.y + (int)block.y >= 0)
            {
                return false;
            }
        }
        return true;
    }

    public bool IsValidPosition(PieceGrid pieceGrid, Vector2Int position)
    {
        if (!IsInsideGrid(pieceGrid, position))
        {
            return false;
        }

        foreach (Vector2 block in pieceGrid.blockPositions)
        {
            if(grid[position.x + (int)block.x, position.y + (int)block.y] == true)
            {
                return false;
            }
        }
        return true;
    }
}
