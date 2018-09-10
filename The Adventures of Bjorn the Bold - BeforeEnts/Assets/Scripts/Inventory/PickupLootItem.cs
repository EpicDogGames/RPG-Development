using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupLootItem : Interactable {

	// spawned whenever enemy killed
	public Item ItemDrop { get; set; }

	public override void Interact()  {
		InventoryController.Instance.GiveItem (ItemDrop);
		Destroy (gameObject);
	}
}
