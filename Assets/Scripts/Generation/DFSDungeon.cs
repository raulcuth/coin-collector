using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DFSDungeon : MonoBehaviour {
    public int width;
    public int height;
    public bool[,] dungeon;
    public bool[,] visited;
    private Stack<Vector2> stack;
    private Vector2 current;
    private int size;

    private void Init() {
        stack = new Stack<Vector2>();
        size = width * height;
        dungeon = new bool[height, width];
        visited = new bool[height, width];
        current.x = Random.Range(0, width - 1);
        current.y = Random.Range(0, height - 1);
        //assign a wall to all the cells in the dungeon
        int i, j;
        for (j = 0; j < height; j++) {
            for (i = 0; i < width; i++) {
                dungeon[j, i] = true;
            }
        }
        //insert the initial position into the stack
        stack.Push(current);
        i = (int)current.x;
        j = (int)current.y;
        //mark that cell as visited, reducing the number of available cells
        visited[j, i] = true;
        size--;
    }

    private Vector2[] GetNeighbours(Vector2 node) {
        List<Vector2> neighbours = new List<Vector2>();
        int originX, targetX, originY, targetY;
        originX = (int)node.x - 1;
        originY = (int)node.y - 1;
        targetX = (int)node.x + 1;
        targetY = (int)node.y + 1;
        int i, j;

        //iterate through the cells and add only the valid and available cells
        for (j = originY; j < targetY; j++) {
            if (j < 0 || j >= height) {
                continue;
            }
            for (i = originX; i < targetX; i++) {
                if (i < 0 || i >= width) {
                    if (i == node.x && j == node.y) {
                        continue;
                    }
                }
                if (visited[j, i]) {
                    continue;
                }
                neighbours.Add(new Vector2(i, j));
            }
        }
        return neighbours.ToArray();
    }

    public void Build() {
        Init();
        while (size > 0) {
            Vector2[] neighbours = GetNeighbours(current);
            if (neighbours.Length > 0) {
                stack.Push(current);

                //select a random neighbour and remove the wall between the neighbour and the current cell
                int rand = Random.Range(0, neighbours.Length - 1);
                Vector2 n = neighbours[rand];
                int i = (int)current.y;
                int j = (int)current.x;
                dungeon[j, i] = false;
                i = (int)n.y;
                j = (int)n.x;
                dungeon[j, i] = false;

                //mark the neighbour as visited
                visited[j, i] = true;
                current = n;
                size--;
            } else if (stack.Count > 0) {
                current = stack.Pop();
            }
        }
    }
}
