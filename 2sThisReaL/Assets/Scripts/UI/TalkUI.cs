using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class TalkUI : MonoBehaviour
{
    [SerializeField] private RectTransform dialoguePanel; // 대화 판넬
    [SerializeField] private float slideDuration = 0.5f;   // 지속 시간
    [SerializeField] private RectTransform playerImage;
    [SerializeField] private RectTransform npcImage;
    [SerializeField] private float characterEnterDuration = 0.5f;
    [SerializeField] private TextMeshProUGUI dialogueText; // 텍스트 효과
    [SerializeField] private float typingDuration = 1.5f; // 길이 조절

    private Vector2 hiddenPosition = new Vector2(0, -900); // 화면 밖
    private Vector2 visiblePosition = new Vector2(0, -500); // 기존 위치

    private Vector2 playerHiddenPos = new Vector2(-1000, 0);
    private Vector2 npcHiddenPos = new Vector2(1000, 0);
    private Vector2 visiblePos = Vector2.zero;
    private Vector2 targetScale = Vector2.zero;
    Vector2 offScreenLeft = new Vector2(-Screen.width, 0);


    private void Start()
    {
        playerImage.anchoredPosition = offScreenLeft;
        dialoguePanel.DOAnchorPos(Vector2.zero, 0.5f)
    .SetEase(Ease.OutBack)
    .SetUpdate(true); // 시간 정지 상태에서도 작동
        npcImage.anchoredPosition = npcHiddenPos;
        dialoguePanel.anchoredPosition = hiddenPosition;
    }

    public void PlayDialogue(string dialogue)
    {
        ShowCharacters();         // 캐릭터 양쪽 등장
    }

    public void ShowCharacters()
    {
        playerImage.DOAnchorPos(visiblePos, characterEnterDuration).SetEase(Ease.OutBack);
        playerImage.transform.DOScale(targetScale, 3).SetEase(Ease.InOutBack);
        npcImage.DOAnchorPos(visiblePos, characterEnterDuration).SetEase(Ease.OutBack);
    }

    public void ShowDialogue()
    {
        dialoguePanel.DOAnchorPos(visiblePosition, slideDuration).SetEase(Ease.OutCubic);
    }

    public void HideDialogue()
    {
        dialoguePanel.DOAnchorPos(hiddenPosition, slideDuration).SetEase(Ease.InCubic);
    }

    void Update()
    {
        
    }
}
