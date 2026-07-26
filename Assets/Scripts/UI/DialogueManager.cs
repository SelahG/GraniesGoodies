using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueCheckpoint : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed = 0.05f;
    public float timeBetweenLines = 2f;

    private int index;
    private bool dialogueActive;

    private void Start()
    {
        textComponent.text = string.Empty;
        textComponent.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !dialogueActive)
        {
            StartDialogue();
        }
    }

    private void StartDialogue()
    {
        if (lines.Length == 0)
            return;

        index = 0;
        dialogueActive = true;

        textComponent.text = string.Empty;
        textComponent.gameObject.SetActive(true);

        StartCoroutine(TypeLine());
    }

    private IEnumerator TypeLine()
    {
        foreach (char character in lines[index])
        {
            textComponent.text += character;
            yield return new WaitForSeconds(textSpeed);
        }

        yield return new WaitForSeconds(timeBetweenLines);
        NextLine();
    }

    private void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            dialogueActive = false;
            textComponent.text = string.Empty;
            textComponent.gameObject.SetActive(false);
        }
    }
}