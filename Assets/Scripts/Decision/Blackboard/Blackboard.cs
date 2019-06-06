using System.Collections.Generic;

public class Blackboard {
    public List<BlackboardDatum> entries;
    public List<BlackboardAction> pastActions;
    public List<BlackboardExpert> experts;

    public Blackboard() {
        entries = new List<BlackboardDatum>();
        pastActions = new List<BlackboardAction>();
        experts = new List<BlackboardExpert>();
    }

    public void RunIteration() {
        BlackboardExpert bestExpert = null;
        float maxInsistence = 0f;

        //loop for deciding which expert will run next depending on the problem
        //that needs to be solved
        foreach (BlackboardExpert e in experts) {
            float insistence = e.GetInsistence(this);
            if (insistence > maxInsistence) {
                maxInsistence = insistence;
                bestExpert = e;
            }
        }

        if (bestExpert != null) {
            bestExpert.Run(this);
        }
    }
}
