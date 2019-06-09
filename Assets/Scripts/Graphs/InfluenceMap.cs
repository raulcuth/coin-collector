using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfluenceMap : Graph {
    public List<Unit> unitList;
    //works as vertices in regular graphs
    GameObject[] locations;

    private void Awake() {
        if (unitList == null) {
            unitList = new List<Unit>();
        }
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
            for (int i= 1; i < u.radius; i++) {
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
}
