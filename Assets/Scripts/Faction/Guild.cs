using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guild : MonoBehaviour {
    public string guildName;
    public int maxStrength;
    public GameObject baseObject;
    [HideInInspector]
    public int strength;

    public virtual void Awake() {
        strength = maxStrength;
    }

    public virtual float GetDropOff(float distance) {
        float d = Mathf.Pow(1 + distance, 2f);
        return strength / d;
    }
}
