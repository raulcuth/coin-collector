using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour {
    public List<Transition> transitions;

    public virtual void Awake() {
        transitions = new List<Transition>();
        //TODO
        //setup transitions
    }

    public virtual void OnEnable() {
        //TODO
        //develop state's initialization
    }

    public virtual void OnDisable() {
        //TODO
        //develop state's finalization
    }

    public virtual void Update() {
        //TODO
        //develop behaviour
    }

    public void LateUpdate() {
        foreach (Transition t in transitions) {
            if (t.condition.Test()) {
                t.target.enabled = true;
                this.enabled = false;
                return;
            }
        }
    }
}
