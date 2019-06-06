using UnityEngine;
using System.Collections;

public class DecisionTree : DecisionTreeNode {
    public DecisionTreeNode root;
    public Action actionNew;
    public Action actionOld;

    public override DecisionTreeNode MakeDecision() {
        return root.MakeDecision();
    }

    public void Update() {
        actionNew.activated = false;
        actionOld = actionNew;
        actionNew = root.MakeDecision() as Action;
        if (actionNew == null) {
            actionNew = actionOld;
        }
        actionNew.activated = true;
    }
}
