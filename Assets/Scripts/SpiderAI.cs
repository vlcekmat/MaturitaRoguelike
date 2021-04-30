using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

// this script could be arguably called "EnemyPathfinding", it handles all the pathfinding for hostile entities
// this script has been written with the help of this video: https://www.youtube.com/watch?v=jvtFUfJ6CP8&t=1069s
public class SpiderAI : MonoBehaviour
{
    public Transform target;

    public float speed = 200f;
    public float nextWaypointDistance = 3f;

    Path path;
    int currentWaypoint = 0;

    Seeker seeker;
    Rigidbody2D rb;
    Enemy enemy;
    public GameObject projectilePrefab;

    // state
    bool stopping = false;

    // walk animator
    Animator animator;

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

    void FixedUpdate()
    {
        if(!stopping)
        {
            MoveOnPath();
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
