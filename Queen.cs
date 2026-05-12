using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : ChessPiece
{
    public override List<Vector2Int> GetLegalMoves(ChessBoard board)
    {
        List<Vector2Int> moves = new();

        int[] dirs = { -1, 0, 1 };

        foreach (int dx in dirs)
            foreach (int dy in dirs)
            {
                if (dx == 0 && dy == 0) continue;

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
