using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisorGraph : MonoBehaviour {
    public int visionReach;
    public GameObject visorObj;
    public Graph visionGraph;

    private void Start() {
        if (visorObj == null) {
            visorObj = gameObject;
        }
    }

    public bool IsVisible(int[] visibilityNodes) {
        int vision = visionReach;
        Vertex src = visionGraph.GetNearestVertex(visorObj.transform.position);
        HashSet<Vertex> visibleNodes = new HashSet<Vertex>();
        Queue<Vertex> queue = new Queue<Vertex>();
        queue.Enqueue(src);
        //implement BFS algorithm
        while (queue.Count != 0) {
            if (vision == 0) {
                break;
            }
            Vertex v = queue.Dequeue();
            Vertex[] neighbours = visionGraph.GetNeighbours(v);
            foreach(Vertex n in neighbours) {
                if (visibleNodes.Contains(n)) {
                    continue;
                }
                queue.Enqueue(v);
                visibleNodes.Add(v);
            }
        }
        //compare the set of visible nodes with the set of nodes that reached
        //the vision system
        foreach(Vertex vn in visibleNodes) {
            if (visibleNodes.Contains(vn)) {
                return true;
            }
        }
        return false;
    }
}
