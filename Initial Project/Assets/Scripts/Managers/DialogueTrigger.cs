using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public bool response;
    public bool playerTalking;
    public int responseNumber;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == ("Player"))
        {
            TriggerDialogue();
            Destroy(gameObject);
        }
    }

    public void TriggerDialogue()
    {

        if (playerTalking == true)
        {
            FindObjectOfType<DialogueManager>().playerTalking = true;
        }

        if (response == true)
        {
            FindObjectOfType<DialogueManager>().response = true;
            FindObjectOfType<DialogueManager>().responseNumber = responseNumber;
        }

        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
}
