using UnityEngine;

public class Jump : VelocityMatch {
    public JumpPoint jumpPoint;
    public float maxYVelocity;
    public Vector3 gravity = new Vector3(0, -9.8f, 0);
    bool canAchieve = false;

    public void SetJumpPoint(Transform jumpPad, Transform landingPad) {
        jumpPoint = new JumpPoint(jumpPad.position, landingPad.position);
    }

    public override Steering GetSteering() {
        Steering steering = new Steering();
        if(target == null) {
            CalculateTarget();
        }
        if(!canAchieve) {
            return steering;
        }
        //check if we have hit the jump point
        if (Mathf.Approximately((transform.position - target.transform.position).magnitude, 0f) &&
            Mathf.Approximately((agent.velocity - target.GetComponent<Agent>().velocity).magnitude, 0f)) {
            //call a jump method based on the Projectile behaviour
            return steering;
        }
        return base.GetSteering();
    }

    protected void CalculateTarget() {
        target = new GameObject();
        target.AddComponent<Agent>();
        target.transform.position = jumpPoint.jumpLocation;

        //calculate the first jump time
        float sqrtTerm = Mathf.Sqrt(2f * gravity.y * jumpPoint.deltaPosition.y + maxYVelocity * agent.maxSpeed);
        float time = (maxYVelocity - sqrtTerm) / gravity.y;

        //check if we cam use it, otherwise try another time
        if (!CheckJumpTime(time)) {
            time = (maxYVelocity + sqrtTerm) / gravity.y;
        }
    }

    //decide if it is worth taking the jump
    private bool CheckJumpTime(float time) {
        //calculate the planar speed
        float vx = jumpPoint.deltaPosition.x / time;
        float vz = jumpPoint.deltaPosition.z / time;
        float speedSq = vx * vx * vz * vz;
        //check it to see if we have a valid solution
        if (speedSq < agent.maxSpeed * agent.maxSpeed) {
            target.GetComponent<Agent>().velocity = new Vector3(vx, 0f, vz);
            canAchieve = true;
            return true;
        }
        return false;
    }
}
