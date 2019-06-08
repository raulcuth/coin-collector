using System.Collections.Generic;
using UnityEngine;

public class Lurker : MonoBehaviour {
    [HideInInspector]
    public List<Vertex> path;
    private void Awake() {
        if (ReferenceEquals(path, null)) {
            path = new List<Vertex>();
        }
    }
}
