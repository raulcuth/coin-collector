using UnityEngine;

public class PathFollower : Seek {
    public Path path;
    public float pathOffset = 0.0f;
    float currentParam;

    public override void Awake() {
        base.Awake();
        target = new GameObject();
        currentParam = 0f;
    }

    public override Steering GetSteering() {
        currentParam = path.GetParam(transform.position, currentParam);
        float targetParam = currentParam + pathOffset;
        target.transform.position = path.GetPosition(targetParam);
        return base.GetSteering();
    }
}
