using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

// this script handles all the pathfinding for hostile entities
// this script has been written with the help of this video: https://www.youtube.com/watch?v=jvtFUfJ6CP8&t=1069s
public class SkeletonAI : MonoBehaviour
{
    public Transform target;

    public float speed = 200f;
    public float shootCooldown = 1f;
    public float aimingTime = 1f;
    public float nextWaypointDistance = 3f;

    Path path;
    int currentWaypoint = 0;

    Seeker seeker;
    Rigidbody2D rb;
    Enemy enemy;

    public GameObject projectilePrefab;
    public GameObject projectileSpawnPoint;
    public AudioClip shootSound;

    // state
    bool stopping = false;
    public bool idle = false;
    bool shooting = false;

    // walk animator
    Animator animator;

    // layer mask for colliders that would block arrows
    public LayerMask layerMask;

    public float distanceFromTarget;

    void Start()
    {   
        enemy = GetComponent<Enemy>();
        animator = GetComponent<Animator>();
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        target = FindObjectOfType<Player>().gameObject.transform;

        InvokeRepeating("UpdatePath", 0f, 0.04f);
    }

    void Update()
    {
        stopping = enemy.stopping;
        // a little bit of vector math
        distanceFromTarget = Mathf.Sqrt(Mathf.Pow(target.position.x - rb.position.x, 2) + Mathf.Pow(target.position.y - rb.position.y, 2));
    }

    // creates a new path if the previous one has been completed or interupted
    void UpdatePath()
    {
        if (seeker.IsDone() && !stopping && target != null)
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    IEnumerator Shoot()
    {   
        shooting = true;
        yield return new WaitForSeconds(aimingTime);
        gameObject.GetComponent<AudioSource>().PlayOneShot(shootSound);
        animator.SetTrigger("SHOTS FIRED");

        Quaternion arrowDirection = Quaternion.identity;

        if(target.position.x - rb.position.x < 0)
        {
            arrowDirection = new Quaternion(0,0,180,0);
        }

        Instantiate(projectilePrefab, projectileSpawnPoint.transform.position, arrowDirection);
        yield return new WaitForSeconds(shootCooldown);
        animator.SetBool("Aiming", false);
        shooting = false;
    }

    void FixedUpdate()
    {   
        Vector2 animDirection = ((Vector2)target.transform.position - rb.position);
        animator.SetFloat("Horizontal", animDirection.x);
        animator.SetFloat("Vertical", animDirection.y);

        if(!stopping)
        {   if(distanceFromTarget > 4 && !animator.GetBool("Aiming"))
            {   
                idle = false;
                MoveOnPath();
            }
            else
            {   
                idle = true;
                animator.SetBool("Aiming", true);
                if(!shooting)
                {   
                    StartCoroutine(Shoot());
                }
                
            }
            
        }
    }

    // makes the entity move towards the next waypoint
    private void MoveOnPath()
    {
        if(target != null){
         if (path == null)
                return;

            if (currentWaypoint >= path.vectorPath.Count)
            {
                // reached end of path
             return;
            }


            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
            Vector2 animDirection = ((Vector2)target.transform.position - rb.position);

         animator.SetFloat("Horizontal", animDirection.x);
         animator.SetFloat("Vertical", animDirection.y);

         rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);


         float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

         if (distance < nextWaypointDistance)
         {
                currentWaypoint++;
            }
        }
    }
}
