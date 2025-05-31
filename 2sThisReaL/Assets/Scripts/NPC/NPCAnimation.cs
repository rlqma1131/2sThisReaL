using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimation : MonoBehaviour
{
    private Animator anim;
    private Collider npcCollider;

    private void Start()
    {
        anim = GetComponent<Animator>();
        npcCollider = GetComponent<Collider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (anim != null)
            {
                anim.SetBool("Moving", true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (anim != null)
            {
                anim.SetBool("Moving", false);
            }
        }
    }
}
