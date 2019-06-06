using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : Task {
    public override void SetResult(bool r) {
        if (r) {
            isFinished = true;
        }
    }
    public override IEnumerator RunTask() {
        foreach (Task t in children) {
            yield return StartCoroutine(t.RunTask());
        }
    }
}
