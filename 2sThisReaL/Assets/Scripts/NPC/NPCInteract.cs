using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteract : MonoBehaviour
{
    private MouseCursor mouseCursor;

    void Start()
    {
        mouseCursor = FindObjectOfType<MouseCursor>();
    }

    void Update()
    {
        
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
