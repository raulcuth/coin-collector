using System.Collections;

public class ConditionBT : Task {
    public override IEnumerator Run() {
        isFinished = false;
        bool r = false;
        //implement behaviour
        //define result(r) whether true or false
        SetResult(r);
        yield break;
    }
}
