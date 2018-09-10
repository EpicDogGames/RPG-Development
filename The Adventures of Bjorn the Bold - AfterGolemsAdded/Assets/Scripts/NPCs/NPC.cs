using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Interactable {

	public string[] dialogue;
	public string npcName;

	// this handles dialog of npc that aren't quest givers as well as map markers
	public override void Interact()  {
		DialogueManager.Instance.AddNewDialogue (dialogue, npcName);
		MapController.Instance.AddGuardianMarkerToMap (npcName);
	}

}
