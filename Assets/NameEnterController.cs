using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Windows;

public class NameEnterController : MonoBehaviour
{
    public GameObject inputField;
    public DBConnector connector;
    public PlayerController playerController;
    TMP_InputField inp;
    // Start is called before the first frame update
    void Start()
    {
        inp = inputField.GetComponent<TMP_InputField>();
        inp.ActivateInputField();
    }

    public void OnEnter() {
        if (inp.text.Length <= 0) {
            return;
        }

        connector.SendScore(inp.text, playerController.score);
        SceneManager.LoadScene("LeaderBoard");
    }

    public void OnLoseSelection() {
        //inp.ActivateInputField();
    }


    private void Update() {
        if (UnityEngine.Input.GetKeyDown(KeyCode.Return)) {
            OnEnter();
        }
    }
}
