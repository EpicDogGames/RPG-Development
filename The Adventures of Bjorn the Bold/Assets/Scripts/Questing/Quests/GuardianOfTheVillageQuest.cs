using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GuardianOfTheVillageQuest : Quest {

	private QuestDataManager QDM = null;
	private bool questXMLExists;
	private CollectionGoal collectionGoal;

	// Use this for initialization
	void Start () {

		QDM = new QuestDataManager ();
		questXMLExists = false;

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
		//Goals = new List<Goal> {
		//	new CollectionGoal(this, "collect", "sacredBook", "Find the sacred book", false, 0, 1)
		//};
		Goals = new List<Goal> ();
		if (System.IO.File.Exists(Application.persistentDataPath + "/Quests/" + QuestXMLFile))  {
			// restore quest information from previous saved game
			questXMLExists = true;
			QDM.Load (Application.persistentDataPath + "/Quests/" + QuestXMLFile);
			for (int i=0; i<QDM.QD.GI.Count; i++)  {
				if (QDM.QD.GI[i].gType == "collect")  {
					collectionGoal = new CollectionGoal (this, QDM.QD.GI[i].gType, QDM.QD.GI[i].gTarget, QDM.QD.GI[i].gDescription, QDM.QD.GI[i].gCompleted, QDM.QD.GI[i].gCurrentAmount, QDM.QD.GI[i].gRequiredAmount);
					Goals.Add (collectionGoal);
				}
			}
		}
		else  {
			CollectionGoal collectionGoal = new CollectionGoal (this, "collect", "sacredBook", "Find the sacred book", false, 0, 1);
			Goals.Add (collectionGoal);
		}

		// go thru each goal in collection and then initialize the method
		Goals.ForEach (g => g.Init ());

		// build an xml containing quest details for use later
		if (!questXMLExists) {
			QuestController.Instance.PopulateQuestDataXMLFile (this);
		}

		// populate quest ui .. if only single quest exists, can be displayed in quest panel otherwise must select from listed quests in quests panel
		DirectoryInfo dir = new DirectoryInfo (Application.persistentDataPath + "/Quests");
		FileInfo[] info = dir.GetFiles ("*.xml");
		if (info.Length == 1) {
			if (!questXMLExists) {
				QuestUIManager.Instance.SetupSingleQuest (this);
			}
			else  {
				QuestUIManager.Instance.SetupSingleOldQuest (this, QuestXMLFile);
			}
		}
		QuestUIManager.Instance.BuildQuestButton (this);
	}

}
