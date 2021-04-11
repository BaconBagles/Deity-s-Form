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
    public GameObject portrait;
    public bool playerTalking;

    Queue<string> sentences;

    public DialogueTrigger[] triggers;
    public bool response;
    public int responseNumber;

    bool sentenceFinished;
    bool talking;
    

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
        if (playerTalking == true)
        {
            portrait = textBox.transform.Find("playerPortrait").gameObject;
        }
        else
        {
            portrait = textBox.transform.Find("otherPortrait").gameObject;
        }
        portrait.GetComponent<Image>().sprite = dialogue.portrait;
        portrait.gameObject.SetActive(true);

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {

        if (talking == false)
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

    void EndDialogue()
    {
        portrait.gameObject.SetActive(false);
        playerTalking = false;
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
