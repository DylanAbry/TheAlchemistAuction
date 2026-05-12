using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : ChessPiece
{
    public override List<Vector2Int> GetLegalMoves(ChessBoard board)
    {
        List<Vector2Int> moves = new();

        int[,] offsets =
        {
            { 1, 2 }, { 2, 1 }, { -1, 2 }, { -2, 1 },
            { 1, -2 }, { 2, -1 }, { -1, -2 }, { -2, -1 }
        };

        for (int i = 0; i < 8; i++)
        {
            int nx = x + offsets[i, 0];
            int ny = y + offsets[i, 1];

            if (!board.InBounds(nx, ny)) continue;

            if (board.IsEmpty(nx, ny) ||
                board.HasEnemy(nx, ny, color))
            {
                moves.Add(new Vector2Int(nx, ny));
            }
        }

        return moves;
    }
}
