using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.IO;

public class QuestUIManager : MonoBehaviour {

	public static QuestUIManager Instance { get; set; }
	public RectTransform questPanel;
	public RectTransform questScrollViewContent;
	public Text questName, questDescription, questStatus;
	public RectTransform questsScrollViewContent;
	public Image questImage, questStatusImage;

	QuestUIItem questContainer { get; set; }
	bool menuIsActive { get; set; }

	private QuestDataManager QDM = null;

	// Use this for initialization
	void Start () {
		if (Instance != null && Instance != this)  {
			Destroy (gameObject);
		}
		else  {
			Instance = this;
		}
		QDM = new QuestDataManager ();
		questContainer = Resources.Load<QuestUIItem>("UI/QuestItem_Container");
		UIEventController.OnItemAddedToQuest += QuestItemAdded;
		questPanel.gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Q))  {
			DirectoryInfo dir = new DirectoryInfo (Application.persistentDataPath + "/Quests");
			FileInfo[] info = dir.GetFiles ("*.xml");
			if (info.Length > 1)  {
				// clear the individual quest panel for selection from quests panel 
				questName.text = "";
				questDescription.text = "";
				questStatus.text = "";
				questImage.sprite = Resources.Load<Sprite> ("UI/QuestPanel/Quests/questDefaultImage");
				questStatusImage.GetComponent<Image> ().color = new Color32 (0, 0, 0, 0);
				ResetQuestItemList();
			}
			menuIsActive = !menuIsActive;
			questPanel.gameObject.SetActive (menuIsActive);
		}
	}

	public void ResetQuestItemList()  {
		// remove all the containers in the scrollViewContent and clear the list
		if (QuestController.Instance.questItems.Count != 0)  {
			foreach(Transform child in questScrollViewContent.transform)  {
				GameObject.Destroy (child.gameObject);
			}
			QuestController.Instance.questItems.Clear();
		}
	}

	public void QuestItemAdded(Item item)  {
		SortQuestItems ();
		RefreshQuestUI ();
	}

	void SortQuestItems()  {
		QuestController.Instance.questItems = QuestController.Instance.questItems.OrderBy (w => w.ItemName).ToList ();
	}

	void RefreshQuestUI()  {
		// get rid of all current questItemContainers
		foreach (Transform child in questScrollViewContent.transform)  {
			GameObject.Destroy (child.gameObject);
		}
		// regenerate questItemContainers
		int itemCount = 1;
		int lineCount = 0;
		if (QuestController.Instance.questItems.Count == 1)  {
			QuestUIItem emptyItem = Instantiate (questContainer);
			emptyItem.SetItem (QuestController.Instance.questItems [0], 1);
			emptyItem.transform.SetParent (questScrollViewContent);
		}
		else  {
			for (int i=0; i<QuestController.Instance.questItems.Count; i++)  {
				if (lineCount <= QuestController.Instance.questItems.Count-2)  {
					string previousName = QuestController.Instance.questItems [i].ItemName;
					string currentName = QuestController.Instance.questItems [i + 1].ItemName;
					if (previousName == currentName)  {
						itemCount++;
					}
					else  {
						if (itemCount > 1)  {
							QuestUIItem emptyItem = Instantiate (questContainer);
							emptyItem.SetItem (QuestController.Instance.questItems [i], itemCount);
							emptyItem.transform.SetParent (questScrollViewContent);
						}
						else  {
							QuestUIItem emptyItem = Instantiate (questContainer);
							emptyItem.SetItem (QuestController.Instance.questItems [i], 1);
							emptyItem.transform.SetParent (questScrollViewContent);
						}
						itemCount = 1;
					}
					lineCount++;
				}
				else  {
					if (itemCount > 1)  {
						QuestUIItem emptyItem = Instantiate (questContainer);
						emptyItem.SetItem (QuestController.Instance.questItems [lineCount], itemCount);
						emptyItem.transform.SetParent (questScrollViewContent);
					}
					else  {
						QuestUIItem emptyItem = Instantiate (questContainer);
						emptyItem.SetItem (QuestController.Instance.questItems [lineCount], 1);
						emptyItem.transform.SetParent (questScrollViewContent);
					}
				}
			}
		}
	}

	// if the first quest set, populate the quest panel with details of the specific quest
	// this method is used for a new game
	public void SetupSingleQuest(Quest quest)  {

		Debug.Log ("Setting up single quest");
		string description = quest.QuestDescription;
		description += "\n\n";
		description += "Tasks:\n";
		int i = 1;
		foreach(Goal g in quest.Goals)  {
			description += i + ") " + g.Description + "\n";
			i++;
		}

		questName.text = quest.QuestName;
		questImage.sprite = Resources.Load<Sprite> ("UI/QuestPanel/Quests/" + quest.QuestImageName);
		questDescription.text = description;
		if (quest.QuestCompleted)  {
			questStatusImage.GetComponent<Image> ().color = new Color32 (0, 118, 255, 255);
			questStatus.text = "COMPLETED";
		}
		else  {
			questStatusImage.GetComponent<Image> ().color = new Color32 (215, 38, 9, 255);
			questStatus.text = "IN PROGRESS";
		}
		ResetQuestItemList ();
	}

	// if the first quest set, populate the quest panel with details of the specific quest
	// this method is used for an old saved game
	public void SetupSingleOldQuest(Quest quest, string xmlFile)  {
		// get quest name and description as well as goal descriptions
		if (System.IO.File.Exists(Application.persistentDataPath + "/Quests/" + xmlFile))  {
			// restore quest information
			QDM.Load (Application.persistentDataPath + "/Quests/" + xmlFile);
			// populate the quest panel
			questName.text = QDM.QD.QI.qName;
			questImage.sprite = Resources.Load<Sprite> ("UI/QuestPanel/Quests/" + QDM.QD.QI.qImageName);
			string description = QDM.QD.QI.qDescription;
			description += "\n\n";
			description += "Tasks:\n";
			int j = 1;
			for (int i=0; i<QDM.QD.GI.Count; i++)  {
				description += j + ") " + QDM.QD.GI[i].gDescription + "\n";
				j++;
			}
			questDescription.text = description;
		}
		ResetQuestItemList ();
		// repopulate the questItems
		bool questItemsSaved = false;
		for (int i=0; i<QDM.QD.GI.Count; i++)  {
			if (QDM.QD.GI[i].gCurrentAmount > 0)  {
				questItemsSaved = true;
				for (int j = 0; j < QDM.QD.GI[i].gCurrentAmount; j++) {
					QuestController.Instance.ReloadQuestItem (QDM.QD.GI[i].gTarget);
				}
			}
		}
		if (questItemsSaved)  {
			SortQuestItems ();
			RefreshQuestUI ();
		}
		// show status of the quest
		if (QDM.QD.QI.qCompleted)  {
			questStatusImage.GetComponent<Image> ().color = new Color32 (0, 118, 255, 255);
			questStatus.text = "COMPLETED";
		}
		else  {
			questStatusImage.GetComponent<Image> ().color = new Color32 (215, 38, 9, 255);
			questStatus.text = "IN PROGRESS";
		}
	}

	// populate the quests panel with all quests as they are received
	// these are a series of buttons that can be used to retrieve individual quest information to populate quest panel
	public void BuildQuestButton(Quest quest)  {
		GameObject QuestButton = Instantiate (Resources.Load ("UI/Quest_Container")) as GameObject;
		QuestButton.name = quest.QuestName;
		QuestButton.transform.SetParent (questsScrollViewContent);
		QuestButton.transform.Find ("Quest_Name").GetComponent<Text> ().text = quest.QuestName;
		QuestButton.GetComponent<Button>().onClick.AddListener(() => { 
			ShowQuestDetails(quest.QuestXMLFile);
		});
	}

	public void ShowQuestDetails(string xmlFile)  {
		// get quest name and description as well as goal descriptions
		if (System.IO.File.Exists(Application.persistentDataPath + "/Quests/" + xmlFile))  {
			// restore quest information
			QDM.Load (Application.persistentDataPath + "/Quests/" + xmlFile);
			// populate the quest panel
			questName.text = QDM.QD.QI.qName;
			questImage.sprite = Resources.Load<Sprite> ("UI/QuestPanel/Quests/" + QDM.QD.QI.qImageName);
			string description = QDM.QD.QI.qDescription;
			description += "\n\n";
			description += "Tasks:\n";
			int j = 1;
			for (int i=0; i<QDM.QD.GI.Count; i++)  {
				description += j + ") " + QDM.QD.GI[i].gDescription + "\n";
				j++;
			}
			questDescription.text = description;
		}
		ResetQuestItemList ();
		//Debug.Log("Quest item count: " + QuestController.Instance.questItems.Count);
		// repopulate the questItems
		bool questItemsSaved = false;
		for (int i=0; i<QDM.QD.GI.Count; i++)  {
			if (QDM.QD.GI[i].gCurrentAmount > 0)  {
				questItemsSaved = true;
				for (int j = 0; j < QDM.QD.GI [i].gCurrentAmount; j++) {
					QuestController.Instance.ReloadQuestItem (QDM.QD.GI [i].gTarget);
				}
			}
		}
		if (questItemsSaved)  {
			SortQuestItems ();
			RefreshQuestUI ();
		}
		// show status of the quest
		if (QDM.QD.QI.qCompleted)  {
			questStatusImage.GetComponent<Image> ().color = new Color32 (0, 118, 255, 255);
			questStatus.text = "COMPLETED";
		}
		else  {
			questStatusImage.GetComponent<Image> ().color = new Color32 (215, 38, 9, 255);
			questStatus.text = "IN PROGRESS";
		}
	}

}
