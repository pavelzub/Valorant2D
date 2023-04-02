using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMover : MonoBehaviour
{
    public float speed = 1f;
    private bool enable = true;
    public PlayerController playerController;
    public int coinsThreshold = 10;
    public float speedIncreece = 0.1f;
    public float maxSpeed = 2.5f;
    private int currentThreshold = 0;

    // Start is called before the first frame update
    void Start()
    {
        currentThreshold = coinsThreshold;
    }

    // Update is called once per frame
    void Update()
    {
        if (enable) {
            transform.Translate(new Vector2(0, 1) * Time.deltaTime * -speed);

            if (playerController.score > currentThreshold) {
                currentThreshold += coinsThreshold;
                speed += speedIncreece;
                speed = Mathf.Min(speed, maxSpeed);
            }
        }
    }

    public void Stop() {
        enable = false;
    }
}
