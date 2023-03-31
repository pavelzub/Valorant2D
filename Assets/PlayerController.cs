using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f; // скорость движения точки

    // вызывается при запуске игры
    private void Start()
    {
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        var direction = new Vector2(horizontal, vertical);

        transform.Translate(direction * Time.deltaTime * speed);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
      
    }
}
