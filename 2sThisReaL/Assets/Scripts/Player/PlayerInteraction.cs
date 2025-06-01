using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float interactionDistance = 2f;
    [SerializeField] private LayerMask interactable;

    private IInteractable currentTarget;

    void Update()
    {
        FindInteractable();

        if (currentTarget != null && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log($"[E 입력] {currentTarget.GetInteractPrompt()}");
            currentTarget.OnInteract();
        }
    }

    private void FindInteractable()
    {
        currentTarget = null;

        // 플레이어 위치를 중심으로 구 범위 내의 Collider 감지
        Collider[] hits = Physics.OverlapSphere(transform.position, interactionDistance, interactable);

        foreach (Collider hit in hits)
        {
            if (hit.TryGetComponent<IInteractable>(out var interactableObj))
            {
                currentTarget = interactableObj;
                Debug.Log("[상호작용 UI 표시 시도] " + interactableObj.GetInteractPrompt());
                NPCInteract.Instance.Show(interactableObj.GetInteractPrompt());
                return;
            }
        }

        // 감지된 대상이 없으면 UI 숨김
        NPCInteract.Instance.Hide();
    }
}