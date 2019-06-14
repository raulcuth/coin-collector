using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardAI {
    //works like a bounded DFS, in each step the move is chosen by selecting the option that
    //maximizes the player's score and assuming that the opponent will choose the
    //option of minimizing it until a terminal node is reached
    public static float Minimax(Board board,
                                int player,
                                int maxDepth,
                                int currentDepth,
                                ref Move bestMove) {
        if (board.IsGameOver() || currentDepth == maxDepth) {
            return board.Evaluate(player);
        }
        bestMove = null;
        float bestScore = Mathf.Infinity;
        if (board.GetCurrentPlayer() == player) {
            bestScore = Mathf.NegativeInfinity;
        }
        //loop through all possible moves and return the best score
        foreach (Move m in board.GetMoves()) {
            //create a new game state from the current move
            Board boardNewState = board.MakeMove(m);
            float currentScore;
            Move currentMove = null;
            //start the recursion
            currentScore = Minimax(boardNewState, player, maxDepth, currentDepth + 1, ref currentMove);
            //validate score for the current player
            if (board.GetCurrentPlayer() == player) {
                if (currentScore > bestScore) {
                    bestScore = currentScore;
                    bestMove = currentMove;
                }
            } else if (currentScore < bestScore) {
                //validate the score for the adversary
                bestScore = currentScore;
                bestMove = currentMove;
            }
        }
        return bestScore;
    }

    public static float Negamax(Board board,
                                int maxDepth,
                                int currentDepth,
                                ref Move bestMove) {
        if (board.IsGameOver() || currentDepth == maxDepth) {
            return board.Evaluate();
        }
        bestMove = null;
        float bestScore = Mathf.NegativeInfinity;
        //loop through all the available moves and return the best score
        foreach (Move m in board.GetMoves()) {
            //create a new game state from the current move
            Board newBoard = board.MakeMove(m);
            float recursedScore;
            Move currentMove = null;
            recursedScore = Negamax(newBoard, maxDepth, currentDepth + 1, ref currentMove);

            float currentScore = -recursedScore;
            if (currentScore > bestScore) {
                bestScore = currentScore;
                bestMove = m;
            }
        }
        return bestScore;
    }












}
