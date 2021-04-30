using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    SaveData saveData;
    Text text;
    int currentLevel;

    void Start()
    {
        saveData = FindObjectOfType<SaveData>();
        text = GetComponent<Text>();
        currentLevel = saveData.currentLevel;
    }

    void Update()
    {
        text.text = string.Format("At level {0}", currentLevel.ToString());
    }

}
