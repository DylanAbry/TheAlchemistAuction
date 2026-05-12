using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PieceColor
{
    White,
    Black
}

public abstract class ChessPiece : MonoBehaviour
{
    public PieceColor color;
    public GameObject prefabReference;

    public int x;
    public int y;

    public bool hasMoved = false;

    public abstract List<Vector2Int> GetLegalMoves(ChessBoard board);

    public void SetPosition(int newX, int newY)
    {
        x = newX;
        y = newY;
    }
}
