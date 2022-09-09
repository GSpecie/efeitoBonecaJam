using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{

	//public Text nameText;
	public TextMeshProUGUI dialogueText;

    public Animator animator;

    private Queue<string> sentences;

	// Use this for initialization
	void Start()
	{
		sentences = new Queue<string>();
	}

	public void StartDialogue(Dialogue dialogue)
	{
        animator.SetBool("IsOpen", true);

        //nameText.text = dialogue.name;

        sentences.Clear();

		foreach (string sentence in dialogue.sentences)
		{
			sentences.Enqueue(sentence);
		}

		StopAllCoroutines();

		DisplayNextSentence();

		StartCoroutine(WaitingPlayerRead());
	}

	IEnumerator WaitingPlayerRead()
    {
		yield return new WaitForSeconds(5f);
		DisplayNextSentence();

		StartCoroutine(WaitingPlayerRead());
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
	}

	IEnumerator TypeSentence(string sentence)
	{
		dialogueText.text = "";
		foreach (char letter in sentence.ToCharArray())
		{
			dialogueText.text += letter;
			yield return null;
		}
	}

	void EndDialogue()
	{
		StopAllCoroutines();
		animator.SetBool("IsOpen", false);
    }

}