using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
//Scene loader & ext

    //Loads the battle stage
    public void GameStart ()
    {
        SceneManager.LoadScene(1);
    }
    //Loads the controlrs screen
    public void GameControls()
    {
        SceneManager.LoadScene(2);
    }
    //Loads the main menu
    public void GameMainMenu()
    {
        SceneManager.LoadScene(0);
        
    }
    //Exits the game
    public void GameExit()
    {
        Application.Quit();
    }

}
