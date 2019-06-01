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
}
