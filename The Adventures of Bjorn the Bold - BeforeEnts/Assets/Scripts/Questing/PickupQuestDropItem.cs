using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupQuestDropItem : Interactable {

	// spawned whenever enemy killed
	public Item ItemDrop { get; set; }

	public override void Interact()  {
		QuestController.Instance.GiveQuestItem (ItemDrop);
		Destroy (gameObject);
	}
}
