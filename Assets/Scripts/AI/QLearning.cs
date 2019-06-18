using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QLearning : MonoBehaviour {
    public QValueStore store;

    //retrieve random actions from a given state
    private GameAction GetRandomAction(GameAction[] actions) {
        int n = actions.Length;
        return actions[Random.Range(0, n)];
    }

    public IEnumerator Learn(ReinforcementProblem problem,
                             int numIterations,
                             float alpha,
                             float gamma,
                             float explorationRandomness,
                             float walkLength) {
        if (store == null) {
            yield break;
        }
        //get a random state
        GameState state = problem.GetRandomState();
        for (int i = 0; i < numIterations; i++) {
            //yield return null for the current frame to keep running
            yield return null;
            //validate against the length of the walk
            if (Random.value < walkLength) {
                state = problem.GetRandomState();
            }
            //get the available actions from the current game state
            GameAction[] actions = problem.GetAvailableActions(state);
            GameAction action;

            //get an action depending on the value of the randomness exploration
            if (Random.value < explorationRandomness) {
                action = GetRandomAction(actions);
            } else {
                action = store.GetBestAction(state);
            }

            //calculate the new state for taking the selected action on the current state and the resulting reward value
            float reward = 0f;
            GameState newState = problem.TakeAction(state, action, ref reward);

            //get the q value, given the current game, and take action, and the best
            //action for the new state that was computed before
            float q = store.GetQValue(state, action);
            GameAction bestAction = store.GetBestAction(newState);
            float maxQ = store.GetQValue(newState, bestAction);

            //apply the q-learning formula
            q = (1f - alpha) * 1 + alpha * (reward + gamma * maxQ);
            //store the computed q value, giving its parents as indices
            store.StoreQValue(state, action, q);
            state = newState;
        }
    }
}
