using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationManager : MonoBehaviour {
    public FormationPattern pattern;
    private List<SlotAssignment> slotAssignments;
    private Location driftOffset;

    private void Awake() {
        slotAssignments = new List<SlotAssignment>();
    }

    //update the slot assignment given the list's order
    public void UpdateSlotAssignments() {
        for (int i = 0; i < slotAssignments.Count; i++) {
            slotAssignments[i].slotIndex = i;
        }
        driftOffset = pattern.GetDriftOffset(slotAssignments);
    }

    //add a character to the formation
    public bool AddCharacter(GameObject character) {
        int occupiedSlots = slotAssignments.Count;
        //check if there is a slot left in the formation
        if (!pattern.SupportsSlots(occupiedSlots + 1)) {
            return false;
        }
        SlotAssignment sa = new SlotAssignment();
        sa.character = character;
        slotAssignments.Add(sa);
        UpdateSlotAssignments();
        return true;
    }

    //remove a character from the formation
    public void RemoveCharacter(GameObject agent) {
        int index = slotAssignments.FindIndex(x => x.character.Equals(agent));
        slotAssignments.RemoveAt(index);
        UpdateSlotAssignments();
    }

    //update the slots
    public void UpdateSlots() {
        GameObject leader = pattern.leader;
        Vector3 anchor = leader.transform.position;
        Vector3 slotPos;
        Quaternion rotation = leader.transform.rotation;
        foreach (SlotAssignment sa in slotAssignments) {
            Vector3 relPos = anchor;
            slotPos = pattern.GetSlotLocation(sa.slotIndex);
            relPos += leader.transform.TransformDirection(slotPos);
            Location charDrift = new Location(relPos, rotation);
            Character character = sa.character.GetComponent<Character>();
            character.SetTarget(charDrift);
        }
    }
}
