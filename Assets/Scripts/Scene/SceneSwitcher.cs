using Game.Tool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    [SerializeField]
    private string targetSceneName;

    private void OnTriggerEnter2D(Collider2D other)
    {
        DevelopmentToos.WTF(other.tag);
        if (other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene(targetSceneName);
        }
    }
}
