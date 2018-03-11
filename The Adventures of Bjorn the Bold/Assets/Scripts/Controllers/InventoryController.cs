using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour {

	public static InventoryController Instance { get; set; }
	public PlayerWeaponController playerWeaponController;
	public PlayerShieldController playerShieldController;
	public ConsumableController consumableController;
	public InventoryUIDetails inventoryDetailsPanel;
	public List<Item> playerItems = new List<Item> ();

	private Animator playerAnimator;

	void Start()  {
		Debug.Log ("Starting Inventory Controller");
		if (Instance != null && Instance != this)  {
			Destroy (gameObject);
		}
		else {
			Instance = this;
		}
		playerWeaponController = GetComponent<PlayerWeaponController> ();
		playerShieldController = GetComponent<PlayerShieldController> ();
		consumableController = GetComponent<ConsumableController> ();
		playerAnimator = GameObject.FindGameObjectWithTag ("Player").GetComponent<Animator> ();
		GiveItem ("sword0");
		GiveItem ("weakHealthDraft");
		GiveItem ("weakHealthDraft");
	}
		
	public void GiveItem(string itemSlug)  {
		// calls GetItem method from ItemDatabase script
		Item item = ItemDatabase.Instance.GetItem (itemSlug);
		playerItems.Add(item);
		UIEventController.ItemAddedToInventory (item);
	}

	public void GiveItem(Item item)  {
		playerItems.Add (item);
		UIEventController.ItemAddedToInventory (item);
	}

	public void RemoveItem(string itemSlug)  {
		Item item = ItemDatabase.Instance.GetItem (itemSlug);
		playerItems.Remove (item);
		UIEventController.ItemRemovedFromInventory (item);
	}

	public void RemoveItem(Item item)  {
		playerItems.Remove (item);
		UIEventController.ItemRemovedFromInventory (item);
	}

	public void SetItemDetails(Item item, Button selectedButton)  {
		inventoryDetailsPanel.SetItem (item, selectedButton);
	}

	public void EquipItem(Item itemToEquip)  {
		playerAnimator.SetBool ("isArmed", true);
		playerWeaponController.EquipWeapon (itemToEquip);
	}

	public void ShieldItem(Item itemToShield)  {
		playerAnimator.SetBool ("isShielded", true);
		playerShieldController.EquipShield (itemToShield);
	}

	public void ConsumeItem(Item itemToConsume)  {
		consumableController.ConsumeItem (itemToConsume);
	}
		
}
