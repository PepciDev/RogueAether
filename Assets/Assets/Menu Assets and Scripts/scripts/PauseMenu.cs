using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(AudioSource))]



public class PauseMenu : MonoBehaviour
{
    public static bool paused = false;

    public GameObject EscScreen;
    //Checks if game is paused and switches the bool
    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
            {
                resume();
            }
            else
            {
                pause();
            }
        }
        
    }
    void resume ()
    {
        EscScreen.SetActive(false);
        Time.timeScale = 1f;
        paused = false;
    }
    void pause ()
    {
        EscScreen.SetActive(true);
        Time.timeScale = 0f;
        paused = true;
    }
}

