using UnityEngine;

[CreateAssetMenu(fileName = "PieceGrid", menuName = "Tetris/PieceGrid")]
public class PieceGrid : ScriptableObject
{
    public Vector2[] blockPositions;
}
