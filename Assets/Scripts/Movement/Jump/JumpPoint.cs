using UnityEngine;

public class JumpPoint {
    public Vector3 jumpLocation;
    public Vector3 landingLocation;

    //the change in position from jump to landing
    public Vector3 deltaPosition;

    public JumpPoint() : this(Vector3.zero, Vector3.zero) {

    }

    public JumpPoint(Vector3 jumpLocation, Vector3 landingLocation) {
        this.jumpLocation = jumpLocation;
        this.landingLocation = landingLocation;
        this.deltaPosition = this.landingLocation - this.jumpLocation;
    }
}
