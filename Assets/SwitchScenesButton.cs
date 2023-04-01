using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScenesButton : MonoBehaviour
{
    public string naxtSceneName;
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Return)) {

        }
        if (Input.GetKeyDown(KeyCode.Escape)) {
            // выполнение действий при нажатии клавиши Escape
            SwitchToScene();
        }

    }
    public void SwitchToScene() {
        if (!string.IsNullOrEmpty(naxtSceneName)) {
            SceneManager.LoadScene(naxtSceneName);
        }

    }
}
