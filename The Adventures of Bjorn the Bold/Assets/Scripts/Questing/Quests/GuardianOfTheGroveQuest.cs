using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GuardianOfTheGroveQuest : Quest {

	// Use this for initialization
	void Start () {

		// define xml file that will contain quest details
		QuestXMLFile = "GuardianOfTheGrove.xml";

		// setting up the quest
		QuestName = "Guardian of the Grove";
		QuestDescription = "The grove has been overrun with rogues who terrorize any who enter this lovely grove. They have also taken the several bones from the guardian. The Guardian of the Grove has asked for your help. Kill all rogues in the guardian's grove and retrieve his missing bones. He will grant you a faboulous reward if you complete this quest.";
		QuestImageName = "GuardianOfTheGrove";
		ItemReward = ItemDatabase.Instance.GetItem ("axe1");

		// new dialogue
		inProgressDialogue = new string[]  {
			"I can't give you your reward until all the rogues have been killed and my bones returned to me!",
			"Please help me out."
		};
		rewardDialogue = new string[]  {
			"Oh Adventurer you have returned my grove to me as well as my bones!",
			"You deserve this reward for all your hard work.",
			"The goblin king's battleaxe is a very powerful weapon and should help you on your quest."
		};
		completedDialogue = new string[]  {
			"Thank you for ridding my grove of those awful villains.", 
			"Travellers have returned  and are enjoying my grove again."
		};

		// setting up the goals
		Goals = new List<Goal> {
			new KillGoal(this, "kill", "rogue", "Kill 10 rogues", false, 0, 10),
			new CollectionGoal(this, "collect", "bone", "Collect 5 stolen bones", false, 0, 5)
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
