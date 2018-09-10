using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class InventoryUIManager : MonoBehaviour {

	public RectTransform inventoryPanel;
	public RectTransform scrollViewContent;
	public RectTransform characterPanel_playerDetails;

	InventoryUIItem itemContainer { get; set; }
	bool menuIsActive { get; set; }
	Item currentSelectedItem { get; set; }

	// Use this for initialization
	void Start () {
		itemContainer = Resources.Load<InventoryUIItem>("UI/Item_Container");
		Debug.Log ("Item Container: " + itemContainer);
		UIEventController.OnItemAddedToInventory += ItemAdded;
		UIEventController.OnItemRemovedFromInventory += ItemRemoved;
		inventoryPanel.gameObject.SetActive (false);
		characterPanel_playerDetails.gameObject.SetActive (false);
	}

	void Update()  {
		if (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Tab))  {
			menuIsActive = !menuIsActive;
			inventoryPanel.gameObject.SetActive (menuIsActive);
			characterPanel_playerDetails.gameObject.SetActive (menuIsActive);
		}
	}

	public void ItemAdded(Item item)  {
		SortPlayerItems ();
		RefreshInventoryUI ();
	}

	public void ItemRemoved(Item item)  {
		SortPlayerItems ();
		RefreshInventoryUI ();
	}

	void SortPlayerItems()  {
		InventoryController.Instance.playerItems = InventoryController.Instance.playerItems.OrderByDescending (w => w.ItemType).ThenBy (w => w.ItemName).ToList ();
	}

	void RefreshInventoryUI()  {
		// get rid of all the current itemContainers
		foreach (Transform child in scrollViewContent.transform)  {
			GameObject.Destroy (child.gameObject);
		}
		// regenerate the itemContainers
		int itemCount = 1;
		int lineCount = 0;
		if (InventoryController.Instance.playerItems.Count == 1)  {
			InventoryUIItem emptyItem = Instantiate (itemContainer);
			emptyItem.SetItem (InventoryController.Instance.playerItems [0], 1);
			emptyItem.transform.SetParent (scrollViewContent);
		}
		else {
			for (int i=0; i<InventoryController.Instance.playerItems.Count; i++)  {
				if (lineCount <= InventoryController.Instance.playerItems.Count-2)  {
					string previousName = InventoryController.Instance.playerItems [i].ItemName;
					string currentName = InventoryController.Instance.playerItems [i + 1].ItemName;
					if (previousName == currentName)  {
						itemCount++;
					}
					else  {
						if (itemCount > 1)  {
							InventoryUIItem emptyItem = Instantiate (itemContainer);
							emptyItem.SetItem (InventoryController.Instance.playerItems [i], itemCount);
							emptyItem.transform.SetParent (scrollViewContent);
						}
						else  {
							InventoryUIItem emptyItem = Instantiate (itemContainer);
							emptyItem.SetItem (InventoryController.Instance.playerItems [i], 1);
							emptyItem.transform.SetParent (scrollViewContent);
						}
						itemCount = 1;
					}
					lineCount++;
				}
				else  {
					if (itemCount > 1)  {
						InventoryUIItem emptyItem = Instantiate (itemContainer);
						emptyItem.SetItem (InventoryController.Instance.playerItems [lineCount], itemCount);
						emptyItem.transform.SetParent (scrollViewContent);
					}
					else  {
						InventoryUIItem emptyItem = Instantiate (itemContainer);
						emptyItem.SetItem (InventoryController.Instance.playerItems [lineCount], 1);
						emptyItem.transform.SetParent (scrollViewContent);
					}
				}
			}
		}
	}
}
