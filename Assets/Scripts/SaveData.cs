using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    void Awake()
    {
        // A simple singleton

        int saveDataCount = FindObjectsOfType<SaveData>().Length;
        if (saveDataCount > 1)
        {   
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    // Player data
    public float maxHealth = 100f;
    public float health = 100f;
    public int gold = 0;
    public float damage = 10f;
    public ItemDisplay[] items;

    // Game logic data
    public int currentLevel = 0;

    public void SavePlayerData(Player player)
    {
        maxHealth = player.maxHealth;
        health = player.health;
        gold = player.gold;
        damage = player.damage;

        Inventory inventory = GetComponent<Inventory>();

        for(int i = 0; i < inventory.items.Length; i++)
        {
            if(inventory.items[i] != null)
            {   
                inventory.items[i].transform.SetParent(null);
                DontDestroyOnLoad(inventory.items[i]);
            }
        }
    }

    public void SaveGameLogicData(GameLogic gameLogic)
    {
        currentLevel = gameLogic.currentLevel + 1;
    }

    public void LoadData()
    {
        Player player = FindObjectOfType<Player>();
        GameLogic gameLogic = FindObjectOfType<GameLogic>();

        // player data loading
        player.maxHealth = maxHealth;
        player.health = health;
        player.gold = gold;
        player.damage = damage;

        player.ApplyEffects();

        // game logic loading
        gameLogic.currentLevel = currentLevel;
    }
}
