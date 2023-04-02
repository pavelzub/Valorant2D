using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public GameObject camera;
    public AudioSource cointouch;
    public AudioSource wings;
    public AudioSource wallDestroy;
    public AudioSource lose;
    public AudioSource bonustouch;
    public AudioSource bonusOff;
    bool isBonusActive = false;
    public WorldMover mover;
    public GameObject nameWindow;
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
    private bool dead = false;
    private Animation animation;
    private Animator animator;
    private AudioSource audioMainLoop;

    // вызывается при запуске игры
    private void Start() {
        animation = GetComponent<Animation>();
        animator = GetComponent<Animator>();
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

        var audio = GameObject.Find("AudioMainLoop");
        if (audio) {
            audioMainLoop = audio.GetComponent<AudioSource>();
        }

    }

    private void Update() {
        if (dead) {
            return;
        }

        if (destroyTimer > 0) {
            wings.volume = 1;
            destroyTimer -= Time.deltaTime;
        }
        else {
            wings.volume = 0.6f;
            animator.SetBool("Bonus", false);
        }
        wings.Play();
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        var direction = new Vector2(horizontal, vertical);

        transform.Translate(direction * Time.deltaTime * speed);
        if (transform.position.y > (cameraTop - 1)) {
            var directionY = new Vector2(0, -vertical);
            transform.Translate(directionY * Time.deltaTime * speed);
        }

        if (transform.position.y < cameraBottom) {
            Lose();
            //SwitchToScene("LeaderBoard");
        }
    }

    //public void SwitchToScene(string name) {

    //}
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Coin") {
            score++;
            Destroy(other.gameObject);
            cointouch.Play();
        }

        if (other.gameObject.tag == "Spiky") {
            if (destroyTimer > 0) {
                Destroy(other.gameObject);
                wallDestroy.Play();
            }
            else {
                Lose();
            }

        }
        if (other.gameObject.tag == "DestroyBooster") {
            bonustouch.Play();
            isBonusActive = true;
            if (audioMainLoop && audioMainLoop.pitch < 1.2f) {
                audioMainLoop.pitch += 0.025f;
            }
            Destroy(other.gameObject);
            if (destroyTimer <= 0) {
                animator.SetBool("Bonus", true);
            }
            destroyTimer = destroyTime;

        }
        if (other.gameObject.tag == "Wall" && destroyTimer > 0) {
            Destroy(other.gameObject);
            wallDestroy.Play();
        }

        if (destroyTimer <= 0f) {
            if (audioMainLoop && audioMainLoop.pitch > 1f) {
                audioMainLoop.pitch += 0.05f;
                audioMainLoop.pitch = 1f;
            }
            if (isBonusActive) {
                bonusOff.Play();
                isBonusActive = false;
            }

        }

        if (destroyTimer <= 0.5f && isBonusActive) {
            isBonusActive = false;
            bonusOff.Play();
        }

    }

    public void Lose() {
        if (!lose.isPlaying) {
            lose.Play();
        }

        dead = true;
        mover.Stop();
        if (audioMainLoop) {
            audioMainLoop.pitch = 1f;
        }
        animation.Play("Death", PlayMode.StopAll);
    }

    public void DeathAnimEnd() {
        nameWindow.SetActive(true);
    }

    public void SwitchToScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }
}
