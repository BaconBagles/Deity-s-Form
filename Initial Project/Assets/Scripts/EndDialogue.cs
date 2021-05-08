using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndDialogue : MonoBehaviour
{
    public TMPro.TextMeshProUGUI dialogueText;

    public Dialogue dialogue;
    public GameObject textBox;

    Queue<string> sentences;

    bool sentenceFinished;
    bool talking;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
        sentenceFinished = false;
        TriggerDialogue();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        textBox.SetActive(true);
        sentences.Clear();


        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {

        if (talking == false)
        {
            if (sentences.Count == 0)
            {
                EndTheDialogue();
                return;
            }

            string sentence = sentences.Dequeue();
            StopAllCoroutines();
            StartCoroutine(TypeSentence(sentence));
            sentenceFinished = false;
        }
        else
        {
            sentenceFinished = true;
        }
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        talking = true;
        foreach (char letter in sentence.ToCharArray())
        {
            if (sentenceFinished == false)
            {
                dialogueText.text += letter;
                yield return new WaitForSecondsRealtime(0.05f);
            }
            else if (sentenceFinished == true)
            {
                dialogueText.text += letter;
            }
        }
        sentenceFinished = true;
        talking = false;
    }

    void EndTheDialogue()
    {
        sentenceFinished = false;
        textBox.SetActive(false);

    }

    public void TriggerDialogue()
    {
        StartDialogue(dialogue);
    }

}

