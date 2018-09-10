using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GuardianOfTheWillowsQuest : Quest {

	private QuestDataManager QDM = null;
	private bool questXMLExists;
	private CollectionGoal collectionGoal1, collectionGoal2;

	// Use this for initialization
	void Start () {

		QDM = new QuestDataManager ();
		questXMLExists = false;

		// define xml file to contain quest details
		QuestXMLFile = "GuardianOfTheWillows.xml";

		// setting up the quest
		QuestName = "Guardian of the Willows";
		QuestDescription = "A band of beavers has dammed up the river, denying the willows water. The guardian needs your help to unblock the river and allow his willows to drink again.";
		QuestImageName = "GuardianOfTheWillows";
		//ItemReward = ItemDatabase.Instance.GetItem ("shield1");

		// new dialogue
		inProgressDialogue = new string[] {
			"Without the rare powder and empty kegs, I must continue to beg.",
			"Please help me change the river's flow and deliver my willows from their woe."
		};
		rewardDialogue = new string[]  {
			"The waters of the river are blocked as the secret to these items is still locked.",
			"Do not hesitate. Do not falter. Seek your answer at the river's altar."
		};
		completedDialogue = new string[]  {
			"Be on your way my fine young man. Continue your quest of this strange land."
		};

		Goals = new List<Goal> ();
		if (System.IO.File.Exists(Application.persistentDataPath + "/Quests/" + QuestXMLFile))  {
			// restore quest information from previous saved game
			questXMLExists = true;
			QDM.Load (Application.persistentDataPath + "/Quests/" + QuestXMLFile);
			for (int i=0; i<QDM.QD.GI.Count; i++)  {
				if (QDM.QD.GI [i].gType == "collect") {
					if (i == 0) {
						collectionGoal1 = new CollectionGoal (this, QDM.QD.GI [i].gType, QDM.QD.GI [i].gTarget, QDM.QD.GI [i].gDescription, QDM.QD.GI [i].gCompleted, QDM.QD.GI [i].gCurrentAmount, QDM.QD.GI [i].gRequiredAmount);
						Goals.Add (collectionGoal1);
					}
					if (i == 1) {
						collectionGoal2 = new CollectionGoal (this, QDM.QD.GI [i].gType, QDM.QD.GI [i].gTarget, QDM.QD.GI [i].gDescription, QDM.QD.GI [i].gCompleted, QDM.QD.GI [i].gCurrentAmount, QDM.QD.GI [i].gRequiredAmount);
						Goals.Add (collectionGoal2);
					}
				}
			}
		}
		else  {
			CollectionGoal collectionGoal1 = new CollectionGoal (this, "collect", "rarePowder", "Retrieve the rare powder", false, 0, 1);
			Goals.Add (collectionGoal1);
			CollectionGoal collectionGoal2 = new CollectionGoal (this, "collect", "keg", "Gather 3 empty kegs", false, 0, 3);
			Goals.Add (collectionGoal2);
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
