using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task : MonoBehaviour {
    public List<Task> children;
    protected bool result = false;
    protected bool isFinished = false;

    //finalization function
    public virtual void SetResult(bool r) {
        result = r;
        isFinished = true;
    }

    //function for creating behaviours
    public virtual IEnumerator Run() {
        SetResult(true);
        yield break;
    }

    //general function for starting behaviours
    public virtual IEnumerator RunTask() {
        yield return StartCoroutine(Run());
    }
}
