﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour {

	public GameObject playerHand;
	public GameObject EquippedWeapon { get; set; }

	private Transform spawnProjectile;
	private Item currentlyEquippedWeapon;
	private IWeapon equippedWeapon;
	private CharacterStats characterStats;
	private Animator playerAnimator;

	void Start()  {
		spawnProjectile = transform.Find("ProjectileSpawn");
		characterStats = GetComponent<Player>().characterStats;
		playerAnimator = GetComponent<Animator> ();
	}

	void Update()  {
		// check to make sure player is armed ... if player not armed, won't perform attack 
		if (playerAnimator.GetBool ("isArmed")) {
			//if (Input.GetKeyDown (KeyCode.Q)) {
			if (Input.GetMouseButtonDown(1))  {
				PerformWeaponAttack ();
			}
		}
	}

	public void EquipWeapon(Item itemToEquip)  {
		//Debug.Log ("In the equipped section (Start) - Player is Armed : " + playerAnimator.GetBool("isArmed"));
		// check to see if player already has a weapon in hand; if so, destroy before adding new weapon
		if (EquippedWeapon != null) {
			UnequipWeapon();
		}
		InventoryController.Instance.RemoveItem (itemToEquip);
		EquippedWeapon = (GameObject)Instantiate (Resources.Load<GameObject> ("Weapons/" + itemToEquip.ObjectSlug), playerHand.transform.position, playerHand.transform.rotation);
		equippedWeapon = EquippedWeapon.GetComponent<IWeapon> ();
		// determine if weapon equipped is a projectile type (staff); if so get spawnProjectile location
		if (EquippedWeapon.GetComponent<IProjectileWeapon> () != null) {
			EquippedWeapon.GetComponent<IProjectileWeapon> ().ProjectileSpawn = spawnProjectile;
		}
		equippedWeapon.Stats = itemToEquip.Stats;
		EquippedWeapon.transform.SetParent (playerHand.transform);
		currentlyEquippedWeapon = itemToEquip;
		characterStats.AddStatBonus (itemToEquip.Stats);
		equippedWeapon.CharacterStats = characterStats;
		UIEventController.ItemEquipped (itemToEquip);
		UIEventController.PlayerStatsChanged ();
		//Debug.Log ("In the equipped section (End) - Player is Armed : " + playerAnimator.GetBool ("isArmed"));
	}

	public void UnequipWeapon() {
		//Debug.Log ("In the unequipped section (start) - Player is Armed: " + playerAnimator.GetBool("isArmed"));
		playerAnimator.SetBool ("isArmed", false);
		InventoryController.Instance.GiveItem (currentlyEquippedWeapon.ObjectSlug);
		characterStats.RemoveStatBonus (EquippedWeapon.GetComponent<IWeapon> ().Stats);
		Destroy (playerHand.transform.GetChild (0).gameObject);
		UIEventController.PlayerStatsChanged ();
		//Debug.Log ("In the unequipped section (end) - Player is Armed: " + playerAnimator.GetBool("isArmed"));
	}

	public void PerformWeaponAttack()  {
		equippedWeapon.PerformAttack ();
	}
}