using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardTicTac : Board {
    protected int[,] board;
    protected const int ROWS = 3;
    protected const int COLS = 3;

    public BoardTicTac(int player = 1) {
        this.player = player;
        board = new int[ROWS, COLS];
        board[1, 1] = 1;
    }

    public BoardTicTac(int[,] board, int player) {
        this.board = board;
        this.player = player;
    }

    //function for retrieving the next player in turn
    private int GetNextPlayer(int p) {
        if (p == 1) {
            return 2;
        }
        return 1;
    }

    //function for evaluating a given position regarding a given player
    private float EvaluatePosition(int x, int y, int p) {
        if (board[y, x] == 0) {
            return 1f;
        }
        if (board[y, x] == p) {
            return 2f;
        }
        return -1f;
    }

    //define a function for evaluating the neighbours of a given position regarding a given player
    private float EvaluateNeighbours(int x, int y, int p) {
        float eval = 0f;
        for (int i = y - 1; i < y + 2; i++) {
            if (i < 0 || i >= ROWS) {
                continue;
            }
            for (int j = x - 1; j < x + 2; j++) {
                if (j < 0 || j >= COLS) {
                    continue;
                }
                if (i == j) {
                    continue;
                }
                eval += EvaluatePosition(j, i, p);
            }
        }
        return eval;
    }

    //override the member function for getting the available moves from the current state
    public override Move[] GetMoves() {
        List<Move> moves = new List<Move>();
        for (int i = 0; i < ROWS; i++) {
            for (int j = 0; j < COLS; j++) {
                if (board[i, j] != 0) {
                    continue;
                }
                MoveTicTac m = new MoveTicTac(j, i, player);
                moves.Add(m);
            }
        }
        return moves.ToArray();
    }

    //override the function for retrieving a new state from a given move
    public override Board MakeMove(Move m) {
        MoveTicTac move = (MoveTicTac)m;
        int nextPlayer = GetNextPlayer(move.player);

        int[,] copy = new int[ROWS, COLS];
        Array.Copy(board, 0, copy, 0, board.Length);
        copy[move.y, move.x] = move.player;
        BoardTicTac newBoardState = new BoardTicTac(copy, nextPlayer);

        return newBoardState;
    }

    //evaluate the current state given a player
    public override float Evaluate(int player) {
        float eval = 0f;
        for (int i = 0; i < ROWS; i++) {
            for (int j = 0; j < COLS; j++) {
                eval += EvaluatePosition(j, i, player);
                eval += EvaluateNeighbours(j, i, player);
            }
        }
        return eval;
    }

    //evaluate the current state of the current player
    public override float Evaluate() {
        float eval = 0f;
        for (int i = 0; i < ROWS; i++) {
            for (int j = 0; j < COLS; j++) {
                eval += EvaluatePosition(j, i, player);
                eval += EvaluateNeighbours(j, i, player);
            }
        }
        return eval;
    }
}
