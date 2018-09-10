using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Golem_NonPatrolling : Interactable, IEnemy {

	// character variables
	public string ID { get; set; }
	public Player player;
	private Animator enemyAnimator;
	private NavMeshAgent enemyAgent;
	private CharacterStats characterStats;

	// attack variables
	ThrownStone thrownStone;
	public Transform ThrowingLocation;
	[SerializeField] private float followDistance = 30f;
	[SerializeField] private float attackDistance = 15f;
	[SerializeField] private float timeBetweenAttacks;
	private bool playerInRange;
	private int hitDamage = 15;
	private bool willThrowRock;
	private Vector3 punchDirection;
	[SerializeField] private AudioSource audioSource;
	[SerializeField] private AudioClip stoneThrow;

	// loot variables
	public DropTable DropTable { get; set; }
	public PickupLootItem pickupLootItem;

	// health status
	public int currentHealth;
	public int maxHealth = 75;
	public RectTransform healthBarCanvas;
	private Image healthBar;

	// Use this for initialization
	void Start () {
		DropTable = new DropTable ();
		DropTable.loot = new List<LootDrop> {
			new LootDrop("healthElixir", 40),
			new LootDrop("manaElixir", 20)
		};
		ID = "golem_nonpatrolling";
		enemyAnimator = GetComponent<Animator> ();
		enemyAgent = GetComponent<NavMeshAgent> ();
		characterStats = new CharacterStats (6, 10);
		healthBarCanvas.gameObject.SetActive (false);
		healthBar = transform.Find("EnemyCanvas").Find("HealthBG").Find("Health").GetComponent<Image>();
		currentHealth = maxHealth;
		LootTextController.Initialize ();
		thrownStone = Resources.Load<ThrownStone> ("Weapons/Projectiles/ThrownStone");
		StartCoroutine (AttackThePlayer ());
	}
	
	// Update is called once per frame
	void Update () {
		// determine distance between the enemy and the player
		float distanceToPlayer = Vector3.Distance (player.transform.position, this.transform.position);
		// determine if player within follow range
		if (distanceToPlayer < followDistance && distanceToPlayer >= attackDistance)  {
			enemyAgent.SetDestination (player.transform.position);
			healthBarCanvas.gameObject.SetActive (true);
			enemyAnimator.SetBool ("FollowThePlayer", true);
			enemyAnimator.SetBool ("PlayerInRange", false);
			playerInRange = false;
		}
		else if (distanceToPlayer < followDistance && distanceToPlayer <= attackDistance)  {
			enemyAgent.SetDestination (player.transform.position);
			enemyAnimator.SetBool ("FollowThePlayer", false);
			enemyAnimator.SetBool ("PlayerInRange", true);
			playerInRange = true;
			FaceThePlayer(player.transform);
		}
		else  {
			enemyAgent.SetDestination (transform.position);
			healthBarCanvas.gameObject.SetActive (false);
			enemyAnimator.SetBool ("FollowThePlayer", false);
			enemyAnimator.SetBool ("PlayerInRange", false);
			playerInRange = false;
		}
	}

	IEnumerator AttackThePlayer()  {
		// switch to hitting vs throwing if too close to player
		if (playerInRange)  {
			float selectTheAttack = Vector3.Distance (player.transform.position, this.transform.position);
			willThrowRock = false;
			if (selectTheAttack > 3f) { 
				willThrowRock = true;
				enemyAnimator.Play ("cast2");
			}
			else  {
				enemyAnimator.Play ("attack1");
			}
			timeBetweenAttacks = Random.Range (1f, 4f);
			yield return new WaitForSeconds (timeBetweenAttacks);
		}
		yield return null;
		StartCoroutine (AttackThePlayer ());
	}

	private void FaceThePlayer(Transform player)  {
		Vector3 direction = (player.position - transform.position).normalized;
		Quaternion lookRotation = Quaternion.LookRotation (direction);
		transform.rotation = Quaternion.Slerp (transform.rotation, lookRotation, Time.deltaTime * 10f);
	}

	public void PerformAttack()  {
		if (willThrowRock) {
			Debug.Log ("Hit with stone");
			if (GamePreferences.GetFXState() == 1)  {
				audioSource.PlayOneShot (stoneThrow, 0.5f);
			}
			ThrownStone thrownStoneInstance = (ThrownStone)Instantiate (thrownStone, ThrowingLocation.position, ThrowingLocation.rotation);
			thrownStoneInstance.Direction = ThrowingLocation.forward;
			thrownStoneInstance.Damage = hitDamage;
			float rangeToPlayer = Random.Range (15f, 30f);
			thrownStoneInstance.Range = rangeToPlayer;
		}
		else  {
			Debug.Log ("Hit with punch");
			punchDirection = player.transform.position - transform.position;
			punchDirection.y = 0;
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (punchDirection), 0.9f * Time.deltaTime);
		}
	}

	public void TakeDamage(int amount)  {
		enemyAnimator.Play ("hit1");
		currentHealth -= amount;
		healthBar.fillAmount = (float)currentHealth / (float)maxHealth;
		if (currentHealth <= 0) {
			Die ();
		}
	}

	public void Die()  {
		DropLoot ();
		CombatEvents.EnemyDied (this);
		Destroy (gameObject);
	}

	void DropLoot()  {
		Item item = DropTable.GetDrop ();
		if (item != null)  {
			PickupLootItem instance = Instantiate (pickupLootItem, transform.position, Quaternion.identity);
			instance.ItemDrop = item;
			LootTextController.CreateLootText (instance.ItemDrop.ItemName, transform);
		}
	}
}
