using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class Quest : MonoBehaviour {

	public List<Goal> Goals { get; set; } 
	public string QuestName { get; set; }
	public string QuestDescription { get; set; }
	public string QuestImageName { get; set; }
	public string QuestXMLFile { get; set; }
	public Item ItemReward { get; set; }
	public bool QuestCompleted { get; set; }
	public string[] inProgressDialogue, rewardDialogue, completedDialogue;  // additional dialogue strings

	public void CheckGoals()  {
		// called from Goal.cs when a goal is completed
		// goes through all the goals to find out if they have been completed
		//   if any of the goals isn't completed, QuestCompleted is false otherwise true
		QuestCompleted = Goals.All (g => g.GoalCompleted);
	}

	public void GiveReward()  {
		if (ItemReward != null)  {
			if (ItemReward.ItemName == "Bag of Health and Mana Elixirs" || ItemReward.ItemName == "Bag of Health Elixirs" || ItemReward.ItemName == "Bag Of Mana Elixirs") {
				if (ItemReward.ItemName == "Bag of Health and Mana Elixirs" || ItemReward.ItemName == "Bag of Health Elixirs") {
					for (var i = 0; i < 5; i++) {
						InventoryController.Instance.GiveItem ("healthElixir");
					}
				}
				if (ItemReward.ItemName == "Bag of Health and Mana Elixirs" || ItemReward.ItemName == "Bag of Mana Elixirs") {
					for (var i = 0; i < 5; i++) {
						InventoryController.Instance.GiveItem ("manaElixir");
					}
				}
			} 
			else {
				InventoryController.Instance.GiveItem (ItemReward);
			}
		}

		// set up some individual quest completions
		switch (QuestName)  {
			case "Guardian of the Grove":
				Debug.Log ("Guardian of the Grove Quest Completed");
				QuestUIManager.Instance.questStatus.text = "COMPLETED";
				QuestUIManager.Instance.questStatusImage.GetComponent<Image> ().color = new Color32 (0, 118, 255, 255);
				break;
			case "Guardian of the Village":
				Debug.Log ("Guardian of the Village Quest Completed");
				QuestUIManager.Instance.questStatus.text = "COMPLETED";
				QuestUIManager.Instance.questStatusImage.GetComponent<Image> ().color = new Color32 (0, 118, 255, 255);
				break;
			case "Guardian of the Sacred Forest":
				Debug.Log ("Guardian of the Sacred Forest Quest Completed");
				GameObject curse = GameObject.Find ("Mysterious Mist");
				if (curse.activeInHierarchy)  {
					curse.SetActive (false);
				}
				QuestUIManager.Instance.questStatus.text = "COMPLETED";
				QuestUIManager.Instance.questStatusImage.GetComponent<Image> ().color = new Color32 (0, 118, 255, 255);
				break;
			case "Guardian of the Willows":
				Debug.Log ("Guardian of the Willows Quest Completed");
				QuestUIManager.Instance.questStatus.text = "COMPLETED";
				QuestUIManager.Instance.questStatusImage.GetComponent<Image> ().color = new Color32 (0, 118, 255, 255);
				break;
		}

		// update xml file of the quest
		QuestController.Instance.PopulateQuestDataXMLFile (this);

	}


}
