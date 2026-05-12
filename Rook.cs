using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : ChessPiece
{
    public override List<Vector2Int> GetLegalMoves(ChessBoard board)
    {
        List<Vector2Int> moves = new();

        int[] dirs = { -1, 1 };

        // Horizontal & Vertical
        foreach (int d in dirs)
        {
            // X direction
            for (int i = x + d; board.InBounds(i, y); i += d)
            {
                if (board.IsEmpty(i, y))
                {
                    moves.Add(new Vector2Int(i, y));
                }
                else
                {
                    if (board.HasEnemy(i, y, color))
                        moves.Add(new Vector2Int(i, y));
                    break;
                }
            }

            // Y direction
            for (int i = y + d; board.InBounds(x, i); i += d)
            {
                if (board.IsEmpty(x, i))
                {
                    moves.Add(new Vector2Int(x, i));
                }
                else
                {
                    if (board.HasEnemy(x, i, color))
                        moves.Add(new Vector2Int(x, i));
                    break;
                }
            }
        }

        return moves;
    }
}
