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
    //The alpha value is the lowest score a player can achieve, 
    //thus avoiding considering any move where the opponent has the opportunity
    //to lessen it. Similarly, the beta value is the upper limit, and no matter 
    //how tempting the new option is, the algorithm assumes that the opponent 
    //won't provide the opportunity to take it
    public static float ABNegamax(Board board,
                                  int player,
                                  int maxDepth,
                                  int currentDepth,
                                  ref Move bestMove,
                                  float alpha,
                                  float beta) {
        if (board.IsGameOver() || currentDepth == maxDepth) {
            return board.Evaluate(player);
        }
        bestMove = null;
        float bestScore = Mathf.NegativeInfinity;
        //loop through every available move and return the best score
        foreach (Move m in board.GetMoves()) {
            Board newBoard = board.MakeMove(m);
            float recursedScore;
            Move currentMove = null;
            int cd = currentDepth + 1;
            float max = Mathf.Max(alpha, bestScore);
            recursedScore = ABNegamax(newBoard, player, maxDepth, cd, ref currentMove, -beta, max);

            //set the current score and update the best score and move if necessary
            float currentScore = -recursedScore;
            if (currentScore > bestScore) {
                bestScore = currentScore;
                bestMove = m;
                if (bestScore >= beta) {
                    return bestScore;
                }
            }
        }
        return bestScore;
    }

    //this algorithm works by examining the first move of each node.
    //the following moves are examined using a scout pass with a narrower window
    //based on the first move. If the pass fails, it is repeated using a full-width
    //window. As a result a large number og branches are pruned and failures are avoided
    public static float ABNegascout(Board board,
                                    int player,
                                    int maxDepth,
                                    int currentDepth,
                                    ref Move bestMove,
                                    float alpha,
                                    float beta) {
        if (board.IsGameOver() || currentDepth == maxDepth) {
            return board.Evaluate(player);
        }
        bestMove = null;
        float bestScore = Mathf.NegativeInfinity;
        float adaptiveBeta = beta;

        foreach (Move m in board.GetMoves()) {
            Board newGameStateBoard = board.MakeMove(m);
            Move currentMove = null;
            float recursedScore;
            int depth = currentDepth + 1;
            float max = Mathf.Max(alpha, bestScore);

            recursedScore = ABNegamax(newGameStateBoard, player, maxDepth, depth, ref currentMove, -adaptiveBeta, max);

            float currentScore = -recursedScore;
            if (currentScore > bestScore) {
                //validate for pruning
                if (adaptiveBeta.Equals(beta) || currentDepth >= maxDepth - 2) {
                    bestScore = currentScore;
                    bestMove = currentMove;
                } else {
                    float negativeBest =
                        ABNegascout(newGameStateBoard, player, maxDepth, depth, ref bestMove, -beta, -currentScore);
                    bestScore = -negativeBest;
                }

                if (bestScore >= beta) {
                    return bestScore;
                }
                adaptiveBeta = Mathf.Max(alpha, bestScore) + 1f;
            }
        }
        return bestScore;
    }
}
