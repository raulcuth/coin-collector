using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class NavMeshBuilder : MonoBehaviour {
    public NavMeshSurface[] surfaces;

    public void Build() {
        for (int i = 0; i < surfaces.Length; i++) {
            surfaces[i].BuildNavMesh();
        }
    }

    public IEnumerator BuildInFrames(System.Action eventHandler) {
        for (int i = 0; i < surfaces.Length; i++) {
            surfaces[i].BuildNavMesh();
            yield return null;
        }
        if (eventHandler != null) {
            eventHandler.Invoke();
        }
    }
}
