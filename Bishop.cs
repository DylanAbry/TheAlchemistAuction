using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : ChessPiece
{
    public override List<Vector2Int> GetLegalMoves(ChessBoard board)
    {
        List<Vector2Int> moves = new();

        int[] dirs = { -1, 1 };

        foreach (int dx in dirs)
            foreach (int dy in dirs)
            {
                int nx = x + dx;
                int ny = y + dy;

                while (board.InBounds(nx, ny))
                {
                    if (board.IsEmpty(nx, ny))
                    {
                        moves.Add(new Vector2Int(nx, ny));
                    }
                    else
                    {
                        if (board.HasEnemy(nx, ny, color))
                            moves.Add(new Vector2Int(nx, ny));

                        break;
                    }

                    nx += dx;
                    ny += dy;
                }
            }

        return moves;
    }
}
