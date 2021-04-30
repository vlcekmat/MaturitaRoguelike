using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this script keeps track of what is happening in the given room and sets rules according to that
public class Room : MonoBehaviour
{
    public LayerMask entitiesLayers;

    // state
    public bool playerPresent = false;
    public int numOfEnemies = 0;

    void Start()
    {
        playerPresent = false;
        numOfEnemies = 0;
    }

    void Update()
    {
        Collider2D[] detectedEntities = Physics2D.OverlapBoxAll(transform.position, new Vector3(15f, 10f, 10f), 0f, entitiesLayers);
        List<GameObject> enemies = new List<GameObject>{};
        foreach (Collider2D collider in detectedEntities)
            {
                if(collider.tag == "Player")
                {
                    playerPresent = true;
                }
                else
                {
                    playerPresent = false;
                }

                if(collider.tag == "Enemy")
                {
                    enemies.Add(collider.gameObject);
                }
            }
        numOfEnemies = enemies.Count;
    }

    // shows debug gizmos in the editor
    private void OnDrawGizmosSelected()
    { 
        Gizmos.DrawWireCube(transform.position, new Vector3(15f, 10f, 10f));
    }

    public bool PlayerPresent()
    {
        return playerPresent;
    }

    public int GetEnemiesNum()
    {
        return numOfEnemies;
    }
}
