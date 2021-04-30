using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBox : MonoBehaviour
{
    void Awake()
    {
        // A simple singleton

        int musicBoxCount = FindObjectsOfType<MusicBox>().Length;
        if (musicBoxCount > 1)
        {   
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
