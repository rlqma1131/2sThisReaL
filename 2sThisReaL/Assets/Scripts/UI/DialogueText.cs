using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private float typingSpeed = 0.05f;

    private Queue<string> dialogueLines = new Queue<string>();
    private Coroutine typingCoroutine;

    public bool IsFinished { get; private set; } = false;

    private int currentIndex = 0;
    private int totalLines = 0;

    public void StartDialogue(string[] lines)
    {
        dialogueLines.Clear();
        foreach (string line in lines)
            dialogueLines.Enqueue(line);

        currentIndex = 0;
        totalLines = lines.Length;

        IsFinished = false;
        DisplayNextLine();
    }

    private void DisplayNextLine()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        if (dialogueLines.Count == 0)
        {
            dialogueText.text = "";
            IsFinished = true;
            return;
        }

        string line = dialogueLines.Dequeue();
        typingCoroutine = StartCoroutine(TypeLine(line));
    }

    private IEnumerator TypeLine(string line)
    {
        bool isNarration = line.StartsWith("[");
        bool isPlayerSpeaking = false;

        if (!isNarration && currentIndex == totalLines - 1) // 마지막은 플레이어 대사
            isPlayerSpeaking = true;

        TalkUI.Instance?.UpdateNameTagVisibility(isNarration, isPlayerSpeaking);

        string content = isNarration ? line : line;

        dialogueText.text = "";
        foreach (char c in content)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        currentIndex++;

        yield return new WaitUntil(() =>
        Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
        DisplayNextLine();
    }
}