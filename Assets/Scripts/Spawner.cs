using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public List<Transform> possibleSpawnPoints;

    // entity bank
    public List<GameObject> monsterBank;

    // cached reference
    private Room roomScript;

    // state variables
    public int currentLevel = 0;
    public bool enmAlrdSpawned = false;

    GameLogic gameLogic;

    void Start()
    {
        gameLogic = FindObjectOfType<GameLogic>();
        roomScript = gameObject.GetComponent<Room>();
    }

    private void SpawnEnemies()
    {
        if(!enmAlrdSpawned)
        {
            enmAlrdSpawned = true;

            int enemiesToSpawnNum = (int)Random.Range(0, currentLevel);

            for(int i = 0; i <= enemiesToSpawnNum; i++)
            {   

                Transform chosenPosition = possibleSpawnPoints[Random.Range(0, possibleSpawnPoints.Count)];
                possibleSpawnPoints.Remove(chosenPosition);
                Instantiate(monsterBank[Random.Range(0, monsterBank.Count)], chosenPosition.position, Quaternion.identity);
            }
        }
    }

    void Update()
    {   
        currentLevel = gameLogic.currentLevel;
        if(roomScript.PlayerPresent())
        {
            SpawnEnemies();
        }
    }
}
