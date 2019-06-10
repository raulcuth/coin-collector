using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public StateManager stateManager;
    public int slotWeight;
    [HideInInspector]
    public int circleId = -1;
    [HideInInspector]
    public bool isAssigned;
    [HideInInspector]
    public bool isAttacking;
    [HideInInspector]
    public Attack[] attackList;

    private void Start() {
        attackList = gameObject.GetComponents<Attack>();
    }

    //assign a fighting circle
    public void SetCircle(GameObject circleObj = null) {
        int id = -1;
        if (circleObj == null) {
            Vector3 position = transform.position;
            id = stateManager.GetClosestCircle(position);
        } else {
            FightingCircle fc = circleObj.GetComponent<FightingCircle>();
            if (fc != null) {
                id = fc.gameObject.GetInstanceID();
            }
        }
        circleId = id;
    }

    //request a slot from the manager
    public bool RequestSlot() {
        isAssigned = stateManager.GrantSlot(circleId, this);
        return isAssigned;
    }

    //release a slot from the manager
    public void ReleaseSlot() {
        stateManager.ReleaseSlot(circleId, this);
        isAssigned = false;
        circleId = -1;
    }

    //request an attack from the list(the order is the same as in the inspector)
    public bool RequestAttack(int id) {
        return stateManager.GrantAttack(circleId, attackList[id]);
    }

    //attack behaviour
    public virtual IEnumerator Attack() {
        //TODO
        //attack behaviour here
        yield break;
    }
}
