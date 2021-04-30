using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Vector2 target;
    Rigidbody2D rb;
    Vector2 direction;
    public float damage = 10;
    public float arrowReach = 1;
    public LayerMask playerLayers;
    void Start()
    {   
        rb = GetComponent<Rigidbody2D>();

        damage = 5 + FindObjectOfType<GameLogic>().currentLevel * 5;

        if(FindObjectOfType<Player>() != null)
        {
            target = FindObjectOfType<Player>().transform.position;
            direction = new Vector2(target.x - rb.position.x, target.y - rb.position.y);
        }

        rb.AddForce(direction.normalized);
        Destroy(gameObject, 3);
    }

    void Update()
    {
        Collider2D player = DetectEnemy();
        if(player != null)
        {
            player.GetComponent<Player>().Hurt(damage);
            Destroy(gameObject);
        }
    }

    private Collider2D DetectEnemy()
    {
        Collider2D[] detectedColliders = Physics2D.OverlapCircleAll(rb.position, arrowReach, playerLayers);
        foreach (Collider2D collider in detectedColliders)
        {
            return collider;
        }
        return null;
    }

}
