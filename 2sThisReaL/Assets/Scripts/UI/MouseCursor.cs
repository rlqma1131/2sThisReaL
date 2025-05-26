using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour
{
    public Texture2D defaultCursor;
    public Texture2D attackCursor;
    public Texture2D pickupCursor;
    public Texture2D talkCursor;
    public Vector2 hotSpot = Vector2.zero;
    public CursorMode cursorMode = CursorMode.Auto;

    void Start()
    {
        SetDefaultCursor();
    }
    
    public void SetDefaultCursor()
    {
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
    }

    public void SetAttackCursor()
    {
        Cursor.SetCursor(attackCursor, Vector2.zero, CursorMode.Auto);
    }

    public void SetPickupCursor()
    {
        Cursor.SetCursor(pickupCursor, Vector2.zero, CursorMode.Auto);
    }

    public void SetTalkCursor()
    {
        Cursor.SetCursor(talkCursor, Vector2.zero, CursorMode.Auto);
    }
}
