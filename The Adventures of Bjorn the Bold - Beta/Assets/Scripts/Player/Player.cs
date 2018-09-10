using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.IO;

public class Player : MonoBehaviour {

	public NavMeshAgent playerAgent;
	public Transform respawnLocation;
	public GameObject deathPanel;

	public CharacterStats characterStats;
	public int currentHealth;
	public int maxHealth = 100;
	public int currentManaLevel;
	public int maxManaLevel = 100;
	public GameObject healFX;
	public GameObject manaFX;

	private Quaternion playerRotation;
	private int toughness;
	private float percentToughness;
	[SerializeField] AudioSource audioSource;
	[SerializeField] AudioClip deathFX;

	private PlayerDataManager PDM = null;
	private Player player;

	[SerializeField] GameObject pukiPresent;
	[SerializeField] GameObject guardianOfWell_BeforePuki;
	[SerializeField] GameObject guardianOfWell_AfterPuki;
	[SerializeField] GameObject guardianSearchPuzzle;
	[SerializeField] GameObject guardianOfChurch_BeforePuki;
	[SerializeField] GameObject guardianOfChurch_AfterPuki;
	[SerializeField] GameObject guardianOfChurch_caveMazeTrigger;
	[SerializeField] GameObject caveMazeSearchPuzzle;
	[SerializeField] GameObject hideBellInSteeple;

	void Awake()  {
		characterStats = new CharacterStats (10, 0);
		PDM = new PlayerDataManager ();
		player = GameObject.FindObjectOfType<Player> ();
		// determine if there is a previously player file to be loaded
		if (System.IO.File.Exists(Application.persistentDataPath + "/Player/PlayerData.xml"))  {
			PDM.Load (Application.persistentDataPath + "/Player/PlayerData.xml");
			// restore player location
			Vector3 playerPosition = new Vector3 (PDM.GD.PD.playerTransform.X, PDM.GD.PD.playerTransform.Y, PDM.GD.PD.playerTransform.Z);
			playerAgent.Warp (playerPosition);
			// restore health and mana levels
			currentHealth = PDM.GD.PD.playerCurrentHealth;
			currentManaLevel = PDM.GD.PD.playerCurrentMana;
			// determine whether puki has been added through solving the Guardian Search Puzzle or Cave Maze Search Puzzle
			bool pukiAddedToPlayer = PDM.GD.PD.pukiAdded;
			if (pukiAddedToPlayer) {
				GamePreferences.SetPukiFound (1);
				pukiPresent.SetActive (true);
				bool pukiHasMagicShield = PDM.GD.PD.pukiHasMagicShield;
				if (pukiHasMagicShield) {
					GamePreferences.SetMagicShieldForPuki (1);
					guardianOfWell_BeforePuki.SetActive (false);
					guardianOfWell_AfterPuki.SetActive (true);
					guardianSearchPuzzle.SetActive (false);
				}
				bool pukiHasFirePower = PDM.GD.PD.pukiHasFirePower;
				if (pukiHasFirePower)  {
					GamePreferences.SetFirePowerForPuki (1);
					guardianOfChurch_BeforePuki.SetActive (false);
					guardianOfChurch_AfterPuki.SetActive (true);
					guardianOfChurch_caveMazeTrigger.SetActive (false);
					caveMazeSearchPuzzle.SetActive (false);
					hideBellInSteeple.SetActive (false);
				}
			}
		}
		else  {
			currentHealth = maxHealth;
			currentManaLevel = maxManaLevel;
			GamePreferences.SetFirstCombatEncounter (1);
			GamePreferences.SetPukiFound (0);
			guardianOfWell_BeforePuki.SetActive (true);
			guardianOfWell_AfterPuki.SetActive (false);
			guardianOfChurch_BeforePuki.SetActive (true);
			guardianOfChurch_AfterPuki.SetActive (false);
			hideBellInSteeple.SetActive (true);
		}
		StartCoroutine (addHealthOverTime ());
		UIEventController.PlayerHealthChanged (this.currentHealth, this.maxHealth);
		UIEventController.PlayerManaChanged (this.currentManaLevel, this.maxManaLevel);
	}

	public void TakeDamage(int amount)  {
		// this handles the tutorial popup about combat ... this comes up when player is damaged for this first time but hasn't armed himself before
		if (GamePreferences.GetFirstCombatEncounter() == 1)  {
			GamePreferences.SetFirstCombat (1);
		}
		//Debug.Log ("Amount Before Toughness Applied : " + amount);
		// retrieve the toughness value of the character plus whatever equipped with 
		for (int i=0; i<characterStats.stats.Count; i++)  {
			if (characterStats.stats[i].StatName == "Toughness")  {
				toughness = characterStats.stats[i].GetCalculatedStatValue();
			}
		}
		if (toughness > 0) {
			percentToughness = (float)toughness / 100f;
			amount -= (int)(toughness * percentToughness);
		}
		//Debug.Log ("Amount After Toughness Applied : " + amount);
		// use the toughness value to alter the amount of the damage the enemy does
		currentHealth -= amount;
		if (currentHealth <= 0)  {
			Die ();
		}
		UIEventController.PlayerHealthChanged (this.currentHealth, this.maxHealth);
	}

