using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenuButton : MonoBehaviour
{
    public void startGame()
    {
        Debug.Log("StartGame.");
        SceneManager.LoadScene("FinalLevel");
        AudioManager.MainInstance.PlayMusic(BGM.FOREST);
    }

    public void exitGame()
    {
        Debug.Log("Exit the games.");
        Application.Quit();
    }
}
