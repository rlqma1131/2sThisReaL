using UnityEngine;

public class NPCInteract : MonoBehaviour
{
    private MouseCursor mouseCursor;

    [Header("Item")]
    [SerializeField] private ItemData itemToGive;
    
    private bool hasInteracted = false;

    void Start()
    {
        mouseCursor = FindObjectOfType<MouseCursor>();
    }

    void OnMouseEnter()
    {
        if (!hasInteracted && mouseCursor != null)
            mouseCursor.SetTalkCursor();
    }

    void OnMouseExit()
    {
        if (mouseCursor != null)
            mouseCursor.SetDefaultCursor();
    }

    void OnMouseDown()
    {
        if (hasInteracted) return;

        Player player = GameManager.Instance.Player;
        if (player != null && itemToGive != null)
        {

            hasInteracted = true;

            // UIManager.Instance.ShowMessage(interactionMessage);

            player.additem?.Invoke();
        }
    }
}
