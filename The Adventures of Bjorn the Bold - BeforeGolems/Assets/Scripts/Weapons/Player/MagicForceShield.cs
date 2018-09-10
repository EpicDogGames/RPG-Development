using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicForceShield : MonoBehaviour, IWeapon {

	public List<BaseStat> Stats { get; set; }
	public CharacterStats CharacterStats { get; set; }

	private Vector3 playerPos;
	private Animator enemyAnimator;

	[SerializeField] private GameObject shieldContactEffect;

	public void PerformAttack()  {
		playerPos = transform.position;
	}

	void OnTriggerEnter(Collider col)  {
		if (col.tag == "Enemy") {
			// get player's current position for distance to knock back
			playerPos = transform.position;
			// get enemy's animator controller to stop being hit action
			enemyAnimator = col.gameObject.GetComponent<Animator> ();
			// randomize the amount of damage to be done
			int hitDamage = Random.Range (5, 25);
			Debug.Log ("Hit Damage: " + hitDamage);
			col.GetComponent<IEnemy> ().TakeDamage (hitDamage);
			// determine which animation event is going on and turn it off
			if (col.name == "Goblin_NonPatrolling" || col.name == "Goblin_Patrolling" || col.name == "Goblin_Wandering") {
				enemyAnimator.SetBool ("BeenHit", false);
			}
			if (col.name == "Spider_AttackFlee") {
				enemyAnimator.SetBool ("Struck", false);
			}
			if (col.name == "Ent_NonPatrolling") {
				enemyAnimator.SetBool ("Been Hit", false);
			}
			// hit effect
			Vector3 temp = new Vector3 (col.transform.position.x, transform.position.y+1f, transform.position.z-0.35f);
			Instantiate (shieldContactEffect, temp, Quaternion.identity);
			// randomize the knockback distance of the enemy
			float distanceDividedBy = Random.Range (1, 4);
			Debug.Log ("Distance: " + distanceDividedBy);
			col.gameObject.transform.Translate ((-playerPos / distanceDividedBy)  * Time.deltaTime);
		}
	}
}
