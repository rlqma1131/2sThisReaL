using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NpcType
{
    Type1,
    Type2
}

public class NPC : MonoBehaviour, IInteractable
{
    [Header("NPC Item")]
    [SerializeField] private ItemData[] itemData;

    private Animator anim;
    private Collider npcCollider;
    private float playerDistance;
    private bool hasTalked = false;
    public NpcType npcType;
    public bool HasTalked => hasTalked;

    private void Start()
    {
        anim = GetComponent<Animator>();
        npcCollider = GetComponent<Collider>();
    }

    public string GetInteractPrompt()
    {
        return "확인하기";
    }

    public void OnInteract()
    {
        playerDistance = Vector3.Distance(GameManager.Instance.Player.transform.position, transform.position);

        if (playerDistance < 10f)
        {
            if (!hasTalked)
            {
                TalkUI.Instance?.OpenDialogue(this);
            }
            else
            {
                TalkUI.Instance?.ShowDeadDialogue();
            }
        }
    }
    public void MarkAsTalked()
    {
        hasTalked = true;
    }

    public void OnRobbery() // 주머니 뒤져서 강탈했을 때 호출
    {
        for (int i = 0; i < 3 && i < itemData.Length; i++)
        {
            if (itemData[i] != null)
            {
                GameManager.Instance.Player.itemQueue.Add(itemData[i]);
                GameManager.Instance.Player.additem?.Invoke();
            }
        }
    }

    public void OnTalk() // 대화했을 때 호출
    {
        foreach (ItemData data in itemData)
        {
            if (data != null)
            {
                GameManager.Instance.Player.itemQueue.Add(data);
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
