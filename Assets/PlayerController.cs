using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject camera;
    public float speed = 5f; // скорость движения точки

    // вызывается при запуске игры
    private void Start() {
        var cameraComp = camera.GetComponent<Camera>();
        float cameraHeight = 2f * cameraComp.orthographicSize;
        float cameraWidth = cameraHeight * cameraComp.aspect;
        float cameraLeft = cameraComp.transform.position.x - cameraWidth / 2f;
        float cameraRight = cameraComp.transform.position.x + cameraWidth / 2f;
        float cameraBottom = cameraComp.transform.position.y - cameraHeight / 2f;
        float cameraTop = cameraComp.transform.position.y + cameraHeight / 2f;
        var cameraCenter = camera.GetComponent<Transform>().position;
        cameraCenter.z = 0;
        transform.Translate(cameraCenter);
    }

    private void Update() {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        var direction = new Vector2(horizontal, vertical);

        transform.Translate(direction * Time.deltaTime * speed);
    }
    private void OnTriggerEnter2D(Collider2D other) {

    }
}
