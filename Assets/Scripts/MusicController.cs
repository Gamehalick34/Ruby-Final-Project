using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicController : MonoBehaviour
{
    public GameObject Main;
    public GameObject Vicotry;
    public GameObject Lose;

    void Start()
    {
        Main.SetActive(true);
        Vicotry.SetActive(false);
        Lose.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        if (RubyController.RFixed >= 5 && SceneManager.GetActiveScene().name == "Level2")
        {
            Main.SetActive(false);
            Vicotry.SetActive(true);
        }

        if (RubyController.currentHealth <= 0)
        {
            Main.SetActive(false);
            Lose.SetActive(true);
        }

    }
}