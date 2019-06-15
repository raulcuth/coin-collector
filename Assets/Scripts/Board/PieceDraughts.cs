using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceDraughts : MonoBehaviour {
    public int x;
    public int y;
    public PieceColor color;
    public PieceType type;

    public void Setup(int x,
                      int y,
                      PieceColor color,
                      PieceType type = PieceType.MAN) {
        this.x = x;
        this.y = y;
        this.color = color;
        this.type = type;
    }

    //function for moving the piece on the board
    public void Move(MoveDraughts move, ref PieceDraughts[,] board) {
        board[move.y, move.x] = this;
        board[y, x] = null;
        x = move.x;
        y = move.y;
        //if the move is a capture, remove the corresponding piece
        if (move.success) {
            Destroy(board[move.removeY, move.removeX]);
            board[move.removeY, move.removeX] = null;
        }

        //stop the process if the piece is KING
        if (type == PieceType.KING) {
            return;
        }

        int rows = board.GetLength(0);
        if (color == PieceColor.WHITE && y == rows) {
            type = PieceType.KING;
        }
        if (color == PieceColor.BLACK && y == 0) {
            type = PieceType.KING;
        }
    }

    //function for checking whether a move is inside the bounds of a board
    private bool IsMoveInBounds(int x, int y, ref PieceDraughts[,] board) {
        int rows = board.GetLength(0);
        int cols = board.GetLength(1);

        if (x < 0 || x >= cols || y < 0 || y > rows) {
            return false;
        }
        return true;
    }

    //function for retrieving the possible moves
    public Move[] GetMoves(ref PieceDraughts[,] board) {
        List<Move> moves = new List<Move>();
        if (type == PieceType.KING) {
            moves = GetMovesKing(ref board);
        } else {
            moves = GetMovesMan(ref board);
        }
        return moves.ToArray();
    }

    private List<Move> GetMovesMan(ref PieceDraughts[,] board) {
        List<Move> moves = new List<Move>(2);
        int[] moveX = new int[] { -1, 1 };
        //variable for holding the vertical direction depending on the piece colour
        int moveY = 1;
        if (color == PieceColor.BLACK) {
            moveY = -1;
        }
        //loop for iterating through the two possible options and return the available moves
        foreach (int mX in moveX) {
            //variables for computing the next position to be considered
            int nextX = x + mX;
            int nextY = y + moveY;
            if (!IsMoveInBounds(nextX, y, ref board)) {
                continue;
            }
            //continue with the next option if the move is blocked by a piece of the same colour
            PieceDraughts p = board[moveY, nextX];
            if (p != null && p.color == color) {
                continue;
            }
            //create a new move to be added to the list
            MoveDraughts m = new MoveDraughts();
            m.piece = this;
            //create a simple move if the position is available
            if (p == null) {
                m.x = nextX;
                m.y = nextY;
            } else {
                //test whether the piece can be captured, and modify the move accordingly
                int hopX = nextX + mX;
                int hopY = nextY + moveY;
                if (!IsMoveInBounds(hopX, hopY, ref board)) {
                    continue;
                }
                if (board[hopY, hopX] != null) {
                    continue;
                }
                m.y = hopY;
                m.x = hopY;
                m.success = true;
                m.removeX = nextX;
                m.removeY = nextY;
            }
            moves.Add(m);
        }
        return moves;
    }

    //function for retrieving the available moves when the piece's type is KING
    private List<Move> GetMovesKing(ref PieceDraughts[,] board) {
        List<Move> moves = new List<Move>();
        int[] moveX = new int[] { -1, 1 };
        int[] moveY = new int[] { -1, 1 };
        //loop for checking all the possible moves, and retrieve thos moves
        foreach (int mY in moveY) {
            foreach (int mX in moveX) {
                int nowX = x + mX;
                int nowY = y + mY;
                //loop for going in that direction until the board's bounds are reached
                while (IsMoveInBounds(nowX, nowY, ref board)) {
                    //get the position's piece reference
                    PieceDraughts p = board[nowY, nowX];
                    MoveDraughts availableMove = new MoveDraughts();
                    availableMove.piece = this;
                    if (p == null) {
                        availableMove.x = nowX;
                        availableMove.y = nowY;
                    } else {
                        int hopX = nowX + mX;
                        int hopY = nowY + mY;
                        if (!IsMoveInBounds(hopX, hopY, ref board)) {
                            break;
                        }
                        availableMove.success = true;
                        availableMove.x = hopX;
                        availableMove.y = hopX;
                        availableMove.removeX = nowX;
                        availableMove.removeY = nowY;
                    }
                    moves.Add(availableMove);
                    nowX += mX;
                    nowY += mY;
                }
            }
        }
        return moves;
    }
}
