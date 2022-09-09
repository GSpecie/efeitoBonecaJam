using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{

	[SerializeField] Dialogue dialogue;
    [SerializeField] DialogueManager myDialogueManager;
    bool alreadyTriggered = false;

    public void TriggerDialogue()
	{
        myDialogueManager.StartDialogue(dialogue);
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Player" && alreadyTriggered == false)
        {
            TriggerDialogue();
            alreadyTriggered = true;
        }
    }
}