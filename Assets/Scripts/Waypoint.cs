using UnityEngine;

public class Waypoint : MonoBehaviour {
    public static float GetHeightQuality(Vector3 location, Vector3[] surroundings) {
        float maxQuality = 1f;
        float minQuality = -1f;
        float minHeight = Mathf.Infinity;
        float maxHeight = Mathf.NegativeInfinity;
        float height = location.y;

        //traverse surroundings in order to find the maximum and minimum heights
        foreach (Vector3 s in surroundings) {
            if (s.y > maxHeight) {
                maxHeight = s.y;
            }
            if (s.y < minHeight) {
                minHeight = s.y;
            }
        }
        //compute the quality in the given range
        float quality = (height - minHeight) / (maxHeight - minHeight);
        quality *= maxQuality - minQuality;
        quality += minQuality;
        return quality;
    }

    //check if a position is in the same room as the others
    public static bool IsInSameRoom(Vector3 from, Vector3 location, string tagWall = "Wall") {
        RaycastHit[] hits;
        Vector3 direction = location - from;
        float rayLength = direction.magnitude;
        direction.Normalize();
        Ray ray = new Ray(from, direction);
        hits = Physics.RaycastAll(ray, rayLength);
        foreach (RaycastHit h in hits) {
            string tagObj = h.collider.gameObject.tag;
            if (tagObj.Equals(tagWall)) {
                return false;
            }
        }
        return true;
    }

    //compute the quality of the waypoint
    public static float GetCoverQuality(Vector3 location,
                                        int iterations,
                                        Vector3 characterSize,
                                        float radius,
                                        float randomRadius,
                                        float deltaAngle) {
        //degrees of rotation
        float theta = 0f;
        int hits = 0;
        int valid = 0;
        //start the main loop for the iterations to be computed on this waypoint
        for (int i = 0; i < iterations; i++) {
            //create a random position newr the waypoint's origin to see whether
            //the waypoint is easily reachable
            Vector3 from = location;
            float randomBinomial = Random.Range(-1f, 1f);
            from.x += radius * Mathf.Cos(theta) + randomBinomial * randomRadius;
            from.y += Random.value * 2f * randomRadius;
            from.z += radius * Mathf.Sin(theta) + randomBinomial * randomRadius;
            //check whether the random position is in the same room
            if (!IsInSameRoom(from, location)) {
                continue;
            }
            valid++;

            //compute a position, about the size of the template character,
            //around the waypoint
            Vector3 to = location;
            to.x += Random.Range(-1f, 1f) * characterSize.x;
            to.y += Random.value * characterSize.y;
            to.z += Random.Range(-1f, 1f) * characterSize.z;
            //cast a ray to the visibility value to check whether 
            //such a character will be visible
            Vector3 direction = to - location;
            float distance = direction.magnitude;
            direction.Normalize();
            Ray ray = new Ray(location, direction);
            if (Physics.Raycast(ray, distance)) {
                hits++;
            }
            theta = Mathf.Deg2Rad * deltaAngle;
        }
        return (float)(hits / valid);
    }
}
