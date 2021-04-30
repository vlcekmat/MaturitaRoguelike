using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour
{
    void Awake()
    {   
        // A simple singleton
        DontDestroyOnLoad(gameObject);
        GameObject duplicateSceneManager = GameObject.Find(gameObject.name);
        if (duplicateSceneManager != null && duplicateSceneManager != this.gameObject)
        {
            Destroy(duplicateSceneManager);
        }
    }

    public void LoadMainMenu()
    {   
        if(FindObjectOfType<SaveData>() != null)
        {
            Destroy(FindObjectOfType<SaveData>());
        }
        SceneManager.LoadScene(0);
    }
    public void LoadCoreGame()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadStore()
    {
        SceneManager.LoadScene(2);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        if(FindObjectOfType<SaveData>() != null)
        {
            Destroy(FindObjectOfType<SaveData>());
        }
        SceneManager.LoadScene(1);
    }

    public void LoadDeathScreen()
    {
        SceneManager.LoadScene(3);
    }
}
