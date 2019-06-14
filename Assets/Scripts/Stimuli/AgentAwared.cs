using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentAwared : MonoBehaviour {
    protected Interest interest;
    protected bool isUpdated = false;

    //check if a given interest is relevant or not
    public bool isRelevant(Interest i) {
        int oldValue = (int)interest.priority;
        int newValue = (int)i.priority;
        if (newValue <= oldValue) {
            return false;
        }
        return true;
    }

    //set a new interest in the agent
    public void Notice(Interest i) {
        StopCoroutine(Investigate());
        interest = i;
        StartCoroutine(Investigate());
    }

    public virtual IEnumerator Investigate() {
        //TODO
        //develop custom implementation
        yield break;
    }

    //defined what an agent does when it is in charge of giving orders
    public virtual IEnumerator Lead() {
        //TODO
        //develop custom implementation
        yield break;
    }
}
