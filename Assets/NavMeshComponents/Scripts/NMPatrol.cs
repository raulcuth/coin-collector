using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NMPatrol : MonoBehaviour {
    public float pointDistance = 0.5f;
    public Transform[] patrolPoints;
    private int currentPoint = 0;
    private NavMeshAgent agent;

    private void Start() {
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        currentPoint = FindClosestPoint();
        GoToPoint(currentPoint);
    }

    private void Update() {
        if (!agent.pathPending && agent.remainingDistance < pointDistance) {
            currentPoint = (currentPoint + 1) % patrolPoints.Length;
            GoToPoint(CalculateNextPoint());
        }
    }

    private int CalculateNextPoint() {
        return (currentPoint + 1) % patrolPoints.Length;
    }

    //find the closest patrol point
    private int FindClosestPoint() {
        int index = -1;
        float distance = Mathf.Infinity;
        Vector3 agentPosition = transform.position;
        Vector3 pointPosition;

        for (int i = 0; i < patrolPoints.Length; i++) {
            pointPosition = patrolPoints[i].position;
            float d = Vector3.Distance(agentPosition, pointPosition);
            if (d < distance) {
                index = i;
                distance = d;
            }
        }
        return index;
    }

    //update the agent's destination point
    private void GoToPoint(int next) {
        if (next < 0 || next >= patrolPoints.Length) {
            return;
        }
        agent.destination = patrolPoints[next].position;
    }
}
