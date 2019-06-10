using System.Collections;
using UnityEngine;

//template for custom attacks
public class Attack : MonoBehaviour {
    public int weight;

    public virtual IEnumerator Execute() {
        //attack behaviour
        yield break;
    }
}
