using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class InventoryController : MonoBehaviour {

	public static InventoryController Instance { get; set; }
	public PlayerWeaponController playerWeaponController;
	public PlayerShieldController playerShieldController;
	public ConsumableController consumableController;
	public InventoryUIDetails inventoryDetailsPanel;
	public List<Item> playerItems = new List<Item> ();

	private Animator playerAnimator;
	private InventoryDataManager IDM = null;

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
		IDM = new InventoryDataManager ();
		// determine if there is a pre-existing inventory to add .. if not, then load sword and 2 weak health drafts
		if (System.IO.File.Exists (Application.persistentDataPath + "/Player/Inventory.xml")) {
			Debug.Log ("Have old inventory to load");
			// restore inventory items
			IDM.Load (Application.persistentDataPath + "/Player/Inventory.xml");
			Debug.Log (IDM.InvD.II.Count);
			for (int i = 0; i < IDM.InvD.II.Count; i++) {
				GiveItem (IDM.InvD.II [i].inventoryItem);
			}
		} else {
			GiveItem ("sword0");
			GiveItem ("weakHealthDraft");
			GiveItem ("weakHealthDraft");
			GamePreferences.SetFirstInventoryItem (1);
		}
	}
		
	public void GiveItem(string itemSlug)  {
		// calls GetItem method from ItemDatabase script
		Debug.Log ("Method GiveItem called with string itemSlug");
		Item item = ItemDatabase.Instance.GetItem (itemSlug);
		playerItems.Add(item);
		UIEventController.ItemAddedToInventory (item);
		// this handles calling tutorial popup about inventory
		if (GamePreferences.GetFirstInventoryItem() == 1)  {
			GamePreferences.SetFirstInventory (1);
		}
	}

	public void GiveItem(Item item)  {
		Debug.Log ("Method GiveItem called with Item item");
		playerItems.Add (item);
		UIEventController.ItemAddedToInventory (item);
		if (GamePreferences.GetFirstInventoryItem() == 1)  {
			GamePreferences.SetFirstInventory (1);
		}
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

	public void CreateInventoryDataXMLFile()  {
		// clear inventory data
		IDM.InvD.II.Clear ();
		// create a list of everything in the inventory
		foreach (Item i in playerItems)  {
			// create inventory item structure
			InventoryDataManager.ItemInfo II = new InventoryDataManager.ItemInfo ();
			II.inventoryItem = i.ObjectSlug;
			// add the item
			IDM.InvD.II.Add (II);
		}
		// check to see if player is equipped with a weapon .. if so, add weapon to inventory list
		string weaponInHand = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerWeaponController>().CheckForWeaponInHand();
		if (weaponInHand.Length != 0)  {
			Debug.Log ("Adding " + weaponInHand + " to inventory");
			InventoryDataManager.ItemInfo II = new InventoryDataManager.ItemInfo ();
			II.inventoryItem = weaponInHand;
			IDM.InvD.II.Add (II);
		}
		// check to see if player is equipped with a shield .. if so, add shield to inventory list
		string shieldInHand = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerShieldController>().CheckForShieldInHand();
		if (shieldInHand.Length != 0)  {
			Debug.Log ("Adding " + shieldInHand + " to inventory");
			InventoryDataManager.ItemInfo II = new InventoryDataManager.ItemInfo ();
			II.inventoryItem = shieldInHand;
			IDM.InvD.II.Add (II);
		}
		// save inventory details
		IDM.Save (Application.persistentDataPath + "/Player/Inventory.xml");
	}
}
