using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : NPC {

	public bool AssignedQuest { get; set; }
	public bool AssignedQuestCompleted { get; set; }

	[SerializeField] private GameObject quests;
	[SerializeField] private string questType;    // string can be converted into an object which becomes a component within the inspector

	private Quest playerQuest { get; set; }

	public override void Interact()  {
		// handle adding marker to map
		MapController.Instance.AddGuardianMarkerToMap (name);
		// conditions to check
		if (!AssignedQuest && !AssignedQuestCompleted)  {
			// this calls the Interact method of the NPC class, meaning will trigger the dialogue of the NPC
			base.Interact();
			AssignQuest();
		}
		else if (AssignedQuest && !AssignedQuestCompleted)  {
			CheckQuestProgress();
		}
		else  {
			DialogueManager.Instance.AddNewDialogue (playerQuest.completedDialogue, name);			
		}
	}

	public void AssignQuest()  {
		// this handles calling the tutorial popup for quest when the very first quest is assigned
		if (GamePreferences.GetFirstQuestEntry() == 1)  {
			GamePreferences.SetFirstQuest(1);
		}
		AssignedQuest = true;
		playerQuest = (Quest)quests.AddComponent (System.Type.GetType(questType));
	}

	void CheckQuestProgress()  {
		if (playerQuest.QuestCompleted)  {
			playerQuest.GiveReward();
			AssignedQuestCompleted = true;
			AssignedQuest = false;
			DialogueManager.Instance.AddNewDialogue (playerQuest.rewardDialogue, name);
			Destroy(playerQuest.GetComponent(System.Type.GetType(questType)));
		}
		else {
			DialogueManager.Instance.AddNewDialogue (playerQuest.inProgressDialogue, name);

		}
	}

	public void AssignedQuestFinished()  {
		playerQuest = (Quest)quests.AddComponent (System.Type.GetType(questType));
		AssignedQuestCompleted = true;
		AssignedQuest = false;
	}
}
