﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfluenceMap : Graph {
    public List<Unit> unitList;
    public float dropOffThreshold;
    private Guild[] guildList;
    //works as vertices in regular graphs
    GameObject[] locations;

    private void Awake() {
        if (unitList == null) {
            unitList = new List<Unit>();
        }
        guildList = gameObject.GetComponents<Guild>();
    }

    public void AddUnit(Unit u) {
        if (unitList.Contains(u)) {
            return;
        }
        unitList.Add(u);
    }

    public void RemoveUnit(Unit u) {
        unitList.Remove(u);
    }

    public void ComputeInfluenceSimple() {
        VertexInfluence vertexInfluence;
        float dropOff;
        List<Vertex> pending = new List<Vertex>();
        List<Vertex> visited = new List<Vertex>();
        List<Vertex> frontier;
        Vertex[] localNeighbours;

        foreach (Unit u in unitList) {
            Vector3 uPos = u.transform.position;
            Vertex vert = GetNearestVertex(uPos);
            pending.Add(vert);

            //apply BFS-based code for spreading influence given the radius reach
            for (int i = 1; i < u.radius; i++) {
                frontier = new List<Vertex>();
                foreach (Vertex p in pending) {
                    if (visited.Contains(p)) {
                        continue;
                    }
                    visited.Add(p);
                    vertexInfluence = p as VertexInfluence;
                    dropOff = u.GetDropOff(i);
                    vertexInfluence.SetValue(u.faction, dropOff);
                    localNeighbours = GetNeighbours(vert);
                    frontier.AddRange(localNeighbours);
                }
                pending = new List<Vertex>(frontier);
            }
        }
    }

    public List<GuildRecord> ComputeMapFlooding() {
        BinaryHeap<GuildRecord> open = new BinaryHeap<GuildRecord>();
        List<GuildRecord> closed = new List<GuildRecord>();
        //add the initial nodes for each guild in the priority queue
        foreach (Guild g in guildList) {
            GuildRecord gr = new GuildRecord();
            gr.location = GetNearestVertex(g.baseObject.transform.position);
            gr.guild = g;
            gr.strength = g.GetDropOff(0f);
            open.Add(gr);
        }
        //create main Dijkstra iteration and return the assignments
        while (open.Count != 0) {
            GuildRecord current = open.Remove();
            Vertex v = current.location;
            Vector3 currPos = v.transform.position;
            Vertex[] neighbours = GetNeighbours(current.location);

            //loop for computing each neighbour and put the current node in the closed list
            foreach (Vertex n in neighbours) {
                //compute the drop-off from the current vertex and check whether
                //it is worth trying to change the guild assigned
                Vector3 nPos = n.transform.position;
                float dist = Vector3.Distance(currPos, nPos);
                float strength = current.guild.GetDropOff(dist);
                if (strength < dropOffThreshold) {
                    continue;
                }
                //create an auxiliary GuilRecord node with the current vertex's data
                GuildRecord neighGR = new GuildRecord();
                neighGR.location = n;
                neighGR.strength = strength;
                VertexInfluence vi = n as VertexInfluence;
                neighGR.guild = vi.guild;

                //check the closed list and validate the time 
                //when a new assignment must be avoided
                if (closed.Contains(neighGR)) {
                    Vertex location = neighGR.location;
                    int index = closed.FindIndex(x => x.location == location);
                    GuildRecord gr = closed[index];
                    if (gr.guild.name != current.guild.name && gr.strength < strength) {
                        continue;
                    }
                } else if (open.Contains(neighGR)) {
                    bool mustContinue = false;
                    foreach (GuildRecord gr in open) {
                        if (gr.Equals(neighGR)) {
                            mustContinue = true;
                            break;
                        }
                    }
                    if (mustContinue) {
                        continue;
                    }
                } else {
                    neighGR = new GuildRecord();
                    neighGR.location = n;
                }
                neighGR.guild = current.guild;
                neighGR.strength = strength;
                open.Add(neighGR);
            }
            closed.Add(current);
        }
        return closed;
    }
}