using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : Interactable {

	public string itemToPickup;

	public override void Interact()  {
		InventoryController.Instance.GiveItem (itemToPickup);
		Destroy (gameObject);
	}
}
