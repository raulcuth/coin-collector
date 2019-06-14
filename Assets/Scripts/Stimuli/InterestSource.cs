using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterestSource : MonoBehaviour {
    public InterestSense sense;
    public float radius;
    public InterestPriority priority;
    public bool isActive;

    public Interest interest {
        get {
            Interest i;
            i.position = transform.position;
            i.priority = priority;
            i.sense = sense;
            return i;
        }
    }

    protected bool IsAffectedSight(AgentAwared agent) {
        //TODO
        //sight check implementation
        return false;
    }

    protected bool IsAffectedSound(AgentAwared agent) {
        //TODO
        //sound check implementation
        return false;
    }

    public virtual List<AgentAwared> GetAffected(AgentAwared[] agentList) {
        List<AgentAwared> affected = new List<AgentAwared>();
        Vector3 interPos = transform.position;
        Vector3 agentPos;
        float distance;
        //main loop for traversing the list of agents and return the list of those affected
        foreach (AgentAwared agent in agentList) {
            //discriminate an agent if it is beyond the source's action radius
            agentPos = agent.transform.position;
            distance = Vector3.Distance(interPos, agentPos);
            if (distance > radius) {
                continue;
            }
            //check whether the agent is affected, given the source's type of sense
            bool isAffected = false;
            switch (sense) {
                case InterestSense.SIGHT:
                    isAffected = IsAffectedSight(agent);
                    break;
                case InterestSense.SOUND:
                    isAffected = IsAffectedSound(agent);
                    break;
            }
            if (!isAffected) {
                continue;
            }
            affected.Add(agent);
        }
        return affected;
    }
}
