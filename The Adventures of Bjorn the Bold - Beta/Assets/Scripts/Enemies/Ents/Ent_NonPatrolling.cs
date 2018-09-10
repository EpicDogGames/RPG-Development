using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Ent_NonPatrolling : Interactable, IEnemy {

	/// <summary>
	///  This particular class is used for the Guardian of the Willows quest since it needs to drop kegs as the loot item
	/// </summary>

	// character variables
	public string ID { get; set; }
	public LayerMask aggroLayerMask;
	private Collider[] withinAggroColliders;
	private Vector3 direction;
	private CharacterStats characterStats;

	// nonpatrolling behavior
	public float aggroDistance;
	private Player player;
	private NavMeshAgent enemyAgent;
	private Animator enemyAnimator;

	// attack variables
	public float attackCooldownTime;
	public float attackCooldownTimeMain = 1f;

	// Loot variables
	public DropTable DropTable { get; set; }
	public PickupQuestDropItem pickupQuestDropItem;

	// health status
	public RectTransform healthBarCanvas;
	public int currentHealth;
	public int maxHealth = 75;
	private Image healthBar;

	// Use this for initialization
	void Start () {
		DropTable = new DropTable ();
		DropTable.loot = new List<LootDrop> {
			new LootDrop ("keg", 100)
		};
		ID = "willow ent";
		enemyAgent = GetComponent<NavMeshAgent> ();
		enemyAnimator = GetComponent<Animator> ();
		attackCooldownTime = attackCooldownTimeMain;
		characterStats = new CharacterStats (20, 20);
		healthBarCanvas.gameObject.SetActive (false);
		healthBar = transform.Find ("EnemyCanvas").Find ("HealthBG").Find ("Health").GetComponent<Image> ();
		currentHealth = maxHealth;
	}

	void FixedUpdate()  {
		withinAggroColliders = Physics.OverlapSphere (transform.position, aggroDistance, aggroLayerMask);
		if (withinAggroColliders.Length > 0)  {
			enemyAgent.isStopped = false;
			healthBarCanvas.gameObject.SetActive (true);
			enemyAnimator.SetBool ("CanMove", true);
			ChaseThePlayer (withinAggroColliders[0].GetComponent<Player>());
		}
		else {
			enemyAgent.isStopped = true;
			healthBarCanvas.gameObject.SetActive (false);
			enemyAnimator.SetBool ("CanMove", false);
			enemyAnimator.SetBool ("BeenHit", false);
		}

		int currentPlayerHealth = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player>().CheckHealth();

		if (currentPlayerHealth <= 0)  {
			// player should be dead ... stop invoke method .. have to set CanMove to true to stop CanStrike
			healthBarCanvas.gameObject.SetActive (false);
			enemyAnimator.SetBool ("CanMove", false);
			enemyAnimator.SetBool ("BeenHit", false);
			enemyAnimator.SetBool ("CanStrike", false);
		}
	}

	void ChaseThePlayer(Player player)  {
		this.player = player;
		enemyAgent.SetDestination(player.transform.position);

		direction = player.transform.position - transform.position;
		direction.y = 0;
		transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (direction), 0.9f * Time.deltaTime);

		if (enemyAgent.remainingDistance < enemyAgent.stoppingDistance) {
			//enemyAnimator.SetBool ("CanStrike", true);
			enemyAnimator.SetBool ("CanMove", false);
			if (attackCooldownTime > 0) {
				enemyAnimator.SetBool ("CanStrike", false);
				attackCooldownTime -= Time.deltaTime;
			} else {
				attackCooldownTime = attackCooldownTimeMain;
				enemyAnimator.SetBool ("CanStrike", true);
				PerformAttack ();
			}
		}
	}

	public void PerformAttack()  {
		//Debug.Log ("Striking");
		enemyAnimator.SetBool ("BeenHit", false);
	}

	public void TakeDamage(int amount)  {
		enemyAnimator.SetBool ("BeenHit", true);
		currentHealth -= amount;
		healthBar.fillAmount = (float)currentHealth / (float)maxHealth;
		if (currentHealth <= 0)  {
			Die ();
		}
	}

	void Die()  {
		DropLoot ();
		CombatEvents.EnemyDied (this);
		Destroy (gameObject);
	}

	void DropLoot()  {
		Item item = DropTable.GetDrop ();
		if (item != null)  {
			Vector3 temp = new Vector3(transform.position.x, transform.position.y+0.35f, transform.position.z);
			PickupQuestDropItem instance = Instantiate (pickupQuestDropItem, temp, Quaternion.identity);
			instance.ItemDrop = item;
		}		
	}
}
