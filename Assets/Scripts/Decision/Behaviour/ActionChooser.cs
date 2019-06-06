using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionChooser : MonoBehaviour {
    public float CalculateDiscontentment(ActionGOB action, GoalGOB[] goals) {
        float discontentment = 0;
        foreach (GoalGOB goal in goals) {
            float newValue = goal.value + action.GetGoalChange(goal);
            newValue += action.GetDuration() + goal.GetChange();
            discontentment += goal.GetDiscontentment(newValue);
        }
        return discontentment;
    }

    public ActionGOB Choose(ActionGOB[] actions, GoalGOB[] goals) {
        ActionGOB bestAction;
        bestAction = actions[0];
        float bestValue = CalculateDiscontentment(actions[0], goals);
        float value;
        //pick the best action based on the least compromising
        foreach (ActionGOB action in actions) {
            value = CalculateDiscontentment(action, goals);
            if (value < bestValue) {
                bestValue = value;
                bestAction = action;
            }
        }
        return bestAction;
    }
}
