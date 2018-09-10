using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestUIItem : MonoBehaviour {

	public Item item;
	public int itemCount;

	public void SetItem(Item item, int itemCount)  {
		this.item = item;
		this.itemCount = itemCount;
		SetupItemValues ();
	}

	//setup Item_Container prefab in Resources/UI with actual item
	void SetupItemValues()  {
		this.transform.Find ("QuestItem_Icon").GetComponent<Image> ().sprite = Resources.Load<Sprite> ("UI/Icons/Items/" + item.ObjectSlug);
		this.transform.Find ("QuestItem_Name").GetComponent<Text> ().text = item.ItemName;
		if (itemCount > 1)  {
			this.transform.Find ("QuestItem_Count").GetComponent<Text> ().text = "x" + itemCount.ToString ();
		}
	}
}
