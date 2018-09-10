using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Spider_AttackFlee : Interactable, IEnemy {

	// character variables
	public string ID { get; set; }
	public LayerMask aggroLayerMask;
	private Collider[] withinAggroColliders;
	private Vector3 direction;
	private CharacterStats characterStats;

	// attack and flee variables
	//  ... detection
	public LayerMask sightLayer;
	public Transform enemyHead;
	private Spider_AttackFleeEvents enemyAttackFlee;
	private Transform enemyTransform;
	private float checkDetectionRate;
	private float nextDetectionCheck;
	private RaycastHit detectionHit;
	// ... destination reached
	private float checkDestinationReachedRate;
	private float nextDestinationReachedCheck;
	// ... attacking player
	private Transform attackTarget;
	private float attackRate = 1;
	private float nextAttack;
	private float attackRange = 1.5f;    // slightly higher than stopping distance setting
	private int attackDamage = 10;

	// nonpatrolling behavior
	private Player player;
	private NavMeshAgent enemyAgent;

	// Loot variables
	DropTable DropTable { get; set; }
	public PickupQuestDropItem pickupQuestDropItem;

	// health status
	public RectTransform healthBarCanvas;
	public int currentHealth;
	public int maxHealth = 100;
	private int lowHealth = 80;
	private Image healthBar;

	// audio settings for attack
	[SerializeField] private AudioSource audioSource;
	[SerializeField] private AudioClip attackSound;

	void OnEnable()  {
		SetInitialReferences ();
		enemyAttackFlee.EventEnemyDie += DisableThis;
		enemyAttackFlee.EventEnemySetNavTarget += SetAttackTarget;
		enemyAttackFlee.EventEnemyDeductHealth += TakeDamage;
		enemyAttackFlee.EventEnemyIncreaseHealth += IncreaseHealth;
	}
		
	void OnDisable()  {
		enemyAttackFlee.EventEnemyDie -= DisableThis;
		enemyAttackFlee.EventEnemySetNavTarget -= SetAttackTarget;
		enemyAttackFlee.EventEnemyDeductHealth -= TakeDamage;
		enemyAttackFlee.EventEnemyIncreaseHealth -= IncreaseHealth;
	}
		
	void SetInitialReferences () {
		// attack and flee settings
		enemyAttackFlee = GetComponent<Spider_AttackFleeEvents> ();
		enemyTransform = transform;
		if (enemyHead == null)  {
			enemyHead = enemyTransform;
		}
		checkDetectionRate = Random.Range (0.8f, 1.2f);

		// other settings
		DropTable = new DropTable ();
		DropTable.loot = new List<LootDrop> {
			new LootDrop ("lostPage", 100)
		};
		ID = "spider";
		enemyAgent = GetComponent<NavMeshAgent> ();
		characterStats = new CharacterStats (20, 20);
		healthBarCanvas.gameObject.SetActive (false);
		healthBar = transform.Find ("EnemyCanvas").Find ("HealthBG").Find ("Health").GetComponent<Image> ();
		currentHealth = maxHealth;
	}

	void Update()  {
		CarryOutDetection();
		if (Time.time > nextDestinationReachedCheck)  {
			nextDestinationReachedCheck = Time.time + checkDestinationReachedRate;
			CheckIfDestinationReached();
		}
		TryToAttack();
		if (Input.GetKeyUp(KeyCode.Period))  {
			int restoreHealth = lowHealth - currentHealth +5;
			enemyAttackFlee.CallEventEnemyIncreaseHealth (restoreHealth);
		}
	}

	// method to detect player by enemy
	void CarryOutDetection()  {
		if (Time.time > nextDetectionCheck) {
			nextDetectionCheck = checkDetectionRate;
			withinAggroColliders = Physics.OverlapSphere (enemyTransform.position, 5f, aggroLayerMask);
			if (withinAggroColliders.Length > 0)  {
				healthBarCanvas.gameObject.SetActive (true);
				foreach(Collider potentialTargetCollider in withinAggroColliders)  {
					if (potentialTargetCollider.CompareTag("Player"))  {
						if (CanPotentialTargetBeSeen(potentialTargetCollider.transform))  {
							break;
						}
					}
				}
			}
			else  {
				healthBarCanvas.gameObject.SetActive (false);
			}
		}
	}

	 // determine whether potential target is the player
	bool CanPotentialTargetBeSeen(Transform potentialTarget)  {
		if (Physics.Linecast(enemyHead.position, potentialTarget.position, out detectionHit, sightLayer))  {
			if (detectionHit.transform == potentialTarget) {
				enemyAttackFlee.CallEventEnemySetNavTarget(potentialTarget);
				return true;
			}
			else  {
				enemyAttackFlee.CallEventEnemyLostTarget();
				return false;
			}
		}
		else  {
			enemyAttackFlee.CallEventEnemyLostTarget();
			return false;
		}
	}

	// method to handle when enemy has reached player destination during chase
	void CheckIfDestinationReached()  {
		if (enemyAttackFlee.isOnRoute)  {
			if (enemyAgent.remainingDistance < enemyAgent.stoppingDistance)  {
				enemyAttackFlee.isOnRoute = false;
				enemyAttackFlee.CallEventEnemyReachedNavTarget();
			}
		}
	}
		
	// method to set up attack target (the player)
	void SetAttackTarget(Transform targetTransform)  {
		attackTarget = targetTransform;
		enemyTransform = transform;
	}

	// method to determine if it is ok to strike at player which will call the animation to be played
	void TryToAttack()  {
		if (attackTarget != null)  {
			if (Time.time > nextAttack)  {
				nextAttack = Time.time + attackRate;
				// player close enough to attack
				if (Vector3.Distance (enemyTransform.position, attackTarget.position) <= attackRange) {
					// have enemy face the player before attack
					Vector3 lookAtVector = new Vector3 (attackTarget.position.x, enemyTransform.position.y, attackTarget.position.z);
					enemyTransform.LookAt (lookAtVector);
					enemyAttackFlee.CallEventEnemyAttack ();
					enemyAttackFlee.isOnRoute = false;
				}
			}
		}
	}

	// this is called by hit2 animation to do the actual damage
	public void PerformAttack()  {
		if (attackTarget != null)  {
			if (Vector3.Distance(enemyTransform.position, attackTarget.position) <= attackRange && attackTarget.GetComponent<Player>() != null)  {
				// only causes damage if player is in front of the enemy
				Vector3 toOther = attackTarget.position - enemyTransform.position;
				if (Vector3.Dot (toOther, enemyTransform.forward) > 0.5f) {
					attackTarget.GetComponent<Player> ().TakeDamage (attackDamage);
				}
			}
		}
	}

	// method to based on animation event to add strike sound
	private void Strike()  {
		if (GamePreferences.GetFXState () == 1) {
			audioSource.PlayOneShot (attackSound, 0.25f);
		}
	}

	// method to handle enemy taking damage from player hit
	public void TakeDamage(int amount)  {
		currentHealth -= amount;
		healthBar.fillAmount = (float)currentHealth / (float)maxHealth;
		if (currentHealth <= 0)  {
			currentHealth = 0;
			enemyAttackFlee.CallEventEnemyDie();
			DropLoot ();
			Destroy (gameObject);
		}
		CheckHealthFraction ();
	}

	// when enemy killed, item drops
	void DropLoot()  {
		Item item = DropTable.GetDrop ();
		if (item != null)  {
			PickupQuestDropItem instance = Instantiate (pickupQuestDropItem, transform.position, Quaternion.identity);
			instance.ItemDrop = item;
		}
	}

	// check the enemy's health level and determine when fleeing is in order
	void CheckHealthFraction()  {
		if (currentHealth < lowHealth && currentHealth > 0) {
			enemyAttackFlee.CallEventEnemyHealthLow ();
		} else if (currentHealth > lowHealth) {
			enemyAttackFlee.CallEventEnemyHealthRecovered ();
		}
	}
		
	// method to increase enemy health to change out of flee .. this isn't working by any means other than if you manually trigger it
	void IncreaseHealth (int healthChange)  {
		currentHealth += healthChange;
		if (currentHealth > maxHealth)  {
			currentHealth = maxHealth;
		}
		healthBar.fillAmount = (float)currentHealth / (float)maxHealth;
		CheckHealthFraction ();
	}
		
	void DisableThis()  {
		if (enemyAgent != null)  {
			enemyAgent.enabled = false;
		}
		this.enabled = false;
	}
}
