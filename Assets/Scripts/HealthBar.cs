using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{   
    Player player;
    float health = 100;
    private Slider healthSlider;

    void Start()
    {
        player = FindObjectOfType<Player>();
        healthSlider = gameObject.GetComponent<Slider>();
    }

    void Update()
    {   if(player != null){
            health = player.GetHealth();
            if(health >= 0){
                healthSlider.value = health / player.maxHealth;
            }
        }
    }
}