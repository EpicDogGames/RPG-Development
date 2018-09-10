using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Spider_NonPatrolling : Interactable, IEnemy {

	// character variables
	public string ID { get; set; }
	public LayerMask aggroLayerMask;
	private Collider[] withinAggroColliders;
	private Vector3 direction;
	private CharacterStats characterStats;

	// nonpatrolling behavior
	private Player player;
	private NavMeshAgent enemyAgent;
	private Animator enemyAnimator;

	// Loot variables
	DropTable DropTable { get; set; }
	public PickupQuestDropItem pickupQuestDropItem;

	// health status
	public RectTransform healthBarCanvas;
	public int currentHealth;
	public int maxHealth = 100;
	private Image healthBar;

	// Use this for initialization
	void Start () {
		DropTable = new DropTable ();
		DropTable.loot = new List<LootDrop> {
			new LootDrop ("lostPage", 80)
		};
		ID = "spider";
		enemyAgent = GetComponent<NavMeshAgent> ();
		enemyAnimator = GetComponent<Animator> ();
		characterStats = new CharacterStats (20, 20);
		healthBarCanvas.gameObject.SetActive (false);
		healthBar = transform.Find ("EnemyCanvas").Find ("HealthBG").Find ("Health").GetComponent<Image> ();
		currentHealth = maxHealth;
		//LootTextController.Initialize ();
	}
	
	void FixedUpdate()  {
		withinAggroColliders = Physics.OverlapSphere (transform.position, 5f, aggroLayerMask);
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
			CancelInvoke ("PerformAttack");
			healthBarCanvas.gameObject.SetActive (false);
			enemyAnimator.SetBool ("CanMove", true);
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
			enemyAnimator.SetBool ("CanMove", false);
			if (!IsInvoking ("PerformAttack"))  {
				InvokeRepeating ("PerformAttack", 0.05f, 2.0f);
			}
		}
		else {
			enemyAnimator.SetBool ("CanStrike", false);
			CancelInvoke ("PerformAttack");
		}		
	}

	public void PerformAttack()  {
		Debug.Log ("Striking");
		enemyAnimator.SetBool ("CanStrike", true);
		direction = player.transform.position - transform.position;
		direction.y = 0;
		transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (direction), 0.9f * Time.deltaTime);
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
			PickupQuestDropItem instance = Instantiate (pickupQuestDropItem, transform.position, Quaternion.identity);
			instance.ItemDrop = item;
			//LootTextController.CreateLootText (instance.ItemDrop.ItemName, transform);
		}		
	}
}
