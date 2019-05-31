using UnityEngine;

public class AvoidWall : Seek {
    public float avoidDistance;
    public float lookAhead;

    public override void Awake() {
        base.Awake();
        target = new GameObject();
    }

    public override Steering GetSteering() {
        //declare variables needed for ray casting
        Steering steering = new Steering();
        Vector3 position = transform.position;
        Vector3 rayVector = agent.velocity.normalized * lookAhead;
        Vector3 direction = rayVector;
        RaycastHit hit;

        //cast the ray and make the proper calculations if a wall is hit
        if (Physics.Raycast(position, direction, out hit, lookAhead)) {
            position = hit.point + hit.normal * avoidDistance;
            target.transform.position = position;
            steering = base.GetSteering();
        }

        return steering;
    }
}
