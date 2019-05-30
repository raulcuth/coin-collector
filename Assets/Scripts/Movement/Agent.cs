using UnityEngine;

public class Agent : MonoBehaviour {
    public float maxSpeed;
    public float maxAccel;
    public float orientation;
    public float rotation;
    public Vector3 velocity;
    protected Steering steering;

    private void Start() {
        velocity = Vector3.zero;
        steering = new Steering();
    }

    public void SetSteering(Steering steering) {
        this.steering = steering;
    }

    public virtual void Update() {
        Vector3 displacement = velocity * Time.deltaTime;
        orientation += rotation * Time.deltaTime;
        //we need to limit the orientation values
        //to be in the range (0,360)
        if (orientation < 0.0f) {
            orientation += 360.0f;
        } else if (orientation > 360.0f) {
            orientation -= 360.0f;
        }
        transform.Translate(displacement, Space.World);
        transform.rotation = new Quaternion();
        transform.Rotate(Vector3.up, orientation);
    }

    public virtual void LateUpdate() {
        velocity += steering.linear * Time.deltaTime;
        rotation += steering.angular * Time.deltaTime;
        if (velocity.magnitude > maxSpeed) {
            velocity.Normalize();
            velocity = velocity * maxSpeed;
        }
        if (steering.angular == 0.0f) {
            rotation = 0.0f;
        }
        if (steering.linear.sqrMagnitude == 0.0f) {
            velocity = Vector3.zero;
        }
        steering = new Steering();
    }
}
