using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class callHome : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject sureToHomePanel;

    void activHomePanel()
    {
        menuPanel.SetActive(false);
        sureToHomePanel.SetActive(true);
    }

    public void openHomePanel()
    {
        Debug.Log("HomeCalled.");
        activHomePanel();
    }

    public void closeSureToHomePanel()
    {
        sureToHomePanel.SetActive(false);
        Debug.Log("Close toHomePanel.");
    }

    public void toHome()
    {
        sureToHomePanel.SetActive(false);
        Debug.Log("To main menu/home.");
        SceneManager.LoadScene("Main-Menu");
        AudioManager.MainInstance.PlayMusic(BGM.DARK_FOREST);
    }

    void Update()
    {
        // give up!!
       /* Vector3 mousePosition = Input.mousePosition;
        Debug.Log(mousePosition);*/
    }

}
