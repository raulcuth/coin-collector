using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour {
    public GameObject vertexPrefab;
    protected List<Vertex> vertices;
    protected List<List<Vertex>> neighbours;
    protected List<List<float>> costs;
    protected Dictionary<int, int> instIdToId;
    public delegate float Heuristic(Vertex a, Vertex b);
    public List<Vertex> path;
    public bool isFinished;

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

    public List<Vertex> GetPathAStar(GameObject srcObj, GameObject dstObj, Heuristic h = null) {
        if (srcObj == null || dstObj == null) {
            return new List<Vertex>();
        }
        if (ReferenceEquals(h, null)) {
            h = EuclidDist;
        }

        Vertex src = GetNearestVertex(srcObj.transform.position);
        Vertex dst = GetNearestVertex(dstObj.transform.position);
        BinaryHeap<Edge> frontier = new BinaryHeap<Edge>();
        Edge[] edges;
        Edge node, child;
        int size = vertices.Count;
        float[] distValue = new float[size];
        int[] previous = new int[size];

        //add the source node to the heap(working as a priority queue) and assign
        //a distance value of infinity to all of them
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

        //declare the loop for traversing the graph
        while (frontier.Count != 0) {
            //conditions for returning a path when necessary
            node = frontier.Remove();
            int nodeId = node.vertex.id;
            if (ReferenceEquals(node.vertex, dst)) {
                return BuildPath(src.id, node.vertex.id, ref previous);
            }
            //get the vertex's neighbours
            edges = GetEdges(node.vertex);
            //traverse the neighbours for computing the cost function
            foreach (Edge e in edges) {
                int eId = e.vertex.id;
                if (previous[eId] != -1) {
                    continue;
                }
                float cost = distValue[nodeId] + e.cost;
                cost += h(node.vertex, e.vertex);

                //expand the list of explored nodes and update the costs, if necessary
                if (cost < distValue[e.vertex.id]) {
                    distValue[eId] = cost;
                    previous[eId] = nodeId;
                    frontier.Remove();
                    child = new Edge(e.vertex, cost);
                    frontier.Add(child);
                }
            }
        }
        return new List<Vertex>();
    }

    public List<Vertex> GetPathIDAStar(GameObject srcObj, GameObject dstObj, Heuristic h = null) {
        if (srcObj == null || dstObj == null) {
            return new List<Vertex>();
        }
        if (ReferenceEquals(h, null)) {
            h = EuclidDist;
        }

        List<Vertex> path = new List<Vertex>();
        Vertex src = GetNearestVertex(srcObj.transform.position);
        Vertex dst = GetNearestVertex(dstObj.transform.position);
        Vertex goal = null;
        bool[] visited = new bool[vertices.Count];
        for (int i = 0; i < visited.Length; i++) {
            visited[i] = false;
        }
        visited[src.id] = true;

        //algorithm loop
        float bound = h(src, dst);
        while (bound < Mathf.Infinity) {
            bound = RecursiveIDAStar(src, dst, bound, h, ref goal, ref visited);
        }
        if (ReferenceEquals(goal, null)) {
            return path;
        }
        return BuildPath(goal);
    }

    private float RecursiveIDAStar(Vertex v,
                                   Vertex dst,
                                   float bound,
                                   Heuristic h,
                                   ref Vertex goal,
                                   ref bool[] visited) {
        //base case
        if (ReferenceEquals(v, dst)) {
            return Mathf.Infinity;
        }
        Edge[] edges = GetEdges(v);
        if (edges.Length == 0) {
            return Mathf.Infinity;
        }

        //recursive case
        float fn = Mathf.Infinity;
        foreach (Edge e in edges) {
            int eId = e.vertex.id;
            if (visited[eId]) {
                continue;
            }
            visited[eId] = true;
            e.vertex.prev = v;
            float f = h(v, dst);
            float b;
            if (f <= bound) {
                b = RecursiveIDAStar(e.vertex, dst, bound, h, ref goal, ref visited);
                fn = Mathf.Min(f, b);
            } else {
                fn = Mathf.Min(fn, f);
            }
        }
        return fn;
    }

    public float EuclidDist(Vertex a, Vertex b) {
        Vector3 posA = a.transform.position;
        Vector3 posB = b.transform.position;
        return Vector3.Distance(posA, posB);
    }

    public float ManhattanDist(Vertex a, Vertex b) {
        Vector3 posA = a.transform.position;
        Vector3 posB = b.transform.position;
        return Mathf.Abs(posA.x - posB.x) + Mathf.Abs(posA.y - posB.y);
    }

    public IEnumerator GetPathInFrames(GameObject srcObj, GameObject dstObj, Heuristic h = null) {
        isFinished = false;
        path = new List<Vertex>();
        if (srcObj == null || dstObj == null) {
            path = new List<Vertex>();
            isFinished = true;
            yield break;
        }

        if (ReferenceEquals(h, null)) {
            h = EuclidDist;
        }

        Vertex src = GetNearestVertex(srcObj.transform.position);
        Vertex dst = GetNearestVertex(dstObj.transform.position);
        BinaryHeap<Edge> frontier = new BinaryHeap<Edge>();

        Edge[] edges;
        Edge node, child;
        int size = vertices.Count;
        float[] distValue = new float[size];
        int[] previous = new int[size];
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

        while (frontier.Count != 0) {
            yield return null;
            node = frontier.Remove();
            int nodeId = node.vertex.id;
            if (ReferenceEquals(node.vertex, dst)) {
                path = BuildPath(src.id, node.vertex.id, ref previous);
                break;
            }
            edges = GetEdges(node.vertex);

            foreach (Edge e in edges) {
                int eId = e.vertex.id;
                if (previous[eId] != -1) {
                    continue;
                }
                float cost = distValue[nodeId] + e.cost;
                cost += h(node.vertex, e.vertex);

                if (cost < distValue[e.vertex.id]) {
                    distValue[eId] = cost;
                    previous[eId] = nodeId;
                    frontier.Remove(e);
                    child = new Edge(e.vertex, cost);
                    frontier.Add(child);
                }
            }
        }
        isFinished = true;
        yield break;
    }

    //We create a new path, taking the initial node as a starting point, and apply 
    //ray casting to the next node in the path until we get a collision with
    //wall between our current node and the target node.When that happens, we 
    //take the last clear node, put it in the new path, and set it as the 
    //current node to start casting rays again.The process continues until there 
    //are no nodes left to check and the current node is the target node.
    //That way, we build a more intuitive path.
    public List<Vertex> Smooth(List<Vertex> path) {
        List<Vertex> newPath = new List<Vertex>();
        //check whether it is worth computing a new path
        if (path.Count == 0) {
            return newPath;
        }
        if (path.Count < 3) {
            return path;
        }

        //implement the loop for traversing the list and building the new path
        newPath.Add(path[0]);
        int i, j;
        for (i = 0; i < path.Count - 1;) {
            for (j = i + 1; j < path.Count; j++) {
                //variables used by the ray casting function
                Vector3 origin = path[i].transform.position;
                Vector3 destination = path[j].transform.position;
                Vector3 direction = destination - origin;
                float distance = direction.magnitude;
                bool isWall = false;
                direction.Normalize();

                //cast a ray from the current starting node to the next one
                Ray ray = new Ray(origin, direction);
                RaycastHit[] hits;
                hits = Physics.RaycastAll(ray, distance);

                //check whether there is a wall and break the loop accordingly
                foreach (RaycastHit hit in hits) {
                    string tag = hit.collider.gameObject.tag;
                    if (tag.Equals("Wall")) {
                        isWall = true;
                        break;
                    }
                }
                if (isWall) {
                    break;
                }
            }
            i = j - 1;
            newPath.Add(path[i]);
        }
        return newPath;
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

    private List<Vertex> BuildPath(Vertex v) {
        List<Vertex> path = new List<Vertex>();
        while (!ReferenceEquals(v, null)) {
            path.Add(v);
            v = v.prev;
        }
        return path;
    }
}
