using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Goblin_Patrolling : Interactable, IEnemy {

	// character variables
	public string ID { get; set; }
	public Player player;
	public Transform enemyHead;
	private Animator enemyAnimator;
	private CharacterStats characterStats;
	private NavMeshAgent enemyAgent;
	
	// patrolling variables
	public GameObject[] waypoints;
	string state = "patrol";
	int currentWP = 0;
	float rotSpeed = 2f;
	float patrolSpeed = 1f;
	float chaseSpeed = 2f;
	float accuracyWP = 1.5f;

	// health status
	public int currentHealth;
	public int maxHealth = 50;
	public RectTransform healthBarCanvas;
	private Image healthBar;

	void Start()  {
		ID = "rogue";
		enemyAgent = GetComponent<NavMeshAgent> ();
		enemyAnimator = GetComponent<Animator> ();
		characterStats = new CharacterStats (6, 10);
		healthBarCanvas.gameObject.SetActive (false);
		healthBar = transform.Find("EnemyCanvas").Find("HealthBG").Find("Health").GetComponent<Image>();
		currentHealth = maxHealth;
	}
		
	void Update()  {
		Vector3 direction = player.transform.position - this.transform.position;
		direction.y = 0;
		float angle = Vector3.Angle (direction, enemyHead.forward);

		// patrolling state, moving between waypoints
		if (state == "patrol" && waypoints.Length > 0) {
			// set animation states
			enemyAnimator.SetBool ("CanWalk", true);
			enemyAnimator.SetBool ("CanRun", false);
			enemyAnimator.SetBool ("CanStrike", false);
			// move between waypoints, randomizing the point to go to
			if (Vector3.Distance (waypoints[currentWP].transform.position, transform.position) < accuracyWP) {
				currentWP = Random.Range (0, waypoints.Length);
			}
			// move and rotate towards waypoint
			enemyAgent.SetDestination (waypoints [currentWP].transform.position);
			direction = waypoints [currentWP].transform.position - transform.position;
			this.transform.rotation = Quaternion.Slerp (this.transform.rotation, Quaternion.LookRotation (direction), rotSpeed * Time.deltaTime);
			this.transform.Translate (0, 0, Time.deltaTime * patrolSpeed);
		}

		// chasing player when 'seen'
		if (Vector3.Distance(player.transform.position, this.transform.position) < 10 && (angle < 360 || state == "pursuing"))  {
			// run toward the player
			healthBarCanvas.gameObject.SetActive (true);
			state = "pursuing";
			enemyAgent.SetDestination (player.transform.position);
			//this.player = player;
			this.transform.rotation = Quaternion.Slerp (this.transform.rotation, Quaternion.LookRotation (direction), rotSpeed * Time.deltaTime);
			// attack the player if within attacking distance
			if (direction.magnitude <= 1.5f)  {
				enemyAnimator.SetBool("CanStrike", true);
				if (!IsInvoking ("PerformAttack")) {
					InvokeRepeating ("PerformAttack", 0.05f, 1.3f);
				}
				enemyAnimator.SetBool("CanRun", false);
				enemyAnimator.SetBool("CanWalk", false);
			}
			else  {
				CancelInvoke ("PerformAttack");
				this.transform.Translate (0, 0, Time.deltaTime * chaseSpeed);
				enemyAnimator.SetBool("CanRun", true);
				enemyAnimator.SetBool("CanWalk", false);
				enemyAnimator.SetBool("CanStrike", false);
				enemyAnimator.SetBool ("BeenHit", false);
			}
		}
		else {
			healthBarCanvas.gameObject.SetActive (false);
			enemyAnimator.SetBool("CanWalk", true);
			enemyAnimator.SetBool("CanRun", false);
			enemyAnimator.SetBool("CanStrike", false);
			enemyAnimator.SetBool ("BeenHit", false);
			state = "patrol";
		}
	}
		
	public void PerformAttack()  {
		enemyAnimator.SetBool ("BeenHit", false);
	}

	public void TakeDamage(int amount)  {
		enemyAnimator.SetBool ("BeenHit", true);
		currentHealth -= amount;
		healthBar.fillAmount = (float)currentHealth / (float)maxHealth;
		if (currentHealth <= 0) {
			Die ();
		}
	}

	public void Die()  {
		CombatEvents.EnemyDied (this);
		Destroy (gameObject);
	}

}
