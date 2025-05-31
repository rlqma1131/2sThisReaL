using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class TalkUI : MonoBehaviour
{
    public static TalkUI Instance; // 싱글톤

    [Header("UI 구성 요소")]
    [SerializeField] private CanvasGroup dialoguePanel;
    [SerializeField] private Button[] choiceButtons;
    [SerializeField] private RectTransform cursor;
    [SerializeField] private DialogueText dialogueText;

    [Header("캐릭터 네임텍")]
    [SerializeField] private GameObject playerNameTag;
    [SerializeField] private GameObject npcNameTag;

    [Header("캐릭터 애니메이션")]
    [SerializeField] private Transform playerModel;
    [SerializeField] private Image playerSpriteImage;
    [SerializeField] private Transform npcCharacterModel;
    [SerializeField] private Image npcSpriteImage;

    private NPC currentTarget;
    private bool isNpcTalking = false;

    private void Awake()
    {
        Instance = this;

        dialoguePanel.alpha = 0;
        dialoguePanel.gameObject.SetActive(false);

        // 초기에 선택지off
        foreach (var btn in choiceButtons)
            btn.gameObject.SetActive(false);
        cursor.gameObject.SetActive(false);
    }

    public void OpenDialogue(NPC npc)
    {
        currentTarget = npc;

        dialoguePanel.gameObject.SetActive(true);
        dialoguePanel.alpha = 0;

        SetIdleVisual();
        UpdateNameTagVisibility(true); // 나레이션 시작

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        dialoguePanel.DOFade(1f, 0.4f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            dialogueText.StartDialogue(new string[]
            {
                "[안색이 창백한 사람이다. 어떻게 할까?]"
            });
            StartCoroutine(WaitThenShowChoices());
        });
    }

    private IEnumerator WaitThenShowChoices()
    {
        yield return new WaitUntil(() => dialogueText.IsFinished);
        yield return new WaitForSeconds(0.2f);
        ShowChoices();
    }

    public void ShowChoices()
    {
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            var btn = choiceButtons[i];
            btn.gameObject.SetActive(true);
            btn.transform.localScale = Vector3.zero;
            btn.transform.DOScale(1f, 0.3f).SetDelay(i * 0.1f);

            EventTrigger trigger = btn.GetComponent<EventTrigger>() ?? btn.gameObject.AddComponent<EventTrigger>();
            trigger.triggers.Clear();
            AddPointerEnter(trigger, btn.transform as RectTransform);

            int index = i;
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => OnChoiceSelected(index));
        }
    }

    private void AddPointerEnter(EventTrigger trigger, RectTransform target)
    {
        var entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
        entry.callback.AddListener((_) =>
        {
            cursor.gameObject.SetActive(true);
            cursor.DOMoveY(target.position.y, 0.2f);
        });
        trigger.triggers.Add(entry);
    }

    private void OnChoiceSelected(int index)
    {
        HideChoices();
        SetTalkingState(true);

        string[] dialogueLines;

        if (index == 0) // 대화
        {
            dialogueLines = new string[]
            {
                "[다가가자 남자가 겨우 고개를 든다]",
                "... 거기.. 사람인가?",
                "하... 다행이다...",
                "제발... 이걸... 받아줘...",
                "난... 더는 못 가...",
                "[남자가 힘겹게 소지품이 든 가방을 건낸다.]",
                "..저 성 뒤편... 거긴 아무도 가지 않았지...",
                "너무 좁고... 괜히 돌아가면 시간 낭비 같았거든...",
                "하지만.. 난 봤어...",
                "누군가 그 구석에... 뭔가를 숨기는 걸...",
                "뭐냐고 물었더니 '세상의 끝'이라고 하더군...",
                "자네는 꼭..'세상의 끝'에 다다르기를...",
                "[남자는 말을 마치고 조용히 눈을 감는다]",
                "...평안하기를"
            };

            currentTarget.OnTalk();
        }
        else // 강탈
        {
            dialogueLines = new string[]
            {
                "[그의 소지품을 몰래 가져왔다.]"
            };

            currentTarget.OnRobbery();
        }

        dialogueText.StartDialogue(dialogueLines);

        StartCoroutine(CloseAfterDialogue());
    }

    private IEnumerator CloseAfterDialogue()
    {
        yield return new WaitUntil(() => dialogueText.IsFinished);
        yield return new WaitForSeconds(0.3f);

        currentTarget?.MarkAsTalked();
        CloseDialogue();
    }

    private void HideChoices()
    {
        foreach (var btn in choiceButtons)
        {
            btn.gameObject.SetActive(false);
            btn.onClick.RemoveAllListeners();
        }

        cursor.gameObject.SetActive(false);
    }

    public void CloseDialogue()
    {
        SetTalkingState(false);
        SetIdleVisual();

        dialoguePanel.DOFade(0f, 0.3f).OnComplete(() =>
        {
            dialoguePanel.gameObject.SetActive(false);
        });

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void SetTalkingState(bool isTalking)
    {
        isNpcTalking = isTalking;
    }

    private void PlayPlayerTalkingVisual()
    {
        playerModel?.DOScale(1.2f, 0.2f);
        playerSpriteImage?.DOColor(Color.white, 0.2f);

        npcSpriteImage?.DOColor(new Color(0.5f, 0.5f, 0.5f), 0.2f);
    }

    private void PlayNpcTalkingVisual()
    {
        npcCharacterModel?.DOScale(1.2f, 0.2f);
        npcSpriteImage?.DOColor(Color.white, 0.2f);

        playerSpriteImage?.DOColor(new Color(0.5f, 0.5f, 0.5f), 0.2f);
    }

    private void SetIdleVisual()
    {
        npcCharacterModel?.DOScale(1f, 0.2f);
        npcSpriteImage?.DOColor(new Color(0.5f, 0.5f, 0.5f), 0.2f);

        playerModel?.DOScale(1f, 0.2f);
        playerSpriteImage?.DOColor(new Color(0.5f, 0.5f, 0.5f), 0.2f);
    }
    public void ShowDeadDialogue()
    {
        dialoguePanel.gameObject.SetActive(true);
        dialoguePanel.alpha = 0;

        SetIdleVisual();
        UpdateNameTagVisibility(true);

        npcCharacterModel?.gameObject.SetActive(false);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        dialoguePanel.DOFade(1f, 0.4f).OnComplete(() =>
        {
            dialogueText.StartDialogue(new string[]
            {
            "[싸늘하다. 이미 늦었다. 숨소리가 멎어있다.]"
            });

            StartCoroutine(CloseAfterDialogue());
        });
    }

    public void UpdateNameTagVisibility(bool isNarration, bool isPlayerSpeaking = false)
    {
        if (isNarration)
        {
            playerNameTag.SetActive(false);
            npcNameTag.SetActive(false);
            SetIdleVisual();
        }
        else
        {
            playerNameTag.SetActive(isPlayerSpeaking);
            npcNameTag.SetActive(!isPlayerSpeaking);

            if (isPlayerSpeaking)
                PlayPlayerTalkingVisual();
            else
                PlayNpcTalkingVisual();
        }
    }
}