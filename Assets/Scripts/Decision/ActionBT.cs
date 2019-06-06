using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBT : Task {
    public override IEnumerator Run() {
        isFinished = false;
        //implement behaviour
        return base.Run();
    }
}
