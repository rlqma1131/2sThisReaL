using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditRoll : MonoBehaviour
{
    public RectTransform creditsPanel;  // 움직일 대상
    public float scrollSpeed = 50f;     // 스크롤 속도 (픽셀/초)
    public float endY = 1000f;          // 종료 Y 위치
    private Vector2 startPos;

    void Start()
    {
        if (creditsPanel != null)
        {
            startPos = creditsPanel.anchoredPosition;
        }
    }

    void Update()
    {
        if (creditsPanel == null) return;

        creditsPanel.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;

        if (creditsPanel.anchoredPosition.y >= endY)
        {
            // 종료 후 리셋하거나, 씬 전환 가능
            creditsPanel.anchoredPosition = startPos;
            // 또는 Destroy(gameObject); 또는 SceneManager.LoadScene(...) 가능
        }
    }
}
