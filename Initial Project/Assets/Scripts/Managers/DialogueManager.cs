using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public OptionsMenu options;

    public TMPro.TextMeshProUGUI nameText;
    public TMPro.TextMeshProUGUI dialogueText;

    public GameObject textBox;

    Queue<string> sentences;

    public DialogueTrigger[] triggers;
    public bool response;
    public int responseNumber;

    bool sentenceFinished;
    

    void Start()
    {
        sentences = new Queue<string>();
        sentenceFinished = false;
    }

    public void StartDialogue(Dialogue dialogue)
    {
        textBox.SetActive(true);
        Time.timeScale = 0f;
        options.GameIsPaused = true;

        nameText.text = dialogue.name;

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
        sentenceFinished = false;
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        if (sentenceFinished == false)
        {
            foreach (char letter in sentence.ToCharArray())
            {
                dialogueText.text += letter;
                yield return new WaitForSecondsRealtime(0.05f);
            }
        }
        else if (sentenceFinished == true)
        {
            foreach (char letter in sentence.ToCharArray())
            {
                dialogueText.text += letter;
            }
        }
    }

    void EndDialogue()
    {
        sentenceFinished = false;
        if (response == true)
        {
            response = false;
            triggers[responseNumber].TriggerDialogue();
        }
        else
        {
            Time.timeScale = 1f;
            options.GameIsPaused = false;

            textBox.SetActive(false);
        }
    }
}
