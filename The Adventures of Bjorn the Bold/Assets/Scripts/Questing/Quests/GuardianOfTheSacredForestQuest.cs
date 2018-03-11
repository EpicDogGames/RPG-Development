using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GuardianOfTheSacredForestQuest : Quest {

	// Use this for initialization
	void Start () {

		// define xml file to contain quest details
		QuestXMLFile = "GuardianOfTheSacredForest.xml";

		// setting up the quest
		QuestName = "Guardian of the Sacred Forest";
		QuestDescription = "A horrible curse has befallen the village. The Guardian of the Village has asked you to take the sacred book you found to the Guardian of the Sacred Forest in the hope he will be able to lift the curse. However the Guardian of the Sacred Forest can't help as several pages from the book are missing. The pages have been buried in a nearby graveyard and it is your task to retrieve them.";
		QuestImageName = "GuardianOfTheSacredForest";
		ItemReward = ItemDatabase.Instance.GetItem ("shield1");

		// new dialogue
		inProgressDialogue = new string[] {
			"You still lack all the lost pages necessary to lift this curse."
		};
		rewardDialogue = new string[]  {
			"With all pages returned to me, I can now lift the curse.",
			"As a reward, the village commissioned a wonderous armorer to create a special shield",
			"I'm sure it will be of great value to you on your journey."
		};
		completedDialogue = new string[]  {
			"Thank you for helping to remove the curse."
		};

		// setting up the goals
		Goals = new List<Goal> {
			new CollectionGoal(this, "collect", "lostPage", "Recover 8 pages from sacred book", false, 0, 8)
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
