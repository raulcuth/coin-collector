using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionAnd : Condition {
    public Condition conditionA;
    public Condition conditionB;

    public override bool Test() {
        if (conditionA.Test() && conditionB.Test()) {
            return true;
        }
        return false;
    }
}
