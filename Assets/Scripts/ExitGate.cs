using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGate : MonoBehaviour
{
    public Sprite[] animationSprites;
    public float animDelay = 0.5f;
    private new SpriteRenderer renderer;
    public Transform triggerPoint;
    public float triggerRadius = 2f;
    public LayerMask playerLayers;
    
    private Player player;

    private AudioSource audioSource;

    private void Start()
    {   
        player = FindObjectOfType<Player>();
        audioSource = GetComponent<AudioSource>();
        renderer = GetComponent<SpriteRenderer>();
        StartCoroutine(AnimateObject());
    }

    private void Update()
    {
        DetectPlayer();
    }

    // creates the portal animation
    private IEnumerator AnimateObject()
    {
        for (int i = 0; i < animationSprites.Length; i++)
        {
            renderer.sprite = animationSprites[i];
            yield return new WaitForSeconds(animDelay);
        }
        StartCoroutine(AnimateObject());
    }

    // checks if the player is nearby the gate, if so, the "E" button hint appears and player can now leave the level by pressing the button
    private void DetectPlayer()
    {
        Collider2D[] detectedColliders = Physics2D.OverlapCircleAll(triggerPoint.position, triggerRadius, playerLayers);

        foreach (Collider2D collider in detectedColliders)
        {
            if (collider.gameObject.tag == "Player")
            {
                player.SwitchEHint(true);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    audioSource.Play();
                    SaveData saveData = FindObjectOfType<SaveData>();
                    saveData.SavePlayerData(FindObjectOfType<Player>());
                    saveData.SaveGameLogicData(FindObjectOfType<GameLogic>());
                    FindObjectOfType<MySceneManager>().LoadStore();
                }
            }
        }

    }
}
