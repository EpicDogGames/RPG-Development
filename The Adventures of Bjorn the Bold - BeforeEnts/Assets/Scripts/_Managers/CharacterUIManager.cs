using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUIManager : MonoBehaviour {

	// Player health and mana
	[SerializeField] private Image healthFill, manaFill;
	[SerializeField] private Player player;

	// Player Details Panel
	[SerializeField] private GameObject playerDetails;

	// Stats
	private List<Text> playerStatTexts = new List<Text> ();
	[SerializeField] private Text playerStatPrefab;
	[SerializeField] private Transform playerStatPanel;

	// Equipped Weapon
	private List<Text> weaponStatTexts = new List<Text> ();
	[SerializeField] private Sprite defaultWeaponSprite;
	[SerializeField] private Image weaponIcon;
	[SerializeField] private Text weaponNameText;
	[SerializeField] private Transform weaponStatPanel;
	[SerializeField] private Text weaponStatPrefab;
	private PlayerWeaponController PlayerWeaponController;
	private Animator playerAnimator;

	// Equipped Shield
	private List<Text> shieldStatTexts = new List<Text>();
	[SerializeField] private Sprite defaultShieldSprite;
	[SerializeField] private Image shieldIcon;
	[SerializeField] private Text shieldNameText;
	[SerializeField] private Transform shieldStatPanel;
	[SerializeField] private Text shieldStatPrefab;
	private PlayerShieldController PlayerShieldController;

	// Use this for initialization
	void Start () {
		PlayerWeaponController = player.GetComponent<PlayerWeaponController> ();
		PlayerShieldController = player.GetComponent<PlayerShieldController> ();
		playerAnimator = player.GetComponent<Animator> ();
		UIEventController.OnPlayerHealthChanged += UpdateHealth;
		UIEventController.OnPlayerManaChanged += UpdateMana;
		UIEventController.OnPlayerStatsChanged += UpdateStats;
		UIEventController.OnItemEquipped += UpdateEquippedWeapon;
		UIEventController.OnItemShielded += UpdateEquippedShield;
		InitializeStats();
		playerDetails.gameObject.SetActive (false);
	}
	
	void UpdateHealth(int currentHealth, int maxHealth)  {
		this.healthFill.fillAmount = (float)currentHealth / (float)maxHealth;
	}

	void UpdateMana(int currentMana, int maxMana)  {
		this.manaFill.fillAmount = (float)currentMana / (float)maxMana;
	}

	void InitializeStats()  {
		for(int i=0; i<player.characterStats.stats.Count; i++)  {
			playerStatTexts.Add (Instantiate (playerStatPrefab));
			playerStatTexts [i].transform.SetParent (playerStatPanel);
		}
		UpdateStats();
	}

	void UpdateStats()  {
		for(int i=0; i<player.characterStats.stats.Count; i++)  {
			playerStatTexts[i].text = player.characterStats.stats[i].StatName + " : " + player.characterStats.stats[i].GetCalculatedStatValue().ToString();
		}
	}

	void UpdateEquippedWeapon(Item item)  {
		//Debug.Log ("When does this get run");
		// setting the bool isArmed is necessary here to account for switching weapons
		playerAnimator.SetBool ("isArmed", true);
		weaponIcon.sprite = Resources.Load<Sprite>("UI/Icons/Items/" + item.ObjectSlug);
		weaponNameText.text = item.ItemName;
		for (int i=0; i<item.Stats.Count; i++) {
			weaponStatTexts.Add (Instantiate (weaponStatPrefab));
			weaponStatTexts [i].transform.SetParent (weaponStatPanel);
			weaponStatTexts [i].text = item.Stats [i].StatName + " : " + item.Stats [i].GetCalculatedStatValue ().ToString ();
		}
	}

	public void UnequipWeapon()  {
		if (playerAnimator.GetBool ("isArmed")) {
			weaponIcon.sprite = defaultWeaponSprite;
			weaponNameText.text = "";
			for (int i=0; i<weaponStatTexts.Count; i++)  {
				Destroy (weaponStatTexts [i].gameObject);
			}
			weaponStatTexts.Clear ();
			PlayerWeaponController.UnequipWeapon ();
		}
	}

	void UpdateEquippedShield(Item item)  {
		// setting the bool isArmed is necessary here to account for switching weapons
		playerAnimator.SetBool ("isShielded", true);
		shieldIcon.sprite = Resources.Load<Sprite> ("UI/Icons/Items/" + item.ObjectSlug);
		shieldNameText.text = item.ItemName;
		for (int i=0; i<item.Stats.Count; i++)  {
			shieldStatTexts.Add (Instantiate (shieldStatPrefab));
			shieldStatTexts [i].transform.SetParent (shieldStatPanel);
			shieldStatTexts [i].text = item.Stats [i].StatName + " : " + item.Stats [i].GetCalculatedStatValue ().ToString ();
		}
	}

	public void UnequipShield()  {
		if (playerAnimator.GetBool ("isShielded")) {
			shieldIcon.sprite = defaultShieldSprite;
			shieldNameText.text = "";
			for (int i = 0; i < shieldStatTexts.Count; i++) {
				Destroy (shieldStatTexts [i].gameObject);
			}
			shieldStatTexts.Clear ();
			PlayerShieldController.UnequipShield ();
		}
	}
}
