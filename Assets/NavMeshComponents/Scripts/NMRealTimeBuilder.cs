using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NavMeshBuilder = UnityEngine.AI.NavMeshBuilder;

//We take the size of the box around our agent, and the required difference 
//between the box around the previous frame and the current one.If that 
//difference is greater than the percentage we declared in sizeChange, 
//the NavMeshBuilder takes the new position, disposes of the previous 
//navigation mesh, and creates a new one
//can be changed to get the direction of the target and then tell the agent 
//to walk to the farthest point of the NavMesh in that direction
public class NMRealTimeBuilder : MonoBehaviour {
    public Transform agent;
    public Vector3 boxSize = new Vector3(50f, 20f, 50f);
    [Range(0.01f, 1f)]
    public float sizeChange = 0.1f;
    private NavMeshData navMesh;
    private AsyncOperation operation;
    private NavMeshDataInstance navMeshInstance;
    private List<NavMeshBuildSource> sources = new List<NavMeshBuildSource>();

    static private Vector3 Quantize(Vector3 a, Vector3 q) {
        float x = q.x * Mathf.Floor(a.x / q.x);
        float y = q.y * Mathf.Floor(a.y / q.y);
        float z = q.z * Mathf.Floor(a.z / q.z);
        return new Vector3(x, y, z);
    }

    private Bounds QuantizeBounds() {
        Vector3 position = agent.transform.position;
        return new Bounds(Quantize(position, boxSize * sizeChange), boxSize);
    }

    private void UpdateNavMesh(bool isAsync = false) {
        NavMeshSourceTag.Collect(ref sources);
        NavMeshBuildSettings settings;
        settings = NavMesh.GetSettingsByID(0);
        Bounds bounds = QuantizeBounds();
        if (isAsync) {
            operation = NavMeshBuilder.UpdateNavMeshDataAsync(navMesh, settings, sources, bounds);
        } else {
            NavMeshBuilder.UpdateNavMeshData(navMesh, settings, sources, bounds);
        }
    }

    private void Awake() {
        if (agent == null) {
            agent = transform;
        }
    }

    private void OnEnable() {
        navMesh = new NavMeshData();
        navMeshInstance = NavMesh.AddNavMeshData(navMesh);
        UpdateNavMesh(false);
    }

    private void OnDisable() {
        navMeshInstance.Remove();
    }
}
