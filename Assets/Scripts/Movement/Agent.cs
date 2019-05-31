using UnityEngine;

public class Agent : MonoBehaviour {
    public float maxSpeed;
    public float maxAccel;
    public float maxRotation;
    public float maxAngularAccel;
    public float orientation;
    public float rotation;
    public Vector3 velocity;
    protected Steering steering;
    private Rigidbody aRigidBody;

    private void Start() {
        velocity = Vector3.zero;
        steering = new Steering();
        aRigidBody = GetComponent<Rigidbody>();
    }

    public void SetSteering(Steering steering) {
        this.steering = steering;
    }

    public virtual void Update() {
        if (aRigidBody == null) {
            return;
        }
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

    public virtual void FixedUpdate() {
        if (aRigidBody == null) {
            return;
        }
        Vector3 displacement = velocity * Time.deltaTime;
        orientation += rotation * Time.deltaTime;
        if (orientation < 0.0f) {
            orientation += 360.0f;
        } else if (orientation > 360.0f) {
            orientation -= 360.0f;
        }
        //the force mode will depend on what you want to achieve
        //we are using VelocityChange for illustration purposes
        aRigidBody.AddForce(displacement, ForceMode.VelocityChange);
        Vector3 orientationVector = OrientationToVector(orientation);
        aRigidBody.rotation = Quaternion.LookRotation(orientationVector, Vector3.up);
    }

    //function for transforming an orientation value to vector
    public Vector3 OrientationToVector(float orientation) {
        Vector3 vector = Vector3.zero;
        vector.x = Mathf.Sin(orientation * Mathf.Deg2Rad) * 1.0f;
        vector.z = Mathf.Cos(orientation * Mathf.Deg2Rad) * 1.0f;
        return vector.normalized;
    }
}
