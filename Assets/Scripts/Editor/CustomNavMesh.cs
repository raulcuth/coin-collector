using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomNavMesh : GraphVisibility {
    public override void Start() {
       instIdToId = new Dictionary<int, int>();
    }
}
