using UnityEngine;

public class ActionGOB : MonoBehaviour {
    public virtual float GetGoalChange(GoalGOB goal) {
        return 0f;
    }

    public virtual float GetDuration() {
        return 0f;
    }
}
