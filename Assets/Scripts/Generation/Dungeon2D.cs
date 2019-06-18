using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon2D : MonoBehaviour {
    public float minAcceptSize;
    public Rect area;
    public Dictionary<int, List<DungeonNode2D>> tree;
    public HashSet<DungeonNode2D> leaves;
    public delegate Rect[] Split(Rect area);
    public Split splitCall;
    public DungeonNode2D root;

    public void Init() {
        leaves.Clear();
        tree.Clear();
        if (splitCall == null) {
            splitCall = SplitNode;
        }
        root = new DungeonNode2D(area, this);
    }

    public void Build() {
        root.Split(splitCall);
        foreach (DungeonNode2D node in leaves) {
            node.CreateBlock();
        }
    }

    private void Awake() {
        tree = new Dictionary<int, List<DungeonNode2D>>();
        leaves = new HashSet<DungeonNode2D>();
    }

    public Rect[] SplitNode(Rect area) {
        Rect[] areas = null;
        DungeonNode2D[] children = null;
        //check whether the area's width or height are below the minimum. If so, return that area
        float value = Mathf.Min(area.width, area.height);
        if (value < minAcceptSize) {
            return areas;
        }
        //check the greater value between the width and height
        areas = new Rect[2];
        bool isHeightMax = area.height > area.width;
        float half;

        //split by height, if the height is the maximum value
        if (isHeightMax) {
            half = area.height / 2f;
            areas[0] = new Rect(area);
            areas[0].height = half;
            areas[1] = new Rect(area);
            areas[1].y = areas[0].y + areas[0].height;
        } else {
            half = area.width / 2f;
            areas[0] = new Rect(area);
            areas[0].width = half;
            areas[1] = new Rect(area);
            areas[1].x = areas[0].x + areas[0].width;
        }
        return areas;
    }
}
