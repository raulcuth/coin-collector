using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public float maxTime = 120f;
    [SerializeField]
    private float countDown = 0;
    // Start is called before the first frame update
    void Start()
    {
        countDown = maxTime;
    }

    // Update is called once per frame
    void Update()
    {
        countDown -= Time.deltaTime;
        if (countDown <= 0) {
            //reset the coin count
            Coin.coinCount = 0;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
