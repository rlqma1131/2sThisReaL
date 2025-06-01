using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPCInteract : MonoBehaviour
{
    public static NPCInteract Instance;
    private MouseCursor mouseCursor;
    public Image Icon;
    public TextMeshProUGUI promptText;

    void Start()
    {
        mouseCursor = FindObjectOfType<MouseCursor>();
    }

    private void Awake()
    {
        Instance = this;
    }

    public void Show(string prompt)
    {
        promptText.text = prompt;
        Icon.gameObject.SetActive(true);
        gameObject.SetActive(true); // UI 표시
    }

    public void Hide()
    {
        gameObject.SetActive(false); // UI 숨김
    }

    void OnMouseEnter()
    {
        if (mouseCursor != null)
            mouseCursor.SetTalkCursor();
    }

    void OnMouseExit()
    {
        if (mouseCursor != null)
            mouseCursor.SetDefaultCursor();
    }
}
