using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour {
    public List<GameObject> nodes;
    List<PathSegment> segments;

    private void Start() {
        segments = GetSegments();
    }

    public List<PathSegment> GetSegments() {
        List<PathSegment> segmentsList = new List<PathSegment>();
        for (int i = 0; i < nodes.Count - 1; i++) {
            Vector3 src = nodes[i].transform.position;
            Vector3 dst = nodes[i + 1].transform.position;
            PathSegment segment = new PathSegment(src, dst);
            segmentsList.Add(segment);
        }
        return segmentsList;
    }

    public float GetParam(Vector3 position, float lastParam) {
        //find the segment that the agent is closest to
        float param = 0f;
        PathSegment currentSegment = null;
        float tempParam = 0f;
        foreach (PathSegment ps in segments) {
            tempParam += Vector3.Distance(ps.a, ps.b);
            if (lastParam <= tempParam) {
                currentSegment = ps;
                break;
            }
        }
        if (currentSegment == null) {
            return 0f;
        }

        //given the current position, work out the direction to go to
        Vector3 currPos = position - currentSegment.a;
        Vector3 segmentDirection = currentSegment.b - currentSegment.a;
        segmentDirection.Normalize();

        //find the point in the segment using vector projection
        Vector3 pointInSegment = Vector3.Project(currPos, segmentDirection);

        //return the next position to reach along the path
        param = tempParam - Vector3.Distance(currentSegment.a, currentSegment.b);
        param += pointInSegment.magnitude;

        return param;
    }

    public Vector3 GetPosition(float param) {
        //given the current location along the path, find the corresponding segment
        Vector3 position = Vector3.zero;
        PathSegment currentSegment = null;
        float tempParam = 0f;
        foreach (PathSegment ps in segments) {
            tempParam += Vector3.Distance(ps.a, ps.b);
            if (param <= tempParam) {
                currentSegment = ps;
                break;
            }
        }

        if (currentSegment == null) {
            return Vector3.zero;
        }

        Vector3 segmentDirection = currentSegment.b - currentSegment.a;
        segmentDirection.Normalize();
        tempParam -= Vector3.Distance(currentSegment.a, currentSegment.b);
        tempParam = param - tempParam;
        position = currentSegment.a + segmentDirection * tempParam;

        return position;
    }

    private void OnDrawGizmos() {
        Vector3 direction;
        Color tmp = Gizmos.color;
        Gizmos.color = Color.magenta;
        for (int i=0;i<nodes.Count -1; i++) {
            Vector3 src = nodes[i].transform.position;
            Vector3 dst = nodes[i + 1].transform.position;
            direction = dst - src;
            Gizmos.DrawRay(src, direction);
        }
        Gizmos.color = tmp;
    }
}
