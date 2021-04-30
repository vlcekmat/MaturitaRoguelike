using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this script handles player controls and player stats
public class Player : MonoBehaviour
{

    // movement
    public float movSpeed = 5f;
    float vertical;
    float horizontal;
    Vector2 movement;
    Animator animator;

    Rigidbody2D rb;

    // stats
    public float maxHealth = 100f;
    public float health = 100f;

    public int gold = 0;
    public float damage = 10f;
    public float reach = 2f;

    public float pForce = 5f;

    public float attackCooldown = 0.5f;

    public float hurtDuration = 1f;

    // state
    public bool stunned = false;

    public bool attacking = false;

    public bool beingHurt = false;


    // combat
    public Transform attackPoint;
    private Quaternion attackRotation;
    public LayerMask enemyLayers;

    // misc rotation stuff
    public Transform weaponAnchor;


    // weapons
    Weapons primary = Weapons.SWORD;

    // other cached reference
    private SpriteRenderer sr;
    private SaveData saveData;
    private Inventory inventory;

    private AudioSource audioSource;

    // other
    public GameObject eHintButton;
    public LayerMask gateLayers;

    enum Weapons 
    {
        SWORD,
        SPEAR,
        BOW
    }

    private void Start()
    {   
        SwitchEHint(false);
        sr = gameObject.GetComponent<SpriteRenderer>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        audioSource = gameObject.GetComponent<AudioSource>();
        animator = gameObject.GetComponent<Animator>();
        saveData = FindObjectOfType<SaveData>();
        inventory = saveData.gameObject.GetComponent<Inventory>();
    }

    void Update()
    {
        HandleControls();

        if(Input.GetKeyDown("escape"))
        {
            Destroy(FindObjectOfType<SaveData>());
            FindObjectOfType<MySceneManager>().LoadMainMenu();
        }
    }

    void HandleControls()
    {
        vertical = Input.GetAxisRaw("Vertical");
        horizontal = Input.GetAxisRaw("Horizontal");
        movement = new Vector2(horizontal, vertical);

        if (Input.GetButtonDown("Fire1"))
        {
           Attack();
        }

        Rotate();
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.magnitude);

        Vector2 dirToAttackPoint = (attackPoint.position - transform.position).normalized;
        animator.SetFloat("AttackHorizontal", dirToAttackPoint.x); 
        animator.SetFloat("AttackVertical", dirToAttackPoint.y); 

        Collider2D[] detectedColliders = Physics2D.OverlapCircleAll(transform.position, 1.3f, gateLayers);
        if (detectedColliders.Length <= 0)
        {
            SwitchEHint(false);
        }
    }

    // updates player's position according to the controls
    void FixedUpdate()
    {
        if (!stunned && !attacking)
        {
            rb.velocity = Vector3.zero;
            rb.MovePosition(rb.position + movement * movSpeed * Time.fixedDeltaTime);
        }
    }

    // rotates the attack trigger so it faces the cursor
    private void Rotate()
    {
        Vector2 direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - weaponAnchor.position);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float snappedAngle = SnapAngle(angle);
        Quaternion rotation = Quaternion.AngleAxis(snappedAngle, Vector3.forward);
        weaponAnchor.rotation = Quaternion.Slerp(weaponAnchor.rotation, rotation, 10f).normalized;
    }

    // snaps the angle to the nearest right angle
    private float SnapAngle(float rawAngle)
    {   
        float newAngle = rawAngle;
        if (135 > rawAngle && rawAngle >= 45)
        {
            newAngle = 90;
        }
        else if(45 > rawAngle && rawAngle >= -45)
        {
            newAngle = 0;
        }
        else if(-45 > rawAngle && rawAngle > -135)
        {
            newAngle = -90;
        }
        else if(Mathf.Abs(rawAngle) >= 135)
        {
            newAngle = 180;
        }

        return newAngle;
    }

    // creates a circle at position of the attackPoint, which checks all colliders in the given area
    // if an enemy is detected, its collider is returned
    // attacked enemy is also pushed in direction of the attack
    // this function is called only when the cooldown has ran out
    private void Attack()
    {
        if (primary == Weapons.SWORD && !attacking)
        {   
            attacking = true;
            animator.SetBool("Attacking", true);
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, reach, enemyLayers);
            foreach (Collider2D enemyCollider in hitEnemies)
            {
                enemyCollider.gameObject.GetComponent<Enemy>().Hurt(damage);
                Push(enemyCollider.gameObject.GetComponent<Rigidbody2D>(), pForce);
            }
            StartCoroutine(SetAttackingFalse(attackCooldown));
        }
    }

    private IEnumerator SetAttackingFalse(float duration)
    {
        yield return new WaitForSeconds(duration);
        attacking = false;
        animator.SetBool("Attacking", false);
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

    // destroys the player object and initiates Game Over
    private void Die()
    {
        FindObjectOfType<MySceneManager>().LoadDeathScreen();
        //Destroy(gameObject, 0.3f);
    }

    // shows the "E button" tooltip, when the control is available
    public void SwitchEHint(bool val)
    {
        eHintButton.SetActive(val);
    }

    // stuns the player, so controls can't interupt push
    public void Stun(float stunDuration)
    {
        if(!stunned)
        {
            stunned = true;
            StartCoroutine(UncheckStun(stunDuration));
        }
    }

    private IEnumerator UncheckStun(float duration)
    {
        yield return new WaitForSeconds(duration);
        stunned = false;
    }

    // pushes enemy in direction of the attack
    public void Push(Rigidbody2D enemyRb, float pushForce)
    {   
        Enemy enemy = enemyRb.gameObject.GetComponent<Enemy>();
        Vector2 direction = (enemyRb.gameObject.transform.position - rb.gameObject.transform.position).normalized;
        StartCoroutine(enemy.Stop(enemy.stunDuration));
        enemyRb.velocity = Vector2.zero;
        Vector2 forceToAdd = new Vector2(direction.x * pushForce, direction.y * pushForce);
        enemyRb.AddForce(forceToAdd);
    }

    // applies item effects to the real player stats
    public void ApplyEffects()
    {
        for(int i = 0; i < inventory.items.Length; i++)
        {
            if(inventory.items[i] != null)
            {   
                ItemDisplay itemDisplay = inventory.items[i].GetComponent<ItemDisplay>();
                maxHealth += itemDisplay.onMaxHealth;
                damage += itemDisplay.onDamage;
                movSpeed += itemDisplay.onSpeed;
                health = maxHealth;
            }
        }
    }


    // shows debug gizmos in the editor
    private void OnDrawGizmosSelected()
    { 

        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, reach);
    }

    public float GetHealth()
    {
        return health;
    }
}
