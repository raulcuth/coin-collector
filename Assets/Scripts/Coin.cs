using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {
    public static int coinCount = 0;
    // Start is called before the first frame update
    void Start() {
        ++Coin.coinCount;
    }

    private void OnTriggerEnter(Collider col) {
        if (col.CompareTag("Player")) {
            Destroy(gameObject);
        }
    }

    private void OnDestroy() {
        --Coin.coinCount;
        if (Coin.coinCount <= 0) {
            //game won destroy timer and launch fireworks
            GameObject timer = GameObject.Find("LevelTimer");
            Destroy(timer);
            GameObject[] fireworkSystems = GameObject.FindGameObjectsWithTag("Fireworks");
            foreach (GameObject go in fireworkSystems) {
                go.GetComponent<ParticleSystem>().Play();
            }
        }
    }

}
