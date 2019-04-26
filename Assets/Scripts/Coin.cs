using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {
    public static int coinCount = 0;
    // Start is called before the first frame update
    void Start() {
        ++Coin.coinCount;
    }

    private void OnTriggerEnter(Collider collider) {
        if ( collider.CompareTag("Player")) {
            Destroy(gameObject);
        }
    }

    private void OnDestroy() {
        --Coin.coinCount;
        if (Coin.coinCount <= 0) {

        }
    }

}
