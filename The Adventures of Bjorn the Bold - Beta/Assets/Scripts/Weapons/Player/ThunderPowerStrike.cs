using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderPowerStrike : MonoBehaviour, IWeapon {

	public List<BaseStat> Stats { get; set; }
	public CharacterStats CharacterStats { get; set; }

	private Vector3 playerPos;
	private Animator enemyAnimator;

	public void PerformAttack()  {
		playerPos = transform.position;
	}

	void OnTriggerEnter(Collider col)  {
		if (col.tag == "Enemy") {
			// get player's current position for distance to knock back
			playerPos = transform.position;
			// get enemy's animator controller to stop being hit action
			enemyAnimator = col.gameObject.GetComponent<Animator> ();
			Debug.Log ("Enemy has hit zone");
			// randomize the amount of damage to be done
			int hitDamage = Random.Range (10, 30);
			Debug.Log ("Hit Damage: " + hitDamage);
			col.GetComponent<IEnemy> ().TakeDamage (hitDamage);
			// knockback distance of the enemy
			col.gameObject.transform.Translate ((-playerPos / 3f)  * Time.deltaTime);
		}
	}

}
