﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smeller : MonoBehaviour {
    private Vector3 target;
    private Dictionary<int, GameObject> particles;

    private void Start() {
        particles = new Dictionary<int, GameObject>();
    }

    //add the colliding objects that have the OdourParticle component attached
    private void OnTriggerEnter(Collider coll) {
        GameObject obj = coll.gameObject;
        OdourParticle op = obj.GetComponent<OdourParticle>();
        if (op == null) {
            return;
        }
        int objId = obj.GetInstanceID();
        particles.Add(objId, obj);
        UpdateTarget();
    }

    //release the odour particles from the local disctionary when they are out
    //of range or after they have been destroyed
    public void OnTriggerExit(Collider coll) {
        GameObject obj = coll.gameObject;
        int objId = obj.GetInstanceID();
        bool isRemoved = particles.Remove(objId);
        if (!isRemoved) {
            return;
        }
        UpdateTarget();
    }

    //compute the odor centroid according to the current elements in the dictionary
    private void UpdateTarget() {
        Vector3 centroid = Vector3.zero;
        foreach (GameObject p in particles.Values) {
            Vector3 pos = p.transform.position;
            centroid += pos;
        }
        target = centroid;
    }

    //return the odour centroids, if any
    public Vector3? GetTargetPosition() {
        if (particles.Keys.Count == 0) {
            return null;
        }
        return target;
    }
}
