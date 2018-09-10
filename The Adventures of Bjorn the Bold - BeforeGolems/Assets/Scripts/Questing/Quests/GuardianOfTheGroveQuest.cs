using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GuardianOfTheGroveQuest : Quest {

	private QuestDataManager QDM = null;
	private bool questXMLExists;
	private KillGoal killGoal;
	private CollectionGoal collectionGoal;

	// Use this for initialization
	void Start () {

		QDM = new QuestDataManager ();
		questXMLExists = false;

		// define xml file that will contain quest details
		QuestXMLFile = "GuardianOfTheGrove.xml";

		// setting up the quest
		QuestName = "Guardian of the Grove";
		QuestDescription = "The grove has been overrun with rogues who terrorize any who enter. They have also taken all guardian's bones. The Guardian of the Grove has asked for your help. Kill all rogues in the guardian's grove and retrieve his missing bones. He will grant you a faboulous reward if you complete this quest.";
		QuestImageName = "GuardianOfTheGrove";
		ItemReward = ItemDatabase.Instance.GetItem ("axe1");

		// new dialogue
		inProgressDialogue = new string[]  {
			"The reward you receive is quite grand, if you complete the tasks at hand."
		};
		rewardDialogue = new string[]  {
			"My groves and bones have been returned to me and this weapon is my reward to thee.",
			"Take the battleaxe for your quest and I wish you all the best."
		};
		completedDialogue = new string[]  {
			"Thank you for ridding my grove of those awful villains.", 
			"Travellers have returned  and are enjoying my grove again."
		};

		// setting up the goals
		Goals = new List<Goal> ();
		if (System.IO.File.Exists(Application.persistentDataPath + "/Quests/" + QuestXMLFile))  {
			// restore quest information from previous saved game
			questXMLExists = true;
			QDM.Load (Application.persistentDataPath + "/Quests/" + QuestXMLFile);
			for (int i=0; i<QDM.QD.GI.Count; i++)  {
				if (QDM.QD.GI[i].gType == "kill")  {
					killGoal = new KillGoal (this, QDM.QD.GI[i].gType, QDM.QD.GI[i].gTarget, QDM.QD.GI[i].gDescription, QDM.QD.GI[i].gCompleted, QDM.QD.GI[i].gCurrentAmount, QDM.QD.GI[i].gRequiredAmount);
					Goals.Add (killGoal);
				}
				if (QDM.QD.GI[i].gType == "collect")  {
					collectionGoal = new CollectionGoal (this, QDM.QD.GI[i].gType, QDM.QD.GI[i].gTarget, QDM.QD.GI[i].gDescription, QDM.QD.GI[i].gCompleted, QDM.QD.GI[i].gCurrentAmount, QDM.QD.GI[i].gRequiredAmount);
					Goals.Add (collectionGoal);
				}
			}

		}
		else  {
			KillGoal killGoal = new KillGoal (this, "kill", "rogue", "Kill 10 rogues", false, 0, 10);
			Goals.Add (killGoal);
			CollectionGoal collectionGoal = new CollectionGoal (this, "collect", "bone", "Collect 5 stolen bones", false, 0, 5);
			Goals.Add (collectionGoal);
		}
			
		// go thru each goal in collection and then initialize the method
		Goals.ForEach (g => g.Init ());

		// build an xml containing quest details for use later
		// populate the quest ui
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
