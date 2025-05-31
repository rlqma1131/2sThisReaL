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

    public void StartDialogue(string[] lines)
    {
        dialogueLines.Clear();
        foreach (string line in lines)
            dialogueLines.Enqueue(line);

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
        dialogueText.text = "";
        foreach (char c in line)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        DisplayNextLine();
    }
}