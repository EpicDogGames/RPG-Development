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
			// determine which animation event is going on and turn it off ... only the golem doesn't have animation event for being hit so don't have to account for it
			string enemyName = col.name;
			if (enemyName.Contains("Goblin"))  {
				enemyAnimator.SetBool ("BeenHit", false);
			}
			if (enemyName.Contains("Spider"))  {
				enemyAnimator.SetBool ("Struck", false);
			}
			if (enemyName.Contains("Ent"))  {
				enemyAnimator.SetBool ("BeenHit", false);
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
