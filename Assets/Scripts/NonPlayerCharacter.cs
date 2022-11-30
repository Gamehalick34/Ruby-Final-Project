using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NonPlayerCharacter : MonoBehaviour
{
    public float displayTime = 4.0f;
    public GameObject dialogBox;
    public GameObject NextLevel;
    float timerDisplay;

    void Start()
    {
        dialogBox.SetActive(false);
        NextLevel.SetActive(false);
        timerDisplay = -1.0f;
    }

    void Update()
    {
        if (timerDisplay >= 0)
        {
            timerDisplay -= Time.deltaTime;
            if (timerDisplay < 0)
            {
                dialogBox.SetActive(false);
            }
        }

        if (RubyController.RFixed >= 5 && SceneManager.GetActiveScene().name == "Main")
        {
            NextLevel.SetActive(true);
        }

    }

    public void DisplayDialog()
    {
        timerDisplay = displayTime;
        dialogBox.SetActive(true);
    }
}