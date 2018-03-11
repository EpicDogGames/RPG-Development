using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class QuestController : MonoBehaviour {

	public static QuestController Instance { get; set; }

	public List<Item> questItems = new List<Item> ();

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
			//Debug.Log (g.TargetID);
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
