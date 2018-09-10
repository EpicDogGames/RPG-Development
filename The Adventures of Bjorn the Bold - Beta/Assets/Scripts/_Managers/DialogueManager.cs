using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

	public static DialogueManager Instance { get; set; }
	public GameObject dialoguePanel;
	public List<string> dialogueLines = new List<string> ();
	public string npcName;

	Button continueButton, skipButton;
	Text dialogueText, nameText;
	int dialogueIndex;

	void Awake()  {

		continueButton = dialoguePanel.transform.Find ("Continue").GetComponent<Button> ();
		skipButton = dialoguePanel.transform.Find ("Skip").GetComponent<Button> ();
		dialogueText = dialoguePanel.transform.Find ("DialogueText").GetComponent<Text> ();
		nameText = dialoguePanel.transform.Find ("NamePanel").GetChild (0).GetComponent<Text>();

		dialoguePanel.SetActive (false);

		continueButton.onClick.AddListener (delegate {
			ContinueDialogue ();
		});
		skipButton.onClick.AddListener (delegate {
			SkipDialogue ();
		});

		if (Instance != null && Instance != this)  {
			Destroy (gameObject);
		}
		else {
			Instance = this;
		}
	}

	public void AddNewDialogue(string[] lines, string npcName)  {
		dialogueIndex = 0;
		dialogueLines = new List<string> (lines.Length);
		dialogueLines.AddRange (lines);
		this.npcName = npcName;
		CreateDialogue ();
	}

	public void CreateDialogue()  {
		dialogueText.text = dialogueLines [dialogueIndex];
		nameText.text = npcName;
		dialoguePanel.SetActive (true);
	}

	public void ContinueDialogue() {
		if (dialogueIndex < dialogueLines.Count-1)  {
			dialogueIndex++;
			dialogueText.text = dialogueLines [dialogueIndex];
		}
		else  {
			dialoguePanel.SetActive (false);
		}
	}

	public void SkipDialogue()  {
		dialoguePanel.SetActive (false);
	}

}

