using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float interactionDistance = 2f;
    [SerializeField] private LayerMask interactableLayer;

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

        Collider[] hits = Physics.OverlapSphere(transform.position, interactionDistance, interactableLayer);

        foreach (Collider hit in hits)
        {
            Debug.Log($"[상호작용 대상 후보]: {hit.name}");

            IInteractable interactable = hit.GetComponent<IInteractable>();
            if (interactable != null)
            {
                currentTarget = interactable;
                Debug.Log($"[감지된 상호작용 대상]: {hit.name}");
                break;
            }
        }
    }
}