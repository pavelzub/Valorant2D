using System;
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
    public float destroyTime = 5;
    private float destroyTimer = 0;
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
        if (destroyTimer > 0) {
            destroyTimer -= Time.deltaTime;
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        var direction = new Vector2(horizontal, vertical);

        transform.Translate(direction * Time.deltaTime * speed);
        if (transform.position.y > (cameraTop - 1)) {
            var directionY = new Vector2(0, -vertical);
            transform.Translate(directionY * Time.deltaTime * speed);
        }

        if (transform.position.y < cameraBottom) {
            //transform.SetPositionAndRotation(cameraCenter, Quaternion.identity);
            SwitchToScene("LeaderBoard");
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Coin") {
            score++;
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "Spiky") {
            if (destroyTimer > 0) {
                Destroy(other.gameObject);
            }
            else {
                SwitchToScene("LeaderBoard");
            }

        }
        if (other.gameObject.tag == "DestroyBooster") {

            Destroy(other.gameObject);
            destroyTimer = destroyTime;
        }
        if (other.gameObject.tag == "Wall" && destroyTimer > 0) {
            Destroy(other.gameObject);
        }

    }

    public void SwitchToScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }
}
