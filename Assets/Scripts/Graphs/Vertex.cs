using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Vertex : MonoBehaviour {
    public int id;
    public List<Edge> neighbours;
    [HideInInspector]
    public Vertex prev;
}
