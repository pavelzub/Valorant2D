using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMover : MonoBehaviour
{
    public float speed = 1f;
    private bool enable = true;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (enable) {
            transform.Translate(new Vector2(0, 1) * Time.deltaTime * speed);
        }
    }

    public void Stop() {
        enable = false;
    }
}
