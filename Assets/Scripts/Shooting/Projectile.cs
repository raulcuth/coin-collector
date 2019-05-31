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
}
