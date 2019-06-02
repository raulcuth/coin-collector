using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour {
    public GameObject vertexPrefab;
    protected List<Vertex> vertices;
    protected List<List<Vertex>> neighbours;
    protected List<List<float>> costs;

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
}
