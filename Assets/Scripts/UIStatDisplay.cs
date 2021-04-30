using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStatDisplay : MonoBehaviour
{
    public enum StatType 
    {MAXHEALTH, DAMAGE, SPEED, GOLD, SHOP_MONEY};

    public StatType statType = StatType.MAXHEALTH;

    private float myStat = 0;
    Player player;

    Text text;
    SaveData saveData;

    void Start()
    {   
        saveData = FindObjectOfType<SaveData>();
        if(FindObjectOfType<Player>() != null)
            {
                player = FindObjectOfType<Player>();
            }
        text = GetComponent<Text>();
    }

    void Update()
    {
        if(player != null)
        {

            if(statType == StatType.MAXHEALTH)
            {
                myStat = player.maxHealth;
            }
            else if(statType == StatType.DAMAGE)
            {   
                myStat = player.damage + 90;
            }
            else if(statType == StatType.SPEED)
            {   
                myStat = player.movSpeed;
            }
            else if(statType == StatType.GOLD)
            {   
                myStat = (float)player.gold;
            }
        }
        
        else if(statType == StatType.SHOP_MONEY)
        {
            myStat = (float)saveData.gold;
        }

        text.text = myStat.ToString();
    }
}
