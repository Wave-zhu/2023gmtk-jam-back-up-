using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class isRenderUI : MonoBehaviour
{
   
    public GameObject activePanel;

    void Start()
    {
        activePanel.SetActive(true);
    }

    void Update()
    {
        if(GameInputManager.MainInstance.callMenu)
        {
            activePanel.SetActive(!(activePanel.activeSelf));
        }
    }
}
