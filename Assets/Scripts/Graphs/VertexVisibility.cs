using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertexVisibility : Vertex {
    private void Awake() {
        neighbours = new List<Edge>();
    }

    public void FindNeighbours(List<Vertex> vertices) {
        Collider c = gameObject.GetComponent<Collider>();
        c.enabled = false;
        Vector3 direction = Vector3.zero;
        Vector3 origin = transform.position;
        Vector3 target = Vector3.zero;
        RaycastHit[] hits;
        Ray ray;
        float distance = 0f;

        //get over each object and cast a ray to validate whether it's completely
        //visible and add it to the list of neighbours
        for (int i = 0; i < vertices.Count; i++) {
            if (vertices[i] == this) {
                continue;
            }
            target = vertices[i].transform.position;
            direction = target - origin;
            distance = direction.magnitude;
            ray = new Ray(origin, direction);
            hits = Physics.RaycastAll(ray, distance);
            if (hits.Length == 1) {
                if (hits[0].collider.gameObject.tag.Equals("Vertex")) {
                    Edge e = new Edge();
                    e.cost = distance;
                    GameObject go = hits[0].collider.gameObject;
                    Vertex v = go.GetComponent<Vertex>();
                    if (v != vertices[i]) {
                        continue;
                    }
                    e.vertex = v;
                    neighbours.Add(e);
                }
            }
        }
        c.enabled = true;
    }
}
