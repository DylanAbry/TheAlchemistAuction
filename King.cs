using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : ChessPiece
{
    public override List<Vector2Int> GetLegalMoves(ChessBoard board)
    {
        // Normal move generation (with castling)
        return GetLegalMoves(board, true);
    }

    // Overload used when checking for attacks (NO castling)
    public List<Vector2Int> GetLegalMoves(ChessBoard board, bool includeCastling)
    {
        List<Vector2Int> moves = new();

        // Normal king moves (1 square in any direction)
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue;

                int nx = x + dx;
                int ny = y + dy;

                if (!board.InBounds(nx, ny)) continue;

                if (board.IsEmpty(nx, ny) ||
                    board.HasEnemy(nx, ny, color))
                {
                    moves.Add(new Vector2Int(nx, ny));
                }
            }
        }

        if (includeCastling)
            TryAddCastling(board, moves);

        return moves;
    }

    void TryAddCastling(ChessBoard board, List<Vector2Int> moves)
    {
        // King must not have moved
        if (hasMoved)
            return;

        // King must not currently be in check
        if (board.IsKingInCheck(color))
            return;

        int row = (color == PieceColor.White) ? 0 : 7;

        // King-side castling
        TryCastleSide(board, moves, row, true);

        // Queen-side castling
        TryCastleSide(board, moves, row, false);
    }

    void TryCastleSide(ChessBoard board, List<Vector2Int> moves, int row, bool kingSide)
    {
        int rookX = kingSide ? 7 : 0;
        int direction = kingSide ? 1 : -1;

        ChessPiece rook = board.GetPiece(rookX, row);
        if (rook == null || rook is not Rook || rook.hasMoved)
            return;

        // Squares between king and rook must be empty
        for (int cx = x + direction; cx != rookX; cx += direction)
        {
            if (!board.IsEmpty(cx, row))
                return;
        }

        // King cannot pass through or land in check
        for (int i = 1; i <= 2; i++)
        {
            int testX = x + direction * i;
            if (board.WouldMoveCauseCheck(this, testX, row))
                return;
        }

        // Add castling move (king moves two squares)
        moves.Add(new Vector2Int(x + direction * 2, row));
    }
}


