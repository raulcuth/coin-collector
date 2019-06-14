using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//handles the global update for every source, taking into account the active ones
//an interest source receives the list of agents and retrieves only the affected ones
//the manager handles the affected agents, sets up scouts and a leader, and informs
//them about their relative interests
public class SensoryManager : MonoBehaviour {
    public List<AgentAwared> agents;
    public List<InterestSource> sources;

    private void Awake() {
        agents = new List<AgentAwared>();
        sources = new List<InterestSource>();
    }

    //get the scouts, given a group of agents
    public List<AgentAwared> GetScouts(AgentAwared[] agents, int leader = -1) {
        if (agents.Length == 0) {
            return new List<AgentAwared>(0);
        }
        if (agents.Length == 1) {
            return new List<AgentAwared>(agents);
        }
        //remove the leader if given its index
        List<AgentAwared> agentList = new List<AgentAwared>(agents);
        if (leader > -1) {
            agentList.RemoveAt(leader);
        }

        //calculate the number of scouts to retrieve
        List<AgentAwared> scouts = new List<AgentAwared>();
        float numAgents = agents.Length;
        int numScouts = (int)Mathf.Log(numAgents, 2f);
        //get the random scouts from the list of agents
        while (numScouts != 0) {
            int numA = agentList.Count;
            int r = Random.Range(0, numA);
            AgentAwared a = agentList[r];
            scouts.Add(a);
            agentList.RemoveAt(r);
            numScouts--;
        }
        return scouts;
    }

    public void UpdateLoop() {
        List<AgentAwared> affected;
        AgentAwared leader;
        List<AgentAwared> scouts;
        foreach (InterestSource source in sources) {
            //avoid inactive sources
            if (!source.isActive) {
                continue;
            }
            source.isActive = false;
            //avoid sources that don't affect any agent
            affected = source.GetAffected(agents.ToArray());
            if (affected.Count == 0) {
                continue;
            }
            //get a random leader and set of scouts
            int randomLeadPos = Random.Range(0, affected.Count);
            leader = affected[randomLeadPos];
            scouts = GetScouts(affected.ToArray(), randomLeadPos);
            //call the leader to its role if necessary
            if (leader.Equals(scouts[0])) {
                StartCoroutine(leader.Lead());
            }
            //inform the scouts about noticing the interest, in case it's relevant
            foreach (AgentAwared a in scouts) {
                Interest i = source.interest;
                if (a.isRelevant(i)) {
                    a.Notice(i);
                }
            }
        }
    }
}
