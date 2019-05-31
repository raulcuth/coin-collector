using UnityEngine;

public class AvoidAgent : AgentBehaviour {
    public float collisionRadius = 0.4f;
    GameObject[] targets;

    private void Start() {
        targets = GameObject.FindGameObjectsWithTag("Agent");
    }

    public override Steering GetSteering() {
        //compute the distances and velocity from agents that are nearby
        Steering steering = new Steering();
        float shortestTime = Mathf.Infinity;
        GameObject firstTarget = null;
        float firstMinSeparation = 0.0f;
        float firstDistance = 0.0f;
        Vector3 firstRelativePos = Vector3.zero;
        Vector3 firstRelativeVel = Vector3.zero;

        //find the closest agent that is prone to collision with the current one
        FindClosestAgentProneToCollision(ref shortestTime, ref firstTarget,
                                         ref firstMinSeparation, ref firstRelativePos, ref firstRelativeVel);

        //if there is one that is prone to collision, then move away
        if (firstTarget == null) {
            return steering;
        }
        if (firstMinSeparation <= 0.0f || firstDistance < 2 * collisionRadius) {
            firstRelativePos = firstTarget.transform.position;
        } else {
            firstRelativePos += firstRelativeVel * shortestTime;
        }
        firstRelativePos.Normalize();
        steering.linear = -firstRelativePos * agent.maxAccel;
        return steering;
    }

    private void FindClosestAgentProneToCollision(ref float shortestTime,
                                                  ref GameObject firstTarget,
                                                  ref float firstMinSeparation,
                                                  ref Vector3 firstRelativePos,
                                                  ref Vector3 firstRelativeVel) {
        foreach (GameObject t in targets) {
            Vector3 relativePos;
            Agent targetAgent = t.GetComponent<Agent>();
            relativePos = t.transform.position - transform.position;
            Vector3 relativeVel = targetAgent.velocity - agent.velocity;
            float relativeSpeed = relativeVel.magnitude;

            float timeToCollision = Vector3.Dot(relativePos, relativeVel);
            timeToCollision /= relativeSpeed * relativeSpeed * -1;
            float distance = relativePos.magnitude;
            float minSeparation = distance - relativeSpeed * timeToCollision;

            if (minSeparation > 2 * collisionRadius) {
                continue;
            }
            if (timeToCollision > 0.0f && timeToCollision < shortestTime) {
                shortestTime = timeToCollision;
                firstTarget = t;
                firstMinSeparation = minSeparation;
                firstRelativePos = relativePos;
                firstRelativeVel = relativeVel;
            }
        }
    }
}
