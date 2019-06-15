using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bandit : MonoBehaviour {
    bool init;
    int totalActions;
    int[] count;
    float[] score;
    int numActions;
    RPSAction lastAction;
    int lastStrategy;

    //initialize the UCB1 algorithm
    public void InitUCB1() {
        if (init) {
            return;
        }
        totalActions = 0;
        numActions = System.Enum.GetNames(typeof(RPSAction)).Length;
        count = new int[numActions];
        score = new float[numActions];
        for (int i = 0; i < numActions; i++) {
            count[i] = 0;
            score[i] = 0f;
        }
        init = true;
    }

    //compute the next action to be taken by the agent
    public RPSAction GetNextActionUCB1() {
        int best;
        float bestScore;
        float tempScore;
        InitUCB1();
        //check the number of actions available. If an action hasn't been
        //explored, return it
        for (int i = 0; i < numActions; i++) {
            if (count[i] == 0) {
                lastStrategy = i;
                lastAction = GetActionForStrategy((RPSAction)i);
                return lastAction;
            }
        }
        //variables for computing the best score
        best = 0;
        bestScore = score[best] / count[best];
        float input = Mathf.Log(totalActions / count[best]);
        input *= 2f;
        bestScore += Mathf.Sqrt(input);

        //check all actions available
        for (int i = 0; i < numActions; i++) {
            //compute the best score
            tempScore = score[i] / count[i];
            input = Mathf.Log(totalActions / count[best]);
            input *= 2f;
            tempScore = Mathf.Sqrt(input);
            if (tempScore > bestScore) {
                best = i;
                bestScore = tempScore;
            }
        }
        //return best strategy
        lastStrategy = best;
        lastAction = GetActionForStrategy((RPSAction)best);
        return lastAction;
    }

    //function for retrieving the best response for a given initial action
    public RPSAction GetActionForStrategy(RPSAction strategy) {
        RPSAction action;
        //implement basic rules of the game
        switch (strategy) {
            default:
                action = RPSAction.Scissors;
                break;
            case RPSAction.Paper:
                action = RPSAction.Scissors;
                break;
            case RPSAction.Rock:
                action = RPSAction.Paper;
                break;
            case RPSAction.Scissors:
                action = RPSAction.Rock;
                break;
        }
        return action;
    }

    //function for computing the utility of an action, based on the opponent's one
    //initially it is a draw
    public float GetUtility(RPSAction myAction, RPSAction opponentsAction) {
        float utility = 0f;
        //check whether the opponent played paper
        if (opponentsAction == RPSAction.Paper) {
            if (myAction == RPSAction.Rock) {
                utility = -1f;
            } else if (myAction == RPSAction.Scissors) {
                utility = 1f;
            }
        } else if (opponentsAction == RPSAction.Rock) {
            //check whether the opponent played rock
            if (myAction == RPSAction.Paper) {
                utility = 1f;
            } else if (myAction == RPSAction.Scissors) {
                utility = -1f;
            }
        } else {
            //check whether the opponent player scissors
            if (myAction == RPSAction.Rock) {
                utility = -1f;
            } else if (myAction == RPSAction.Paper) {
                utility = 1f;
            }
        }
        return utility;
    }

    //handle the overall actions of the game and inform the algorithm of the 
    //actions from the opponent, in this case, the player's actions
    public void TellOpponentAction(RPSAction action) {
        totalActions++;
        float utility = GetUtility(lastAction, action);
        score[(int)lastAction] += utility;
        count[(int)lastAction] += 1;
    }
}
