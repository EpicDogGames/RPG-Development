using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour, IWeapon {

	public List<BaseStat> Stats { get; set; }
	public CharacterStats CharacterStats { get; set; }

	[SerializeField] private AudioSource audioSource;
	[SerializeField] private AudioClip swordSwing;

	private Animator playerAnimator;
	private bool isAttacking;  // check to make sure player is actually attacking and not just running into an enemy

	void Start()  {
		playerAnimator = GameObject.FindGameObjectWithTag ("Player").GetComponent<Animator> ();
		isAttacking = false;
	}

	// attack can only be performed when the player is in standing idle pose
	public void PerformAttack()  {
		isAttacking = true;
		if (GamePreferences.GetFXState() == 1)  {
			if (playerAnimator.GetCurrentAnimatorStateInfo (0).IsName ("Standing Idle")) {
				audioSource.PlayOneShot (swordSwing, 0.05f);
			}
		}
		if (playerAnimator.GetCurrentAnimatorStateInfo (0).IsName ("Standing Idle")) {
			playerAnimator.SetTrigger ("Sword_Attack");
		}
	}

	void OnTriggerEnter(Collider col)  {
		if (col.tag == "Enemy" && isAttacking)  {
			isAttacking = false;
			col.GetComponent<IEnemy> ().TakeDamage (CharacterStats.GetStat (BaseStat.BaseStatType.Power).GetCalculatedStatValue ());
		}
	}

}
