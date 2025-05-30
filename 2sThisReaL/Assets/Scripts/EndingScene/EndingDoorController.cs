using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingDoorController : MonoBehaviour
{
    private Animator animator;
    private bool hasOpened = false;

    public float delayBeforeFade = 1.5f; // 애니메이션 후 페이드 시작 시간
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasOpened && other.CompareTag("Player"))
        {
            hasOpened = true;
            animator.SetTrigger("Open");
            
            Invoke(nameof(TriggerFadeOut), delayBeforeFade);
        }
    }

    private void TriggerFadeOut()
    {
        FindObjectOfType<ScreenFade>().StartFadeToWhite("HappyEndingScene");
    }
}
