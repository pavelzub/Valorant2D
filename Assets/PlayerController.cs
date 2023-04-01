﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public GameObject camera;
    public float speed = 5f; // скорость движения точки
    private float cameraHeight;
    private float cameraWidth;
    private float cameraLeft;
    private float cameraRight;
    private float cameraBottom;
    private float cameraTop;
    public int score = 0;
    private Vector3 cameraCenter;

    // вызывается при запуске игры
    private void Start() {
        var cameraComp = camera.GetComponent<Camera>();
        cameraHeight = 2f * cameraComp.orthographicSize;
        cameraWidth = cameraHeight * cameraComp.aspect;
        cameraLeft = cameraComp.transform.position.x - cameraWidth / 2f;
        cameraRight = cameraComp.transform.position.x + cameraWidth / 2f;
        cameraBottom = cameraComp.transform.position.y - cameraHeight / 2f;
        cameraTop = cameraComp.transform.position.y + cameraHeight / 2f;
        var center = camera.GetComponent<Transform>().position;
        center.z = 0;
        cameraCenter = center;
        transform.Translate(cameraCenter);
    }

    private void Update() {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        var direction = new Vector2(horizontal, vertical);

        transform.Translate(direction * Time.deltaTime * speed);
        if (transform.position.y > cameraTop) {
            //transform.SetPositionAndRotation(cameraCenter, Quaternion.identity);
            SwitchToScene("LeaderBoard");
        }

        if (transform.position.y < cameraBottom) {
            var directionY = new Vector2(0, -vertical);
            transform.Translate(directionY * Time.deltaTime * speed);
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Coin") {
            score++;
            Destroy(other.gameObject);
        }
    }

    public void SwitchToScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }
}
