﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour {
    public GameObject vertexPrefab;
    protected List<Vertex> vertices;
    protected List<List<Vertex>> neighbours;
    protected List<List<float>> costs;
    protected Dictionary<int, int> instIdToId;

    public virtual void Start() {
        Load();
    }

    public virtual void Load() {

    }

    public virtual int GetSize() {
        if (ReferenceEquals(vertices, null)) {
            return 0;
        }
        return vertices.Count;
    }

    public virtual Vertex GetNearestVertex(Vector3 position) {
        return null;
    }

    public virtual Vertex GetVertexObj(int id) {
        if (ReferenceEquals(vertices, null) || vertices.Count == 0) {
            return null;
        }
        if (id < 0 || id >= vertices.Count) {
            return null;
        }
        return vertices[id];
    }

    public virtual Vertex[] GetNeighbours(Vertex v) {
        if (ReferenceEquals(neighbours, null) || neighbours.Count == 0) {
            return new Vertex[0];
        }
        if (v.id < 0 || v.id >= neighbours.Count) {
            return new Vertex[0];
        }
        return neighbours[v.id].ToArray();
    }

    public virtual Edge[] GetEdges(Vertex v) {
        if (ReferenceEquals(neighbours, null) || neighbours.Count == 0)
            return new Edge[0];
        if (v.id < 0 || v.id >= neighbours.Count)
            return new Edge[0];
        int numEdges = neighbours[v.id].Count;
        Edge[] edges = new Edge[numEdges];
        List<Vertex> vertexList = neighbours[v.id];
        List<float> costList = costs[v.id];
        for (int i = 0; i < numEdges; i++) {
            edges[i] = new Edge();
            edges[i].cost = costList[i];
            edges[i].vertex = vertexList[i];
        }
        return edges;
    }

    public List<Vertex> GetPathBFS(GameObject srcObj, GameObject dstObj) {
        if (srcObj == null || dstObj == null) {
            return new List<Vertex>();
        }
        Vertex[] bfsNeighbours;
        Queue<Vertex> q = new Queue<Vertex>();
        Vertex src = GetNearestVertex(srcObj.transform.position);
        Vertex dst = GetNearestVertex(dstObj.transform.position);
        Vertex v;
        int[] previous = new int[vertices.Count];
        for (int i = 0; i < previous.Length; i++) {
            previous[i] = -1;
        }
        previous[src.id] = src.id;
        q.Enqueue(src);

        //implement the BFS algorithm for finding a path
        while (q.Count != 0) {
            v = q.Dequeue();
            if (ReferenceEquals(v, dst)) {
                return BuildPath(src.id, v.id, ref previous);
            }
            bfsNeighbours = GetNeighbours(v);
            foreach (Vertex n in bfsNeighbours) {
                if (previous[n.id] != -1) {
                    continue;
                }
                previous[n.id] = v.id;
                q.Enqueue(n);
            }
        }
        return new List<Vertex>();
    }

    public List<Vertex> GetPathDFS(GameObject srcObj, GameObject dstObj) {
        if (srcObj == null || dstObj == null) {
            return new List<Vertex>();
        }
        Vertex src = GetNearestVertex(srcObj.transform.position);
        Vertex dst = GetNearestVertex(dstObj.transform.position);
        Vertex[] dfsNeighbours;
        Vertex v;
        int[] previous = new int[vertices.Count];
        for (int i = 0; i < previous.Length; i++) {
            previous[i] = -1;
        }
        previous[src.id] = src.id;
        Stack<Vertex> s = new Stack<Vertex>();
        s.Push(src);

        //DFS algorithm for finding a path
        while (s.Count != 0) {
            v = s.Pop();
            if (ReferenceEquals(v, dst)) {
                return BuildPath(src.id, v.id, ref previous);
            }
            dfsNeighbours = GetNeighbours(v);
            foreach (Vertex n in dfsNeighbours) {
                if (previous[n.id] != -1) {
                    continue;
                }
                previous[n.id] = v.id;
                s.Push(n);
            }
        }
        return new List<Vertex>();
    }

    public List<Vertex> GetPathDijkstra(GameObject srcObj, GameObject dstObj) {
        if (srcObj == null || dstObj == null) {
            return new List<Vertex>();
        }
        Vertex src = GetNearestVertex(srcObj.transform.position);
        Vertex dst = GetNearestVertex(dstObj.transform.position);
        BinaryHeap<Edge> frontier = new BinaryHeap<Edge>();
        Edge[] edges;
        Edge node, child;
        int size = vertices.Count;
        float[] distValue = new float[size];
        int[] previous = new int[size];

        //add the source node to the priority queue and assign a distance value 
        //of infinity to all of them but the source node
        node = new Edge(src, 0);
        frontier.Add(node);
        distValue[src.id] = 0;
        previous[src.id] = src.id;
        for (int i = 0; i < size; i++) {
            if (i == src.id) {
                continue;
            }
            distValue[i] = Mathf.Infinity;
            previous[i] = -1;
        }

        //define a loop to iterate while the queue is not empty
        while (frontier.Count != 0) {
            node = frontier.Remove();
            int nodeId = node.vertex.id;
            //call the procedure to build the path when arriving at the destination
            if (ReferenceEquals(node.vertex, dst)) {
                return BuildPath(src.id, node.vertex.id, ref previous);
            }
            //otherwise process the visited nodes and add its neighbours to the
            //queue, and return the path(not empty if there is a path from the
            //source to the destination vertex
            edges = GetEdges(node.vertex);
            foreach (Edge e in edges) {
                int eId = e.vertex.id;
                if (previous[eId] != -1) {
                    continue;
                }
                float cost = distValue[nodeId] + e.cost;
                if (cost < distValue[e.vertex.id]) {
                    distValue[eId] = cost;
                    previous[eId] = nodeId;
                    frontier.Remove(e);
                    child = new Edge(e.vertex, cost);
                    frontier.Add(child);
                }
            }
        }

        return new List<Vertex>();
    }

    private List<Vertex> BuildPath(int srcId, int dstId, ref int[] prevList) {
        List<Vertex> path = new List<Vertex>();
        int prev = dstId;
        do {
            path.Add(vertices[prev]);
            prev = prevList[prev];
        } while (prev != srcId);
        return path;
    }
}
