//class for defining the experts
public class BlackboardExpert {
    public virtual float GetInsistence(Blackboard board) {
        return 0f;
    }
    public virtual void Run(Blackboard board) {
        //create experts for every field and override this
    }
}
