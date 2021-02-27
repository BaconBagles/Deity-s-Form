using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public bool response;
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
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
        if (response == true)
        {
            FindObjectOfType<DialogueManager>().response = true;
            FindObjectOfType<DialogueManager>().responseNumber = responseNumber;
        }
    }
}
