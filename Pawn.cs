using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : ChessPiece
{
    public override List<Vector2Int> GetLegalMoves(ChessBoard board)
    {
        List<Vector2Int> moves = new List<Vector2Int>();

        int dir = (color == PieceColor.White) ? 1 : -1;

        int startRow = (color == PieceColor.White) ? 1 : 6;

        int nextY = y + dir;

        // Forward 1
        if (board.IsEmpty(x, nextY))
        {
            moves.Add(new Vector2Int(x, nextY));

            // Forward 2 (first move)
            if (y == startRow && board.IsEmpty(x, nextY + dir))
            {
                moves.Add(new Vector2Int(x, nextY + dir));
            }
        }

        // Diagonal captures
        if (board.HasEnemy(x - 1, nextY, color))
            moves.Add(new Vector2Int(x - 1, nextY));

        if (board.HasEnemy(x + 1, nextY, color))
            moves.Add(new Vector2Int(x + 1, nextY));

        TryAddEnPassant(board, moves);

        return moves;
    }

    void TryAddEnPassant(ChessBoard board, List<Vector2Int> moves)
    {
        // Last move must be opponent pawn double-step
        if (!board.WasLastMovePawnDoubleStep())
            return;

        ChessPiece lastPawn = board.lastMovedPiece;
        if (lastPawn == null || lastPawn.color == color)
            return;

        // Capturing pawn must be on same rank as the pawn that moved
        if (lastPawn.y != y)
            return;

        // Must be horizontally adjacent
        if (Mathf.Abs(lastPawn.x - x) != 1)
            return;

        int direction = (color == PieceColor.White) ? 1 : -1;

        int targetX = lastPawn.x;
        int targetY = y + direction;

        // Target square must be valid and empty
        if (board.InBounds(targetX, targetY) &&
            board.IsEmpty(targetX, targetY))
        {
            moves.Add(new Vector2Int(targetX, targetY));
        }
    }
}
