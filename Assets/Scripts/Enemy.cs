using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this script handles enemy stats
public class Enemy : MonoBehaviour
{
    // cached reference
    public Rigidbody2D rb;
    private SpriteRenderer sr;
    private AudioSource audioSource;

    // reference
    public Transform attackPoint;
    public LayerMask playerLayers;

    // stats
    public float health = 100f;
    public float reach = 1f;
    public float attackCooldown = 0.5f;
    public float damage = 10f;
    public bool hostile = true;
    public float stunDuration = 0.5f;

    public float pushForce = 5f;
    public float hurtDuration = 1f;

    public int reward = 40;

    // state
    public bool attacking = false;

    public bool stopping = false;

    public bool beingHurt = false;

    private void Start()
    {   
        damage = 10 + FindObjectOfType<GameLogic>().currentLevel * 5;
        health = 80 + FindObjectOfType<GameLogic>().currentLevel * 20;
        reward = 0 + FindObjectOfType<GameLogic>().currentLevel * 50;

        rb = gameObject.GetComponent<Rigidbody2D>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    private void Update()
    {   
        if (!attacking)
        {
            StartCoroutine(Attack());
        }
    }

    // when called, the passed damage is subtracted from player's health
    // it also calls the function Die(), when the health is too low
    public void Hurt(float damage)
    {   
        StartCoroutine(PlayHurtAnimation());
        audioSource.Play();

        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    // destroys the object
    private void Die()
    {
        Destroy(gameObject, 0.25f);
        FindObjectOfType<Player>().gold += reward;
        // Die animation + destroy objects + leave rewards
    }

    // creates a circle at position of the attackPoint, which checks all colliders in the given area
    // if an enemy (the player in this case) is detected, its collider is returned
    private Collider2D DetectEnemy()
    {
        Collider2D[] detectedColliders = Physics2D.OverlapCircleAll(attackPoint.position, reach, playerLayers);
        foreach (Collider2D collider in detectedColliders)
        {
            return collider;
        }
        return null;
    }

    // calls DetectEnemy(), if an enemy is found, damage is applied to it
    // attacked enemy is also pushed in direction of the attack
    // this function is called only when the cooldown has ran out
    public IEnumerator Attack()
    {
        attacking = true;
        var enemy = DetectEnemy();
        if (enemy != null)
        {
            enemy.gameObject.GetComponent<Player>().Hurt(damage);
            Push(enemy.GetComponent<Rigidbody2D>());
            yield return new WaitForSeconds(attackCooldown);
        }
        // if enemies, Attack and wait for cooldown
        attacking = false;
    }

    // pushes enemy in direction of the attack
    public void Push(Rigidbody2D enemyRb)
    {
        Vector2 direction = (enemyRb.gameObject.transform.position - rb.gameObject.transform.position).normalized;
        StartCoroutine(Stop(stunDuration));
        enemyRb.gameObject.GetComponent<Player>().Stun(stunDuration);
        enemyRb.velocity = Vector2.zero;
        Vector2 forceToAdd = new Vector2(direction.x * pushForce, direction.y * pushForce);
        enemyRb.AddForce(forceToAdd);
    }

    private IEnumerator PlayHurtAnimation()
    {   
        for (int i = 0; i < 3; i++)
        {
            sr.color = new Color(1f, 0f, 0f, 1f);
            yield return new WaitForSeconds(hurtDuration / 6);
            sr.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(hurtDuration / 6);
        }
    }

    // cancels movement of this entity
    public IEnumerator Stop(float duration)
    {
        stopping = true;
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(duration);
        stopping = false;
        rb.velocity = Vector2.zero;
    }

    // only for debugging purposes
    private void OnDrawGizmos()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, reach);
    }
}