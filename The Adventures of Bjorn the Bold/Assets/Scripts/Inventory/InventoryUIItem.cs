using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIItem : MonoBehaviour {

	public Item item;
	public int itemCount;

	public void SetItem(Item item, int itemCount)  {
		this.item = item;
		this.itemCount = itemCount;
		SetupItemValues ();
	}

	//setup Item_Container prefab in Resources/UI with actual item
	void SetupItemValues()  {
		this.transform.Find ("Item_Icon").GetComponent<Image> ().sprite = Resources.Load<Sprite> ("UI/Icons/Items/" + item.ObjectSlug);
		this.transform.Find ("Item_Name").GetComponent<Text> ().text = item.ItemName;
		if (itemCount > 1)  {
			this.transform.Find ("Item_Count").GetComponent<Text> ().text = "x" + itemCount.ToString ();
		}
	}

	public void OnSelectItemButton()  {
		InventoryController.Instance.SetItemDetails (item, GetComponent<Button> ());
	}

}
