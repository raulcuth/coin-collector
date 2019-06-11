using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visor : MonoBehaviour {
    public string tagWall = "Wall";
    public string tagTarget = "Enemy";
    public GameObject agent;

    private void Start() {
        if (agent == null) {
            agent = gameObject;
        }
    }

    private void OnTriggerStay(Collider coll) {
        //discard collision if it is not a target
        string tag = coll.gameObject.tag;
        if (!tag.Equals(tagTarget)) {
            return;
        }

        //get the game object's position and compute its direction from the visor
        GameObject target = coll.gameObject;
        Vector3 agentPos = agent.transform.position;
        Vector3 targetPos = target.transform.position;
        Vector3 direction = targetPos - agentPos;

        //compute its length and create a new ray to be shot soon
        float length = direction.magnitude;
        direction.Normalize();
        Ray ray = new Ray(agentPos, direction);

        //cast the ray created and retrieve all the hits
        RaycastHit[] hits = Physics.RaycastAll(ray, length);

        for (int i = 0; i < hits.Length; i++) {
            GameObject hitObj = hits[i].collider.gameObject;
            tag = hitObj.tag;
            if (tag.Equals(tagWall)) {
                return;
            }
        }
        //TODO
        //target is visible, code the behaviour
    }

}
