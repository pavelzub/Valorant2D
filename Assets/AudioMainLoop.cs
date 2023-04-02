using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMainLoop : MonoBehaviour
{
    public static AudioMainLoop instance;

    private AudioSource audioSource;
    public AudioClip clip;

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
    }

    private void Start() {
        audioSource.loop = true;
        audioSource.Play();
    }
}
