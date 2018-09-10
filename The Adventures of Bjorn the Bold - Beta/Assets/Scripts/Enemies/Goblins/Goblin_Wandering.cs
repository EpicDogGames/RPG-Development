using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Goblin_Wandering : Interactable, IEnemy {

	// character variables
	public string ID { get; set; }
	public GameObject Target;
	public Transform enemyHead;
	private Animator enemyAnimator;
	private CharacterStats characterStats;
	private NavMeshAgent enemyAgent;

	// wander variables
	public GameObject originalPoint;  // this gameobject must be placed in the game and then wired to player
	public bool returnToOriginalPoint;
	public float distanceFromOrigin;
	float wanderSpeed = 0.025f;
	float chaseSpeed = 0.05f;
	public float wanderTime;

	// attack variables
	public float attackCooldownTime;
	public float attackCooldownTimeMain = 2;

	// loot variables
	DropTable DropTable { get; set; }
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
		ID = "goblin_wandering";
		attackCooldownTime = attackCooldownTimeMain;
		enemyAgent = GetComponent<NavMeshAgent> ();
		enemyAnimator = GetComponent<Animator> ();
		characterStats = new CharacterStats (6, 10);
		healthBarCanvas.gameObject.SetActive (false);
		healthBar = transform.Find("EnemyCanvas").Find("HealthBG").Find("Health").GetComponent<Image>();
		currentHealth = maxHealth;	
		LootTextController.Initialize ();
	}
	
	// Update is called once per frame
	void Update () {
		// check to determine if enemy has gone beyond wander range
		float distanceFromOriginalPoint = Vector3.Distance (originalPoint.transform.position, this.transform.position);
		if (distanceFromOriginalPoint > 50 && !returnToOriginalPoint)  {
			Target = null;
			returnToOriginalPoint = true;
		}
		// handle wander behavior
		if (Target == null)  {
			// set up animation states
			enemyAnimator.SetBool ("CanWalk", true);
			enemyAnimator.SetBool ("CanRun", false);
			enemyAnimator.SetBool ("CanStrike", false);
			// set enemy canvas to be off
			healthBarCanvas.gameObject.SetActive (false);
			if (returnToOriginalPoint)  {
				ReturnToOrigin();
			}
			else  {
				// search for target ... in this case, player
				SearchForTarget ();
				if (wanderTime > 0) {
					transform.Translate (Vector3.forward * wanderSpeed);
					wanderTime -= Time.deltaTime;
				} else {
					wanderTime = Random.Range (5f, 15f);
					Wander ();
				}
			}
		}
		else  {
			FollowTarget ();
		}
	}

	// method to handle wander behavior by switching the direction of enemy movement
	void Wander()  {
		transform.eulerAngles = new Vector3 (0, Random.Range (0, 360), 0);
	}

	// method to handle looking for target ... in this case, the player
	void SearchForTarget()  {
		Vector3 center = new Vector3 (this.transform.position.x, this.transform.position.y, this.transform.position.z);
		Collider[] hitColliders = Physics.OverlapSphere (center, 10);
		int i = 0;
		while (i < hitColliders.Length)  {
			if (hitColliders[i].transform.tag == "Player")  {
				Target = hitColliders [i].transform.gameObject;
			}
			i++;
		}
	}

	// method to handle moving toward player once it has been found
	void FollowTarget()  {
		// face towards target always
		Vector3 targetPosition = Target.transform.position;
		targetPosition.y = transform.position.y;
		transform.LookAt (targetPosition);

		// check to see when enemy should attack vs follow vs wander
		float distance = Vector3.Distance (Target.transform.position, this.transform.position);
		if (distance > 15)  {
			Target = null;
			healthBarCanvas.gameObject.SetActive (false);
			enemyAnimator.SetBool ("CanWalk", true);
			enemyAnimator.SetBool ("CanRun", false);
			enemyAnimator.SetBool ("CanStrike", false);
			enemyAnimator.SetBool ("BeenHit", false);
		} else if (distance <= 15 && distance > 2)  {
			healthBarCanvas.gameObject.SetActive (true);
			enemyAnimator.SetBool("CanRun", true);
			enemyAnimator.SetBool("CanWalk", false);
			enemyAnimator.SetBool("CanStrike", false);
			enemyAnimator.SetBool ("BeenHit", false);
			transform.Translate (Vector3.forward * chaseSpeed);
		} else  {
			enemyAnimator.SetBool ("CanStrike", true);
			enemyAnimator.SetBool ("CanRun", false);
			enemyAnimator.SetBool ("CanWalk", false);
			if (attackCooldownTime > 0) {
				attackCooldownTime -= Time.deltaTime;
			} else {
				attackCooldownTime = attackCooldownTimeMain;
				PerformAttack ();
			}
		}
	}

	// method to handle returning enemy to original location after it has wandered too far away
	void ReturnToOrigin()  {
		// just in case, turn off the enemy canvas if it's on
		healthBarCanvas.gameObject.SetActive (false);
		// get the origin point and point enemy in its direction
		Vector3 originalPosition = originalPoint.transform.position;
		originalPosition.y = transform.position.y;
		transform.LookAt (originalPosition);
		// go to that position and when within 10 units, start wandering again
		float distance = Vector3.Distance (originalPoint.transform.position, this.transform.position);
		if (distance > distanceFromOrigin)  {
			transform.Translate (Vector3.forward * wanderSpeed);
		}
		else  {
			returnToOriginalPoint = false;
		}
	}

	public void PerformAttack()  {
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
			PickupLootItem instance = Instantiate (pickupLootItem, transform.position, Quaternion.identity);
			instance.ItemDrop = item;
			LootTextController.CreateLootText (instance.ItemDrop.ItemName, transform);
		}		
	}
}
