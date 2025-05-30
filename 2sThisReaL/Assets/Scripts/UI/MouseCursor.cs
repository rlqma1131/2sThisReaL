using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour
{
    public Texture2D defaultCursor;
    public Texture2D attackCursor;
    public Texture2D pickupCursor;
    public Texture2D talkCursor;
    public Texture2D destroyCursor;
    public Texture2D buildCursor;
    public Vector2 hotSpot = Vector2.zero; // 커서 좌표
    public CursorMode cursorMode = CursorMode.Auto; // 커서 모드 설정 (기본값 사용)
    void Awake()
    {
        if (FindObjectsOfType<MouseCursor>().Length > 1)
        {
            Destroy(gameObject); // 중복 방지
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        SetDefaultCursor(); // 해당되는 마우스 커서 활성화
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

    public void SetBuildCursor()
    {
        Cursor.SetCursor(buildCursor, Vector2.zero, CursorMode.Auto);
    }

    public void SetDestroyCursor()
    {
        Cursor.SetCursor(destroyCursor, Vector2.zero, CursorMode.Auto);
    }

}
