using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class QuestController : MonoBehaviour {

	public static QuestController Instance { get; set; }

	public List<Item> questItems = new List<Item> ();

	public GameObject quest_guardianOfTheVillage_sacredForestGuardian;
	public GameObject quest_guardianOfTheSacredForest_cursedMist;
	public Animator quest_guardianOfTheSacredForest_gateAnimator;
	public GameObject quest_all_gateBlocker;
	public GameObject quest_guardianOfTheWillows_rarePowderTrigger;
	public GameObject quest_guardianOfTheWillows_entsTrigger;
	public GameObject quest_guardianOfTheWillows_rarePowder;
	public GameObject quest_guardianOfTheWillows_nonpatrollingEnts;
	public GameObject quest_guardianOfTheWillows_nonentTrees;
	public GameObject quest_guardianOfTheWillows_kegsAppearTrigger;
	public GameObject quest_guardianOfTheWillows_kegPuzzleTrigger;

	private DirectoryInfo dirInfo;
	private QuestDataManager QDM = null;

	// Use this for initialization
	void Start () {
		//Debug.Log ("Starting Quest Controller");
		if (Instance != null && Instance != this)  {
			Destroy (gameObject);
		}
		else  {
			Instance = this;
		}
		QDM = new QuestDataManager ();
		LoadOldQuests ();
	}

	void LoadOldQuests()  {
		DirectoryInfo dir = new DirectoryInfo (Application.persistentDataPath + "/Quests");
		FileInfo[] info = dir.GetFiles ("*.xml");
		if (info.Length != 0) {
			foreach (FileInfo f in info) {
				string fName = f.Name;
				int idx = fName.LastIndexOf ('.');
				string questGiver = fName.Substring (0, idx);
				switch (questGiver)
				{
				case "GuardianOfTheGrove":
					Debug.Log ("Guardian of the Grove Quest must be loaded");
					QDM.Load (Application.persistentDataPath + "/Quests/GuardianOfTheGrove.xml");
					if (QDM.QD.QI.qCompleted) {
						GameObject.Find("Guardian of the Grove").GetComponent<QuestGiver>().AssignedQuestFinished();
					} 
					else {
						GameObject.Find ("Guardian of the Grove").GetComponent<QuestGiver> ().AssignQuest ();
					}
					break;
				case "GuardianOfTheVillage":
					Debug.Log("Guardian of the Village Quest must be loaded");
					QDM.Load (Application.persistentDataPath + "/Quests/GuardianOfTheVillage.xml");
					if (QDM.QD.QI.qCompleted)  {
						quest_guardianOfTheVillage_sacredForestGuardian.gameObject.SetActive (true);
						GameObject.Find ("Guardian of the Village").GetComponent<QuestGiver> ().AssignedQuestFinished ();
					}
					else  {
						quest_guardianOfTheVillage_sacredForestGuardian.gameObject.SetActive (false);
						GameObject.Find("Guardian of the Village").GetComponent<QuestGiver>().AssignQuest();
					}
					break;
				case "GuardianOfTheSacredForest":
					Debug.Log("Guardian of the Sacred Forest Quest must be loaded");
					quest_guardianOfTheVillage_sacredForestGuardian.gameObject.SetActive (true);
					quest_guardianOfTheSacredForest_gateAnimator.SetTrigger ("OpenGate");
					Destroy (quest_all_gateBlocker);
					QDM.Load (Application.persistentDataPath + "/Quests/GuardianOfTheSacredForest.xml");
					if (QDM.QD.QI.qCompleted)  {
						quest_guardianOfTheSacredForest_cursedMist.gameObject.SetActive (false);
						GameObject.Find ("Guardian of the Sacred Forest").GetComponent<QuestGiver> ().AssignedQuestFinished ();
					}
					else {
						GameObject.Find ("Guardian of the Sacred Forest").GetComponent<QuestGiver> ().AssignQuest ();
					}
					break;
				case "GuardianOfTheWillows":
					Debug.Log ("Guardian of the Willows Quest must be loaded");
					QDM.Load (Application.persistentDataPath + "/Quests/GuardianOfTheWillows.xml");
					if (QDM.QD.QI.qCompleted)  {
						Destroy (quest_guardianOfTheWillows_rarePowderTrigger);
						Destroy (quest_guardianOfTheWillows_entsTrigger);
						Destroy (quest_guardianOfTheWillows_kegsAppearTrigger);
						quest_guardianOfTheWillows_kegPuzzleTrigger.gameObject.SetActive (true);
						quest_guardianOfTheWillows_rarePowder.gameObject.SetActive (false);
						GameObject.Find ("Guardian of the Willows").GetComponent<QuestGiver> ().AssignedQuestFinished ();
					}
					else  {
						bool powderFound = false;
						for (int i=0; i<QDM.QD.GI.Count; i++)  {
							if (QDM.QD.GI[i].gTarget == "rarePowder")  {
								if (QDM.QD.GI[i].gCurrentAmount == 1)  {
									powderFound = true;
								}
							}
						}
						if (powderFound)  {
							Destroy (quest_guardianOfTheWillows_rarePowderTrigger);
							quest_guardianOfTheWillows_nonentTrees.gameObject.SetActive (false);
							quest_guardianOfTheWillows_nonpatrollingEnts.gameObject.SetActive (true);
						}
						quest_guardianOfTheWillows_kegsAppearTrigger.gameObject.SetActive (true);
						quest_guardianOfTheWillows_kegPuzzleTrigger.gameObject.SetActive (false);
						quest_guardianOfTheWillows_rarePowder.gameObject.SetActive (true);
						GameObject.Find("Guardian of the Willows").GetComponent<QuestGiver>().AssignQuest();
					}
					break;
				}
			}
		}
		else  {
			GamePreferences.SetFirstQuestEntry (1);
		}
	}

	// these two methods for when a quest item is picked up and need to be added to the questItems list
	// this will cause an item to evaluated and added to the xml for the quest
	public void GiveQuestItem(string itemSlug)  {
		// calls GetItem method from ItemDatabase script
		Debug.Log("The item " + itemSlug + " has been added to questItems list as a string");
		Item item = ItemDatabase.Instance.GetItem (itemSlug);
		questItems.Add(item);
		UIEventController.ItemAddedToQuest (item);
	}

	public void GiveQuestItem(Item item)  {
		Debug.Log ("The item " + item.ItemName + " has been added to questItems list as an item");
		questItems.Add (item);
		UIEventController.ItemAddedToQuest (item);
	}

	// these two methods for when a quest item needs to be removed from the questItems list
	public void RemoveQuestItem(string itemSlug)  {
		Item item = ItemDatabase.Instance.GetItem (itemSlug);
		questItems.Remove (item);
		UIEventController.ItemRemovedFromQuest (item);
	}

	public void RemoveQuestItem(Item item)  {
		questItems.Remove (item);
		UIEventController.ItemRemovedFromQuest (item);
	}

	// these two methods for when a quest item needs to be added to the questItems list for display purposes in the QuestUIManager
	public void ReloadQuestItem(string itemSlug)  {
		Debug.Log("The item " + itemSlug + " has been added to questItems list as a string but not sent for evaluation");
		Item item = ItemDatabase.Instance.GetItem (itemSlug);
		questItems.Add(item);	
	}

	public void ReloadQuestItem(Item item)  {
		Debug.Log ("The item " + item.ItemName + " has been added to questItems list as an item but not sent for evaluation");
		questItems.Add (item);		
	}

	public void PopulateQuestDataXMLFile(Quest quest)  {
		// clear goal data
		QDM.QD.GI.Clear();
		// add quest goals
		foreach (Goal g in quest.Goals)  {
			// create goals structure
			QuestDataManager.GoalInfo GI = new QuestDataManager.GoalInfo ();
			GI.gType = g.GoalType;
			GI.gTarget = g.TargetID;
			GI.gDescription = g.Description;
			GI.gCurrentAmount = g.CurrentAmount;
			GI.gRequiredAmount = g.RequiredAmount;
			GI.gCompleted = g.GoalCompleted;

			// Add the goal
			QDM.QD.GI.Add (GI);
		}
		// add quest details
		QDM.QD.QI.qName = quest.QuestName;
		QDM.QD.QI.qDescription = quest.QuestDescription;
		QDM.QD.QI.qImageName = quest.QuestImageName;
		QDM.QD.QI.qCompleted = quest.QuestCompleted;
		// save quest details
		QDM.Save (Application.persistentDataPath + "/Quests/" + quest.QuestXMLFile);
	}

}
