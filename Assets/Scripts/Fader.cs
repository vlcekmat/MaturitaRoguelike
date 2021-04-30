using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this script handles the fader animation, that is used for smooth transitions between levels
public class Fader : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    public void Fade()
    {
        animator.ResetTrigger("Inactive");
        animator.SetTrigger("TriggerFader");
        StartCoroutine(Wait(0.5f));
    }

    private IEnumerator Wait(float sec)
    {
        yield return new WaitForSeconds(sec);
        animator.ResetTrigger("TriggerFader");
        animator.SetTrigger("Inactive");
    }
}
