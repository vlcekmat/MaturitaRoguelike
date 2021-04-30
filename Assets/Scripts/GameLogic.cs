using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public int currentLevel = 0;

    void Start()
    {
        SaveData saveData = FindObjectOfType<SaveData>();
        saveData.LoadData();
        saveData.GetComponent<Inventory>().MakeAllVisible();
    }
}
