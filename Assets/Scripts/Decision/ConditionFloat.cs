using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionFloat : Condition {
    public float valueMin;
    public float valueMax;
    public float valueTest;

    public override bool Test() {
        if (valueMax >= valueTest && valueTest >= valueMin) {
            return true;
        }
        return false;
    }
}
