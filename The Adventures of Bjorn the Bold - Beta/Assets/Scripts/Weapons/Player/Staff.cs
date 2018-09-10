using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Staff : MonoBehaviour, IWeapon, IProjectileWeapon {

	public List<BaseStat> Stats { get; set; }
	public Transform ProjectileSpawn { get; set; }
	public CharacterStats CharacterStats { get; set; }

	private Animator playerAnimator;
	private int currentManaLevel;
	Fireball fireball;

	[SerializeField] private AudioSource audioSource;
	[SerializeField] private AudioClip fireballSpell;

	void Start()  {
		fireball = Resources.Load<Fireball> ("Weapons/Projectiles/Fireball");
		playerAnimator = GameObject.FindGameObjectWithTag ("Player").GetComponent<Animator> ();
	}

	// attack can only happen when the player is in the standing idle position (prevents sliding)
	public void PerformAttack()  {
		currentManaLevel = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player>().CheckManaLevel();
		if (currentManaLevel > 0) {
			if (playerAnimator.GetCurrentAnimatorStateInfo (0).IsName ("Standing Idle")) {
				playerAnimator.SetTrigger ("Cast_Attack");
				CastProjectile ();
			}
		}
		else  {
			Debug.Log ("Not enough mana ... can't use weapon!");
		}
	}

	public void CastProjectile()  {
		StartCoroutine (throwFireball ());
	}

	IEnumerator throwFireball()  {
		yield return new WaitForSeconds (0.5f);
		if (GamePreferences.GetFXState() == 1)  {
			audioSource.PlayOneShot (fireballSpell, GamePreferences.GetFXVolume());
		}
		Fireball fireballInstance = (Fireball)Instantiate (fireball, ProjectileSpawn.position, ProjectileSpawn.rotation);
		// damage, range, and direction are passed to fireball
		fireballInstance.Direction = ProjectileSpawn.forward;
		fireballInstance.Damage = CharacterStats.GetStat (BaseStat.BaseStatType.Power).GetCalculatedStatValue ();
		fireballInstance.Range = 20f;
		GameObject.FindGameObjectWithTag ("Player").GetComponent<Player>().RemoveManaFromPlayer(10);
	}
}
