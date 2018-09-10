using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GuardianOfTheVillageQuest : Quest {

	private QuestDataManager QDM = null;
	private bool questXMLExists;
	private CollectionGoal collectionGoal1, collectionGoal2, collectionGoal3, collectionGoal4;

	// Use this for initialization
	void Start () {

		QDM = new QuestDataManager ();
		questXMLExists = false;

		// define xml containing quest details
		QuestXMLFile = "GuardianOfTheVillage.xml";

		// setting up the quest
		QuestName = "Guardian of the Village";
		QuestDescription = "The village has been cursed due to the villagers cutting down trees from a sacred forest. The guardian is asking for your help in lifting the curse by finding books scattered around the village. These books might help to lift the curse.";
		QuestImageName = "GuardianOfTheVillage";
		ItemReward = ItemDatabase.Instance.GetItem ("bagOfHealthAndManaElixirs");

		// new dialogue
		inProgressDialogue = new string[] {
			"Get thee from this place.",
			"Return the books to me post haste."
		};
		rewardDialogue = new string[]  {
			"You have found the books of yore, but there is so much more.",
			"It is to a new guardian you must seek, for he has the power so to speak.",
			"Travel southeast of here and assure the guardian he has nothing to fear.",
			"As a reward for your labors, here are elixirs of various flavors."
		};
		completedDialogue = new string[]  {
			"You have already done your best, now get thee gone and continue your quest." 
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
					if (i == 0) {
						collectionGoal1 = new CollectionGoal (this, QDM.QD.GI [i].gType, QDM.QD.GI [i].gTarget, QDM.QD.GI [i].gDescription, QDM.QD.GI [i].gCompleted, QDM.QD.GI [i].gCurrentAmount, QDM.QD.GI [i].gRequiredAmount);
						Goals.Add (collectionGoal1);
					}
					if (i == 1) {
						collectionGoal2 = new CollectionGoal (this, QDM.QD.GI [i].gType, QDM.QD.GI [i].gTarget, QDM.QD.GI [i].gDescription, QDM.QD.GI [i].gCompleted, QDM.QD.GI [i].gCurrentAmount, QDM.QD.GI [i].gRequiredAmount);
						Goals.Add (collectionGoal2);
					}
					if (i == 2) {
						collectionGoal3 = new CollectionGoal (this, QDM.QD.GI [i].gType, QDM.QD.GI [i].gTarget, QDM.QD.GI [i].gDescription, QDM.QD.GI [i].gCompleted, QDM.QD.GI [i].gCurrentAmount, QDM.QD.GI [i].gRequiredAmount);
						Goals.Add (collectionGoal3);
					}
					if (i == 3) {
						collectionGoal4 = new CollectionGoal (this, QDM.QD.GI [i].gType, QDM.QD.GI [i].gTarget, QDM.QD.GI [i].gDescription, QDM.QD.GI [i].gCompleted, QDM.QD.GI [i].gCurrentAmount, QDM.QD.GI [i].gRequiredAmount);
						Goals.Add (collectionGoal4);
					}
				}
			}
		}
		else  {
			CollectionGoal collectionGoal1 = new CollectionGoal (this, "collect", "sacredBook", "Find the Knowledge of Sacred Forest book", false, 0, 1);
			Goals.Add (collectionGoal1);
			CollectionGoal collectionGoal2 = new CollectionGoal (this, "collect", "legendBook", "Find the Legends of the Sacred Forest book", false, 0, 1);
			Goals.Add (collectionGoal2);
			CollectionGoal collectionGoal3 = new CollectionGoal (this, "collect", "spiritsBook", "Find the Spirits of the Sacred Forest book", false, 0, 1);
			Goals.Add (collectionGoal3);
			CollectionGoal collectionGoal4 = new CollectionGoal (this, "collect", "creaturesBook", "Find the Creatures of the Sacred Forest book", false, 0, 1);
			Goals.Add (collectionGoal4);
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
