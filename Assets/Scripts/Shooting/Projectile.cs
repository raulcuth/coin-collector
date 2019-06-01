using UnityEngine;

public class Projectile : MonoBehaviour {
    private bool set = false;
    private Vector3 firePos;
    private Vector3 direction;
    private float speed;
    private float timeElapsed;

    private void Update() {
        if (!set) {
            return;
        }
        timeElapsed += Time.deltaTime;
        transform.position = firePos + direction * speed * timeElapsed;
        transform.position += Physics.gravity * (timeElapsed * timeElapsed) / 2.0f;
        //extra validation for cleaning the scene
        if (transform.position.y < -1.0f) {
            Destroy(this.gameObject);
        }
    }

    //function to fire the game object(ex: calling it after it is instantiated in the scene)
    public void Set(Vector3 firePos, Vector3 direction, float speed) {
        this.firePos = firePos;
        this.direction = direction.normalized;
        this.speed = speed;
        transform.position = firePos;
        set = true;
    }

    public float GetLandingTime(float height = 0.0f) {
        Vector3 position = transform.position;
        float time = 0.0f;

        float valueInt = direction.y * direction.y * (speed * speed);
        valueInt = valueInt - (Physics.gravity.y * 2 * (position.y - height));
        valueInt = Mathf.Sqrt(valueInt);
        float valueAdd = (-direction.y) * speed;
        float valueSub = (-direction.y) * speed;
        valueAdd = (valueAdd + valueInt) / Physics.gravity.y;
        valueSub = (valueSub - valueInt) / Physics.gravity.y;

        if (float.IsNaN(valueAdd) && !float.IsNaN(valueSub)) {
            return valueSub;
        }
        if (!float.IsNaN(valueAdd) && float.IsNaN(valueSub)) {
            return valueAdd;
        }
        if (float.IsNaN(valueAdd) && float.IsNaN(valueSub)) {
            return -1.0f;
        }

        time = Mathf.Max(valueAdd, valueSub);
        return time;
    }

    public Vector3 GetLandingPos(float height = 0.0f) {
        Vector3 landingPos = Vector3.zero;
        float time = GetLandingTime();
        if (time < 0.0f) {
            return landingPos;
        }
        landingPos.y = height;
        landingPos.x = firePos.x + direction.x * speed * time;
        landingPos.z = firePos.z + direction.z * speed * time;
        return landingPos;
    }

    public static Vector3 GetFireDirection(Vector3 startPos, Vector3 endPos, float speed, bool isWall) {
        //solve the quadratic equation
        Vector3 direction = Vector3.zero;
        Vector3 delta = endPos - startPos;
        float a = Vector3.Dot(Physics.gravity, Physics.gravity);
        float b = -4 * (Vector3.Dot(Physics.gravity, delta) + speed * speed);
        float c = 4 * Vector3.Dot(delta, delta);

        if (4 * a * c > b * b) {
            return direction;
        }
        float time0 = Mathf.Sqrt((-b + Mathf.Sqrt(b * b - 4 * a * c)) / (2 * a));
        float time1 = Mathf.Sqrt((-b - Mathf.Sqrt(b * b - 4 * a * c)) / (2 * a));
        //end quadratic equation

        //if shooting the projectile is feasible given the parameters,
        //return a non-zero direction vector
        float time;
        if (time0 < 0.0f) {
            if (time1 < 0) {
                return direction;
            }
            time = time1;
        } else {
            if (time1 < 0) {
                time = time0;
            } else {
                if (isWall) {
                    time = Mathf.Max(time0, time1);
                } else {
                    time = Mathf.Min(time0, time1);
                }
            }
        }
        direction = 2 * delta - Physics.gravity * (time * time);
        direction = direction / (2 * speed * time);
        return direction;
    }
}
