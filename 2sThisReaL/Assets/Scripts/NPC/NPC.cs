using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [Header("NPC Item")]
    [SerializeField] private ItemData[] itemData;

    private Animator anim;
    private Collider npcCollider;

    private void Start()
    {
        anim = GetComponent<Animator>();
        npcCollider = GetComponent<Collider>();
    }
    public void OnRobbery() // 강탈했을 때 호출해주세용
    {
        for(int i = 0; i < 3; i++)
        {
            if (itemData[i] != null)
            {
                GameManager.Instance.Player.itemData = itemData[i];
                GameManager.Instance.Player.additem?.Invoke();
            }
        }
    }

    public void OnTalk() // 대화했을 때 호출해주세용
    {
        for(int i = 0; i < itemData.Length; i++)
        {
            if (itemData[i] != null)
            {
                GameManager.Instance.Player.itemData = itemData[i];
                GameManager.Instance.Player.additem?.Invoke();
            }
        }
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
