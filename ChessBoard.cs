using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessBoard
{
    public ChessPiece[,] grid = new ChessPiece[8, 8];

    // For en passant
    public ChessPiece lastMovedPiece;
    public Vector2Int lastMoveFrom;
    public Vector2Int lastMoveTo;

    public ChessPiece GetPiece(int x, int y)
    {
        if (!InBounds(x, y)) return null;
        return grid[x, y];
    }

    public bool IsEmpty(int x, int y)
    {
        return InBounds(x, y) && grid[x, y] == null;
    }

    public bool HasEnemy(int x, int y, PieceColor myColor)
    {
        if (!InBounds(x, y)) return false;
        ChessPiece p = grid[x, y];
        return p != null && p.color != myColor;
    }

    public bool InBounds(int x, int y)
    {
        return x >= 0 && x < 8 && y >= 0 && y < 8;
    }

    public void MovePiece(ChessPiece piece, int newX, int newY)
    {
        // Save previous move (for en passant)
        ChessPiece prevLastMoved = lastMovedPiece;
        Vector2Int prevFrom = lastMoveFrom;
        Vector2Int prevTo = lastMoveTo;

        int oldX = piece.x;
        int oldY = piece.y;

        // Clear old square FIRST
        grid[oldX, oldY] = null;

        // CASTLING: move rook AFTER clearing king
        if (piece is King && Mathf.Abs(newX - oldX) == 2)
        {
            int row = oldY;
            bool kingSide = newX > oldX;

            int rookFromX = kingSide ? 7 : 0;
            int rookToX = kingSide ? newX - 1 : newX + 1;

            ChessPiece rook = grid[rookFromX, row];

            if (rook != null)
            {
                grid[rookFromX, row] = null;
                grid[rookToX, row] = rook;

                rook.SetPosition(rookToX, row);

                
                Vector3 rookWorld =
                    BoardMapper.Instance.BoardToWorld(rookToX, row);
                rook.transform.position = rookWorld;

                rook.hasMoved = true;
            }
        }

        // Normal capture
        if (grid[newX, newY] != null)
        {
            GameObject.Destroy(grid[newX, newY].gameObject);
            grid[newX, newY] = null;
        }

        // EN PASSANT capture
        if (piece is Pawn &&
            prevLastMoved is Pawn &&
            Mathf.Abs(prevFrom.y - prevTo.y) == 2 &&
            prevTo.y == oldY &&
            Mathf.Abs(prevTo.x - oldX) == 1 &&
            newX == prevTo.x &&
            newY == oldY + ((piece.color == PieceColor.White) ? 1 : -1))
        {
            ChessPiece capturedPawn = grid[prevTo.x, oldY];
            if (capturedPawn != null)
            {
                grid[prevTo.x, oldY] = null;
                GameObject.Destroy(capturedPawn.gameObject);
            }
        }

        // Place king / piece LAST
        grid[newX, newY] = piece;
        piece.SetPosition(newX, newY);
        piece.hasMoved = true;

        // Record last move
        lastMovedPiece = piece;
        lastMoveFrom = new Vector2Int(oldX, oldY);
        lastMoveTo = new Vector2Int(newX, newY);
    }

    public ChessPiece FindKing(PieceColor color)
    {
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                ChessPiece p = grid[x, y];
                if (p is King && p.color == color)
                    return p;
            }
        }
        return null;
    }

    public bool IsKingInCheck(PieceColor color)
    {
        ChessPiece king = FindKing(color);
        if (king == null) return false;

        int kx = king.x;
        int ky = king.y;

        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                ChessPiece p = grid[x, y];
                if (p == null || p.color == color) continue;

                List<Vector2Int> moves;

                if (p is King enemyKing)
                    moves = enemyKing.GetLegalMoves(this, false); // NO castling
                else
                    moves = p.GetLegalMoves(this);

                foreach (var m in moves)
                {
                    if (m.x == kx && m.y == ky)
                        return true;
                }
            }
        }

        return false;
    }

    public bool WouldMoveCauseCheck(ChessPiece piece, int newX, int newY)
    {
        ChessPiece captured = grid[newX, newY];

        int oldX = piece.x;
        int oldY = piece.y;

        grid[oldX, oldY] = null;
        grid[newX, newY] = piece;

        piece.x = newX;
        piece.y = newY;

        bool inCheck = IsKingInCheck(piece.color);

        grid[oldX, oldY] = piece;
        grid[newX, newY] = captured;

        piece.x = oldX;
        piece.y = oldY;

        return inCheck;
    }

    public bool HasAnyLegalMove(PieceColor color)
    {
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                ChessPiece p = grid[x, y];
                if (p == null || p.color != color) continue;

                var moves = p.GetLegalMoves(this);
                foreach (var m in moves)
                {
                    if (!WouldMoveCauseCheck(p, m.x, m.y))
                        return true;
                }
            }
        }
        return false;
    }

    public bool IsCheckmate(PieceColor color)
    {
        return IsKingInCheck(color) && !HasAnyLegalMove(color);
    }

    public bool IsStalemate(PieceColor color)
    {
        return !IsKingInCheck(color) && !HasAnyLegalMove(color);
    }

    public bool WasLastMovePawnDoubleStep()
    {
        if (lastMovedPiece is not Pawn)
            return false;

        return Mathf.Abs(lastMoveFrom.y - lastMoveTo.y) == 2;
    }
}
