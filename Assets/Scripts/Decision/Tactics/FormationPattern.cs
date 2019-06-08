using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationPattern : MonoBehaviour {
    public int numOfSlots;
    public GameObject leader;

    private void Start() {
        if (leader == null) {
            leader = transform.gameObject;
        }
    }

    //get the position of a given slot
    public virtual Vector3 GetSlotLocation(int slotIndex) {
        return Vector3.zero;
    }

    //check given number of counts is supported by the function
    public bool SupportsSlots(int slotCount) {
        return slotCount < numOfSlots;
    }

    //set the offset in the location if necessary
    public virtual Location GetDriftOffset(List<SlotAssignment> slotAssignments) {
        Location location = new Location();
        location.position = leader.transform.position;
        location.rotation = leader.transform.rotation;
        return location;
    }
}
