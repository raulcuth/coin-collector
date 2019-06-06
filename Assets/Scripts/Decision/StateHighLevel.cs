using System.Collections.Generic;
using UnityEngine;

public class StateHighLevel : State {
    public List<State> states;
    public State stateInitial;
    protected State stateCurrent;

    public override void OnEnable() {
        if (stateCurrent == null) {
            stateCurrent = stateInitial;
        }
        stateCurrent.enabled = true;
    }
}
