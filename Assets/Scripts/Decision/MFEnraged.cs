public class MFEnraged : MembershipFunction {
    public override float GetDOM(object input) {
        //if input(hp) is below 30, the enemy is enraged
        if ((int)input <= 30) {
            return 1f;
        }
        return 0f;
    }
}
