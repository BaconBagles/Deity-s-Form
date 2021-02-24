using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public bool response;
    public int responseNumber;

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
        if (response == true)
        {
            FindObjectOfType<DialogueManager>().response = true;
            FindObjectOfType<DialogueManager>().responseNumber = responseNumber;
        }
    }
}
