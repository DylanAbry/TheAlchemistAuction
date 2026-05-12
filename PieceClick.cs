using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceClick : MonoBehaviour
{
    ChessPiece piece;

    void Start()
    {
        piece = GetComponent<ChessPiece>();
    }

    void OnMouseDown()
    {
        ChessManager.Instance.SelectPiece(piece);
    }
}
