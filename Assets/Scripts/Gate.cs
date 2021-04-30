using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this script handles behavior of the gates that are used for player transitions between rooms
public class Gate : MonoBehaviour
{
    public Gate partnerGate;
    public Transform triggerPoint;
    public float triggerRadius = 2f;
    public float triggerOffset = 1f;

    private Vector3 cameraSwitch;

    private Player player;
    private AudioSource audioSource;
    public LayerMask playerLayers;

    private Minimap minimap;

    public enum Directions
    {
        UP, DOWN, LEFT, RIGHT
    }

    public Directions gateDirection = Directions.UP;

    private void Start()
    {   
        minimap = FindObjectOfType<Minimap>();
        player = FindObjectOfType<Player>();
        audioSource = gameObject.GetComponent<AudioSource>();
        SwitchDirection();
        SwitchTriggerPosition();
    }

    // moves the gate's trigger point according to its facing direction
    // for example when the gate is placed on the top wall, the trigger must be set below it
    private void SwitchTriggerPosition()
    {
        switch (gateDirection)
        {
            case Directions.UP:
                triggerPoint.position = new Vector2(transform.position.x, transform.position.y - triggerOffset);
                cameraSwitch = new Vector3(0, 10, 0);
                break;
            case Directions.DOWN:
                triggerPoint.position = new Vector2(transform.position.x, transform.position.y + triggerOffset);
                cameraSwitch = new Vector3(0, -10, 0);
                break;
            case Directions.RIGHT:
                triggerPoint.position = new Vector2(transform.position.x - triggerOffset, transform.position.y);
                cameraSwitch = new Vector3(20, 0, 0);
                break;
            case Directions.LEFT:
                triggerPoint.position = new Vector2(transform.position.x + triggerOffset, transform.position.y);
                cameraSwitch = new Vector3(-20, 0, 0);
                break;

        }
    }

    void Update()
    {
        DetectPlayer();
    }

    // displays the correct visuals according to the direction
    void SwitchDirection()
    {
        switch (gateDirection)
        {
            case Directions.UP:
                gameObject.GetComponent<Animator>().SetTrigger("Up");
                gameObject.GetComponent<Animator>().ResetTrigger("Down");
                gameObject.GetComponent<Animator>().ResetTrigger("Right");
                gameObject.GetComponent<Animator>().ResetTrigger("Left");
                break;
            case Directions.DOWN:
                gameObject.GetComponent<Animator>().SetTrigger("Down");
                gameObject.GetComponent<Animator>().ResetTrigger("Up");
                gameObject.GetComponent<Animator>().ResetTrigger("Right");
                gameObject.GetComponent<Animator>().ResetTrigger("Left");
                break;
            case Directions.LEFT:
                gameObject.GetComponent<Animator>().SetTrigger("Left");
                gameObject.GetComponent<Animator>().ResetTrigger("Down");
                gameObject.GetComponent<Animator>().ResetTrigger("Right");
                gameObject.GetComponent<Animator>().ResetTrigger("Up");
                break;
            case Directions.RIGHT:
                gameObject.GetComponent<Animator>().SetTrigger("Right");
                gameObject.GetComponent<Animator>().ResetTrigger("Down");
                gameObject.GetComponent<Animator>().ResetTrigger("Up");
                gameObject.GetComponent<Animator>().ResetTrigger("Left");
                break;
        }
    }

    // handles player interaction with the gate
    private void DetectPlayer()
    {
        Collider2D[] detectedColliders = Physics2D.OverlapCircleAll(triggerPoint.position, triggerRadius, playerLayers);

        foreach (Collider2D collider in detectedColliders)
        {
            if (collider.gameObject.tag == "Player" && GetClosestRoom().GetEnemiesNum() <= 0)
            {
                player.SwitchEHint(true);
                if (Input.GetKeyDown(KeyCode.E))
                {   
                    audioSource.Play();
                    FindObjectOfType<Fader>().Fade();
                    collider.gameObject.transform.position = partnerGate.triggerPoint.position;
                    Camera.main.transform.Translate(cameraSwitch);

                    switch (gateDirection)
                    {
                        case Directions.UP:
                            minimap.Move(0);
                            break;
                        case Directions.DOWN:
                            minimap.Move(1);
                            break;
                        case Directions.LEFT:
                            minimap.Move(2);
                            break;
                        case Directions.RIGHT:
                            minimap.Move(3);
                            break;
                    }

                    float graphCenterX = (AstarPath.active.graphs[0] as Pathfinding.GridGraph).center.x;
                    float graphCenterY = (AstarPath.active.graphs[0] as Pathfinding.GridGraph).center.y;
                    float graphCenterZ = (AstarPath.active.graphs[0] as Pathfinding.GridGraph).center.z;

                    (AstarPath.active.graphs[0] as Pathfinding.GridGraph).center = new Vector3(graphCenterX + cameraSwitch.x, graphCenterY + cameraSwitch.y, graphCenterZ + cameraSwitch.z);
                    AstarPath.active.Scan();
                }
            }
        }

    }

    // help from here: https://forum.unity.com/threads/clean-est-way-to-find-nearest-object-of-many-c.44315/
    Room GetClosestRoom()
    {
        Room[] rooms = FindObjectsOfType<Room>();

        Room rMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;

        foreach (Room r in rooms)
        {
            float dist = Vector3.Distance(r.transform.position, currentPos);
            if (dist < minDist)
            {
                rMin = r;
                minDist = dist;
            }
        }
        return rMin;
    }

    // for debugging only
    private void OnDrawGizmosSelected()
    {

        if (triggerPoint == null)
            return;

        Gizmos.DrawWireSphere(triggerPoint.position, triggerRadius);
    }
}
