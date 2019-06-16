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

    //regret decision making
    float initialRegret = 10f;
    float[] regret;
    float[] chance;
    RPSAction lastOpponentAction;
    RPSAction[] lastActionRM;

    //initialize the regret decision making
    public void InitRegretMatching() {
        if (init) {
            return;
        }
        numActions = System.Enum.GetNames(typeof(RPSAction)).Length;
        regret = new float[numActions];
        chance = new float[numActions];
        for (int i = 0; i < numActions; i++) {
            regret[i] = initialRegret;
            chance[i] = 0f;
        }
        init = true;
    }

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

    //computes the next action to be taken
    public RPSAction GetNextActionRM() {
        float sum = 0f;
        float prob = 0f;
        InitRegretMatching();
        //explore all available options and hold the response to be taken
        for (int i = 0; i < numActions; i++) {
            lastActionRM[i] = GetActionForStrategy((RPSAction)i);
        }
        //sum the overall regret
        for (int i = 0; i < numActions; i++) {
            if (regret[i] > 0f) {
                sum += regret[i];
            }
        }
        //return a random action if the sum is less than or equal to 0
        if (sum <= 0f) {
            lastAction = (RPSAction)Random.Range(0, numActions);
            return lastAction;
        }
        //explore the set of actions and sum the chance of regretting them
        for (int i = 0; i < numActions; i++) {
            chance[i] = 0f;
            if (regret[i] > 0f) {
                chance[i] = regret[i];
            }
            if (i > 0) {
                chance[i] += chance[i - 1];
            }
        }
        //computes a random probability and compare that to the chance of taking
        //the actions. Returns the first one to be greater than the probability computed
        prob = Random.value;
        for (int i = 0; i < numActions; i++) {
            if (prob < chance[i]) {
                lastStrategy = i;
                lastAction = lastActionRM[i];
                return lastAction;
            }
        }
        return (RPSAction)(numActions - 1);
    }

    public void TellOpponentActionRM(RPSAction action) {
        lastOpponentAction = action;
        for (int i = 0; i < numActions; i++) {
            regret[i] += GetUtility((RPSAction)lastActionRM[i], (RPSAction)action);
            regret[i] -= GetUtility((RPSAction)lastAction, (RPSAction)action);
        }
    }
}
