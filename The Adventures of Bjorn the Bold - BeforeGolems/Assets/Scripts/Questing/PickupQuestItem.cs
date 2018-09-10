using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupQuestItem : Interactable {

	public string itemToPickup;

	public override void Interact()  {
		QuestController.Instance.GiveQuestItem (itemToPickup);
		Destroy (gameObject);
	}
}
