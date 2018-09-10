using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class EntNonPatrolling : Interactable, IEnemy {

	// character variables
	public string ID { get; set; }
	public Player player;
	private Animator enemyAnimator;
	private NavMeshAgent enemyAgent;
	private CharacterStats characterStats;

	// attack variables
	[SerializeField] private float followDistance;
	[SerializeField] private float attackDistance;
	[SerializeField] private float timeBetweenAttacks;
	private bool playerInRange;

	// Loot variables
	public DropTable DropTable { get; set; }
	public PickupLootItem pickupLootItem;

	// health status
	public int currentHealth;
	public int maxHealth = 100;
	public RectTransform healthBarCanvas;
	private Image healthBar;

	// Use this for initialization
	void Start () {
		DropTable = new DropTable ();
		DropTable.loot = new List<LootDrop> {
			new LootDrop("healthElixir", 40),
			new LootDrop("manaElixir", 20)
		};
		ID = "ent_nonpatrolling";
		enemyAnimator = GetComponent<Animator> ();
		enemyAgent = GetComponent<NavMeshAgent> ();
		characterStats = new CharacterStats (6, 10);
		healthBarCanvas.gameObject.SetActive (false);
		healthBar = transform.Find("EnemyCanvas").Find("HealthBG").Find("Health").GetComponent<Image>();
		currentHealth = maxHealth;
		LootTextController.Initialize ();
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
			int selectAttack = Random.Range (1, 3);
			Debug.Log ("Attack: " + selectAttack);
			switch (selectAttack)  {
				case 1:
					enemyAnimator.Play ("attack1");
					break;
				case 2:
					enemyAnimator.Play ("attack2");
					break;
				default:
					break;
			} 
			timeBetweenAttacks = Random.Range (1f, 3f);
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
		// have to have this method because all enemies have to have this method
		// not going to do anything because log script (weapon used), takes care of this
	}

	public void TakeDamage(int amount)  {
		enemyAnimator.Play ("hit2");
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
