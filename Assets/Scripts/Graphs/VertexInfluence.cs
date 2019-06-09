using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertexInfluence : Vertex {
    public Faction faction;
    public Guild guild;
    public float value = 0f;

    public bool SetValue(Faction f, float v) {
        bool isUpdated = false;
        if (v > value) {
            value = v;
            faction = f;
            isUpdated = true;
        }
        return isUpdated;
    }
}
