using System.Collections;
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
