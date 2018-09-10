using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GuardianOfTheSacredForestQuest : Quest {

	private QuestDataManager QDM = null;
	private bool questXMLExists;
	private CollectionGoal collectionGoal;

	// Use this for initialization
	void Start () {

		QDM = new QuestDataManager ();
		questXMLExists = false;

		// define xml file to contain quest details
		QuestXMLFile = "GuardianOfTheSacredForest.xml";

		// setting up the quest
		QuestName = "Guardian of the Sacred Forest";
		QuestDescription = "A horrible curse has befallen the village. The Guardian of the Village has asked you to take the books you have found to the Guardian of the Sacred Forest. He hopes that this guardian will be able to lift the curse. However the Guardian of the Sacred Forest can't help as several pages from the books are missing. The pages have been buried in a nearby graveyard and it is your task to retrieve them.";
		QuestImageName = "GuardianOfTheSacredForest";
		ItemReward = ItemDatabase.Instance.GetItem ("shield1");

		// new dialogue
		inProgressDialogue = new string[] {
			"Without those pages for me to read, I cannot remove the deed."
		};
		rewardDialogue = new string[]  {
			"With these eight pages the spell is broken, take this shining shield as a token."
		};
		completedDialogue = new string[]  {
			"Because you heeded the villagers' plea, they decided to leave my forest be.",
			"Thank you for your noble deed."
		};

		// setting up the goals
		//Goals = new List<Goal> {
		//	new CollectionGoal(this, "collect", "lostPage", "Recover 8 pages from sacred book", false, 0, 8)
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
			CollectionGoal collectionGoal = new CollectionGoal (this, "collect", "lostPage", "Recover 8 pages from sacred book", false, 0, 8);
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
