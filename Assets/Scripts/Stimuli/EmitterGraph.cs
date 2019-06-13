using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmitterGraph : MonoBehaviour {
    public int soundIntensity;
    public Graph soundGraph;
    public GameObject emitterObj;

    private void Start() {
        if (emitterObj == null) {
            emitterObj = gameObject;
        }
    }

    public List<Vertex> Emit() {
        List<Vertex> nodes = new List<Vertex>();
        Queue<Vertex> queue = new Queue<Vertex>();
        Vertex[] neighbours;
        int intensity = soundIntensity;
        Vertex srcVertex = soundGraph.GetNearestVertex(emitterObj.transform.position);
        nodes.Add(srcVertex);
        queue.Enqueue(srcVertex);

        //BFS loop to reach out to nodes
        while (queue.Count != 0) {
            if (intensity == 0) {
                break;
            }
            Vertex v = queue.Dequeue();
            neighbours = soundGraph.GetNeighbours(v);
            //check the neighbours and add them to the queue is necessary
            foreach (Vertex n in neighbours) {
                if (nodes.Contains(n)) {
                    continue;
                }
                queue.Enqueue(n);
                nodes.Add(n);
            }
            intensity--;
        }
        return nodes;
    }
}
