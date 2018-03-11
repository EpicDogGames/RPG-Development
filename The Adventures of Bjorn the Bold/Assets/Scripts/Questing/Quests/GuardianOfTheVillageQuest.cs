using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GuardianOfTheVillageQuest : Quest {

	// Use this for initialization
	void Start () {

		// define xml containing quest details
		QuestXMLFile = "GuardianOfTheVillage.xml";

		// setting up the quest
		QuestName = "Guardian of the Village";
		QuestDescription = "The village has been cursed due to the villagers cutting down trees from a sacred forest. The guardian is asking for your help in lifting the curse by finding a book containing the answer to getting rid of the curse.";
		QuestImageName = "GuardianOfTheVillage";
		ItemReward = ItemDatabase.Instance.GetItem ("bagOfHealthAndManaElixirs");

		// new dialogue
		inProgressDialogue = new string[] {
			"Only that sacred book can lift this awful curse.",
			"Don't abandon me in my hour of need."
		};
		rewardDialogue = new string[]  {
			"You've found the book. Saints preserve us.",
			"Now take the book to the Guardian of the Sacred Forest. He should be able to lift the curse.",
			"Look to the southeast of the village near the river's edge.",
			"Bless you my boy. For all your troubles, here is a box of powerful elixirs for you."
		};
		completedDialogue = new string[]  {
			"Thank you for helping to remove the curse.", 
			"I am deeply indebted to you."
		};

		// setting up the goals
		Goals = new List<Goal> {
			new CollectionGoal(this, "collect", "sacredBook", "Find the sacred book", false, 0, 1)
		};

		// go thru each goal in collection and then initialize the method
		Goals.ForEach (g => g.Init ());

		// build an xml containing quest details for use later
		QuestController.Instance.PopulateQuestDataXMLFile (this);

		// populate quest ui .. if only single quest exists, can be displayed in quest panel otherwise must select from listed quests in quests panel
		DirectoryInfo dir = new DirectoryInfo (Application.persistentDataPath + "/Quests");
		FileInfo[] info = dir.GetFiles ("*.xml");
		if (info.Length == 1) {
			QuestUIManager.Instance.SetupSingleQuest (this);
		}
		QuestUIManager.Instance.BuildQuestButton (this);
	}

}
