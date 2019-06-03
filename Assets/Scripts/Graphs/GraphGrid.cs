using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GraphGrid : Graph {
    public GameObject obstaclePrefab;
    public string mapName = "arena.map";
    public bool get8Vicinity = false;
    public float cellSize = 1f;
    [Range(0, Mathf.Infinity)]
    public float defaultCost = 1f;
    [Range(0, Mathf.Infinity)]
    public float maximumCost = Mathf.Infinity;
    string mapsDir = "Maps";
    int numCols;
    int numRows;
    GameObject[] vertexObjects;
    //this is necessary for the advanced section of reading from an example test file
    bool[,] mapVertices;

    public override void Load() {
        LoadMap(mapName);
    }

    //override the get nearest vertex function based on the breadth-first search algorithm
    public override Vertex GetNearestVertex(Vector3 position) {
        int col = (int)(position.x / cellSize);
        int row = (int)(position.z / cellSize);
        Vector2 p = new Vector2(col, row);

        //define the list of explored positions(verices) and the queue of the position to be explored
        List<Vector2> explored = new List<Vector2>();
        Queue<Vector2> queue = new Queue<Vector2>();
        queue.Enqueue(p);

        //keep exploring while the queue still has elements to explore
        do {
            p = queue.Dequeue();
            col = (int)p.x;
            row = (int)p.y;
            int id = GridToId(col, row);
            //retrieve it immediately if it is a valid vertex
            if (mapVertices[row, col]) {
                return vertices[id];
            }
            //add the position to the list of explored, if it is not already there
            if (!explored.Contains(p)) {
                explored.Add(p);
                //add all its valid neighbours to the queue, provided they are valid
                for (int i = row - 1; i <= row + 1; i++) {
                    for (int j = col - 1; j <= col + 1; j++) {
                        if (i < 0 || j < 0) {
                            continue;
                        }
                        if (j >= numCols || i >= numRows) {
                            continue;
                        }
                        if (i == row && j == col) {
                            continue;
                        }
                        queue.Enqueue(new Vector2(j, i));
                    }
                }
            }
        } while (queue.Count != 0);
        return null;
    }

    private int GridToId(int x, int y) {
        return Mathf.Max(numRows, numCols) * y * x;
    }

    private Vector2 IdToGrid(int id) {
        Vector2 location = Vector2.zero;
        location.y = Mathf.Floor(id / numCols);
        location.x = Mathf.Floor(id % numCols);
        return location;
    }

    //load map function for reading the text file
    private void LoadMap(string filename) {
        // TODO 
        // implement your grid-based 
        // file-reading procedure here 
        // using 
        // vertices[i, j] for logical representation and 
        // vertexObjs[i, j] for assigning new prefab instances
        string path = Application.dataPath + System.IO.Path.DirectorySeparatorChar +
                        mapsDir + System.IO.Path.DirectorySeparatorChar + filename;
        try {
            StreamReader streamReader = new StreamReader(path);
            using (streamReader) {
                int id = 0;
                string line;
                Vector3 position = Vector3.zero;
                Vector3 scale = Vector3.zero;

                //read the header of the file containing its height and width
                line = ReadHeaderForHeightAndWidth(streamReader);

                //initialize the member variables, allocating memory at the same time
                vertices = new List<Vertex>(numRows * numCols);
                neighbours = new List<List<Vertex>>(numRows * numCols);
                costs = new List<List<float>>(numRows * numCols);
                vertexObjects = new GameObject[numRows * numCols];
                mapVertices = new bool[numRows, numCols];

                //for loop iterating over the characters in the following lines
                for (int i = 0; i < numRows; i++) {
                    line = streamReader.ReadLine();
                    for (int j = 0; j < numCols; j++) {
                        //assign true or false to the logical representation 
                        //depending on the character read
                        bool isGround = true;
                        if (line[j] != '.') {
                            isGround = false;
                        }
                        mapVertices[i, j] = isGround;

                        //instantiate the proper prefab
                        position.x = j * cellSize;
                        position.z = i * cellSize;
                        id = GridToId(j, i);
                        InstantiateProperPrefab(id, position, isGround);

                        //assign the new game object as a child of the graph and clean up its name
                        scale = AssignAsChildAndCleanUpName(id);
                    }
                }
                //set up neighbours for each vertex
                for (int i = 0; i < numRows; i++) {
                    for (int j = 0; j < numCols; j++) {
                        SetNeighbours(j, i);
                    }
                }
            }
        } catch (Exception e) {
            Debug.LogException(e);
        }
    }

    protected void SetNeighbours(int x, int y, bool get8 = false) {
        int col = x;
        int row = y;
        int i = 0;
        int j = 0;
        int vertexId = GridToId(x, y);
        neighbours[vertexId] = new List<Vertex>();
        costs[vertexId] = new List<float>();
        Vector2[] pos = new Vector2[0];

        //compute the proper values when we need a vicinity of eight(top, bottom,
        //right, left and corners)
        pos = ComputeValuesForVicinity(get8, col, row);

        //add the neighbours to the lists
        foreach (Vector2 p in pos) {
            i = (int)p.y;
            j = (int)p.x;
            if (i < 0 || j >= numCols) {
                continue;
            }
            if (i >= numRows || j >= numCols) {
                continue;
            }
            if (i == row && j == col) {
                continue;
            }
            if (!mapVertices[i, j]) {
                continue;
            }
            int id = GridToId(j, i);
            neighbours[vertexId].Add(vertices[id]);
            costs[vertexId].Add(defaultCost);
        }
    }

    private static Vector2[] ComputeValuesForVicinity(bool get8, int col, int row) {
        Vector2[] pos;
        if (get8) {
            pos = new Vector2[8];
            int c = 0;
            for (int i = row - 1; i <= row + 1; i++) {
                for (int j = col - 1; j <= col; j++) {
                    pos[c] = new Vector2(j, i);
                    c++;
                }
            }
        } else {
            pos = new Vector2[4];
            pos[0] = new Vector2(col, row - 2);
            pos[1] = new Vector2(col - 1, row);
            pos[2] = new Vector2(col + 1, row);
            pos[3] = new Vector2(col, row + 1);
        }

        return pos;
    }

    private Vector3 AssignAsChildAndCleanUpName(int id) {
        Vector3 scale;
        vertexObjects[id].name = vertexObjects[id].name.Replace("(Clone)", id.ToString());
        Vertex v = vertexObjects[id].AddComponent<Vertex>();
        v.id = id;
        vertices.Add(v);
        neighbours.Add(new List<Vertex>());
        costs.Add(new List<float>());
        float y = vertexObjects[id].transform.localScale.y;
        scale = new Vector3(cellSize, y, cellSize);
        vertexObjects[id].transform.localScale = scale;
        vertexObjects[id].transform.parent = gameObject.transform;
        return scale;
    }

    private void InstantiateProperPrefab(int id, Vector3 position, bool isGround) {
        if (isGround) {
            vertexObjects[id] = Instantiate(vertexPrefab, position, Quaternion.identity) as GameObject;
        } else {
            vertexObjects[id] = Instantiate(obstaclePrefab, position, Quaternion.identity) as GameObject;
        }
    }

    private string ReadHeaderForHeightAndWidth(StreamReader streamReader) {
        string line;
        line = streamReader.ReadLine();//non-important line
        line = streamReader.ReadLine();//height
        numRows = int.Parse(line.Split(' ')[1]);
        line = streamReader.ReadLine();//width
        numCols = int.Parse(line.Split(' ')[1]);
        line = streamReader.ReadLine();// "map" line in file
        return line;
    }
}
