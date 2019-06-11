using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundReceiver : MonoBehaviour {
    public float soundThreshold;

    public virtual void Receive(float intensity, Vector3 position) {
        //TODO
        //code behaviour
        Debug.LogFormat("---> Can hear sound from: {0} with intensity: {1}", position, intensity);
    }
}
