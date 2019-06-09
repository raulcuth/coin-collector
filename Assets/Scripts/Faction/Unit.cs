using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Faction {
    BLUE,
    RED
}

public class Unit : MonoBehaviour {
    public Faction faction;
    public int radius = 1;
    public float influence = 1f;

    public virtual float GetDropOff(int locationDistance) {
        float d = influence / radius * locationDistance;
        return influence - d;
    }
}
