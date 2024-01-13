using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class callMenu : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject sureToHomePanel;
    public GameObject helpPanel;

    void activMenuPanel()
    {
        sureToHomePanel.SetActive(false);
        menuPanel.SetActive(true);
    }

    public void openMenu()
    {
        Debug.Log("menuCalled.");
        activMenuPanel();
    }

    public void closeMenu()
    {
        Debug.Log("Close toHomePanel / continue the game.");
        menuPanel.SetActive(false);
    }

    public void callHelp()
    {
        // menuPanel.SetActive(false);
        Debug.Log("Call helps.");
        helpPanel.SetActive(true);
    }

    public void retryGame()
    {
        menuPanel.SetActive(false);
        SceneManager.LoadScene("FinalLevel");
    }

    public void exitGame()
    {
        menuPanel.SetActive(false);
        helpPanel.SetActive(false);
        Debug.Log("Exit the games.");
        Application.Quit();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(helpPanel.activeSelf) 
            { 
                helpPanel.SetActive(false);
            }
        }
    }

}