	public void HealPlayer(int healAmount)  {
		// create the special effect 
		Vector3 temp = transform.position;
		temp.y = temp.y + 2f;
		GameObject healObj = Instantiate (healFX, temp, Quaternion.identity) as GameObject;
		healObj.transform.SetParent (transform);
		// change the health value and update the UI
		currentHealth += healAmount;
		if (currentHealth >= maxHealth) {
			currentHealth = maxHealth;
		}
		UIEventController.PlayerHealthChanged (this.currentHealth, this.maxHealth);
	}

	// used by enemy scripts to determine when player dead 
	//   stops attacking player if he has died
	public int CheckHealth()  {
		return currentHealth;
	}

	public void AddManaToPlayer(int manaAmount)  {
		// create the special effect 
		Vector3 temp = transform.position;
		temp.y = temp.y + 2f;
		GameObject manaObj = Instantiate (manaFX, temp, Quaternion.identity) as GameObject;
		manaObj.transform.SetParent (transform);
		// change the mana value and update the UI
		currentManaLevel += manaAmount;
		if (currentManaLevel >= maxManaLevel)  {
			currentManaLevel = maxManaLevel;
		}
		UIEventController.PlayerManaChanged (this.currentManaLevel, this.maxManaLevel);
	}

	public void RemoveManaFromPlayer(int manaAmount)  {
		// change the mana value and update the UI
		currentManaLevel -= manaAmount;
		if (currentManaLevel <= 0 )  {
			currentManaLevel = 0;
		}
		UIEventController.PlayerManaChanged (this.currentManaLevel, this.maxManaLevel);
	}

	//used by staff script to determine if player can hurl projectiles from staff
	public int CheckManaLevel() {
		return currentManaLevel;
	}

	private void Die()  {
		// if player dies, display death panel and move player back to a respawn point
		// start a coroutine to turn off the death panel display after 3 secs
		if (GamePreferences.GetFXState () == 1) {
			audioSource.PlayOneShot (deathFX, 0.25f);
		}
		deathPanel.SetActive (true);
		playerAgent.Warp (respawnLocation.position);
		StartCoroutine (RespawnPlayerAfterDeath());
	}

	public void SavePlayerData()  {
		Debug.Log ("Writing game data for save");
		File.Create (Application.persistentDataPath + "/Saves/GameSave.xml").Dispose();
		PDM.GD.PD.playerTransform.X = player.transform.position.x;
		PDM.GD.PD.playerTransform.Y = player.transform.position.y;
		PDM.GD.PD.playerTransform.Z = player.transform.position.z;
		PDM.GD.PD.playerTransform.RotX = player.transform.eulerAngles.x;
		PDM.GD.PD.playerTransform.RotY = player.transform.eulerAngles.y;
		PDM.GD.PD.playerTransform.RotZ = player.transform.eulerAngles.z;
		PDM.GD.PD.playerTransform.ScaleX = player.transform.localScale.x;
		PDM.GD.PD.playerTransform.ScaleY = player.transform.localScale.y;
		PDM.GD.PD.playerTransform.ScaleZ = player.transform.localScale.z;
		PDM.GD.PD.playerCurrentHealth = currentHealth;
		PDM.GD.PD.playerCurrentMana = currentManaLevel;
		if (GamePreferences.GetPukiFound() == 1)  {
			PDM.GD.PD.pukiAdded = true;
		}
		else  {
			PDM.GD.PD.pukiAdded = false;
		}
		if (GamePreferences.GetMagicShieldForPuki() == 1)  {
			PDM.GD.PD.pukiHasMagicShield = true;
		}
		else  {
			PDM.GD.PD.pukiHasMagicShield = false;
		}
		if (GamePreferences.GetFirePowerForPuki() == 1)  {
			PDM.GD.PD.pukiHasFirePower = true;
		}
		else  {
			PDM.GD.PD.pukiHasFirePower = false;
		}
		PDM.Save (Application.persistentDataPath + "/Player/PlayerData.xml");
	}

	IEnumerator addHealthOverTime()  {
		// this runs indefinitely
		while(true)  {
			// if health drops below 100, it will be regenerated by 1 health value every 2 seconds
			if (currentHealth > 0 && currentHealth < 100)  {
				currentHealth += 1;
				UIEventController.PlayerHealthChanged (this.currentHealth, this.maxHealth);
				yield return new WaitForSeconds(2);
			}
			// if health is 100, nothing happens
			else  {
				yield return null;
			}
		}
	}

	IEnumerator RespawnPlayerAfterDeath()  {
		yield return new WaitForSeconds (3f);
		currentHealth = maxHealth;
		UIEventController.PlayerHealthChanged (this.currentHealth, this.maxHealth);
		deathPanel.SetActive (false);
	}
}
