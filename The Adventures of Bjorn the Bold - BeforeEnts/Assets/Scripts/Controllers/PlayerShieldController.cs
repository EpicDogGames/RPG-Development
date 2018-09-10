using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShieldController : MonoBehaviour {

	public GameObject playerHand;
	public GameObject EquippedShield { get; set; }

	private Item currentlyEquippedShield;
	private IShield equippedShield;
	private CharacterStats characterStats;
	private Animator playerAnimator;

	private string shieldInHand;

	void Start()  {
		characterStats = GetComponent<Player> ().characterStats;
		playerAnimator = GetComponent<Animator> ();
		shieldInHand = "";
	}

	public void EquipShield(Item itemToShield)  {
		if (EquippedShield != null)  {
			UnequipShield ();
		}
		InventoryController.Instance.RemoveItem (itemToShield);
		EquippedShield = (GameObject)Instantiate (Resources.Load<GameObject> ("Shields/" + itemToShield.ObjectSlug), playerHand.transform.position, playerHand.transform.rotation);
		equippedShield = EquippedShield.GetComponent<IShield> ();
		equippedShield.Stats = itemToShield.Stats;
		EquippedShield.transform.SetParent (playerHand.transform);
		currentlyEquippedShield = itemToShield;
		shieldInHand = currentlyEquippedShield.ObjectSlug;
		characterStats.AddStatBonus (itemToShield.Stats);
		equippedShield.CharacterStats = characterStats;
		UIEventController.ItemShielded (itemToShield);
		UIEventController.PlayerStatsChanged ();
	}

	public void UnequipShield()  {
		playerAnimator.SetBool ("isShielded", false);
		shieldInHand = "";
		InventoryController.Instance.GiveItem (currentlyEquippedShield.ObjectSlug);
		characterStats.RemoveStatBonus (EquippedShield.GetComponent<IShield> ().Stats);
		Destroy (playerHand.transform.GetChild (0).gameObject);
		UIEventController.PlayerStatsChanged ();
	}

	public void PerformRaiseShield()  {
		equippedShield.RaiseShield ();
	}

	public string CheckForShieldInHand()  {
		return shieldInHand;
	}
}
