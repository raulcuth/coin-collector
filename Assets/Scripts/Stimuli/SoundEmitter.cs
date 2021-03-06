﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEmitter : MonoBehaviour {
    public float soundIntensity;
    public float soundAttenuation;
    public GameObject emitterObject;
    public Dictionary<string, float> wallTypes;
    private Dictionary<int, SoundReceiver> receiverDictionary;

    private void Start() {
        receiverDictionary = new Dictionary<int, SoundReceiver>();
        if (emitterObject == null) {
            emitterObject = gameObject;
        }
    }

    private void OnTriggerEnter(Collider coll) {
        SoundReceiver receiver = coll.gameObject.GetComponent<SoundReceiver>();
        if (receiver == null) {
            return;
        }
        int objId = coll.gameObject.GetInstanceID();
        receiverDictionary.Add(objId, receiver);
    }

    private void OnTriggerExit(Collider coll) {
        SoundReceiver receiver = coll.gameObject.GetComponent<SoundReceiver>();
        if (receiver == null) {
            return;
        }
        int objId = coll.gameObject.GetInstanceID();
        receiverDictionary.Remove(objId);
    }

    public void Emit() {
        GameObject srObj;
        Vector3 srPos;
        float intensity;
        float distance;
        Vector3 emitterPos = emitterObject.transform.position;
        //compute sound attenuation for receiver
        foreach (SoundReceiver sr in receiverDictionary.Values) {
            srObj = sr.gameObject;
            srPos = srObj.transform.position;
            distance = Vector3.Distance(srPos, emitterPos);
            intensity = soundIntensity;
            intensity -= soundAttenuation * distance;
            if (intensity < sr.soundThreshold) {
                continue;
            }
            sr.Receive(intensity, emitterPos);
        }
    }

    public float GetWallAttenuation(Vector3 emitterPos, Vector3 receiverPos) {
        float attenuation = 0f;
        Vector3 direction = receiverPos - emitterPos;
        float distance = direction.magnitude;
        direction.Normalize();
        Ray ray = new Ray(emitterPos, direction);
        RaycastHit[] hits = Physics.RaycastAll(ray, distance);
        for (int i = 0; i < hits.Length; i++) {
            GameObject obj = hits[i].collider.gameObject;
            string tag = obj.tag;
            if (wallTypes.ContainsKey(tag)) {
                attenuation += wallTypes[tag];
            }
        }
        return attenuation;
    }

}
