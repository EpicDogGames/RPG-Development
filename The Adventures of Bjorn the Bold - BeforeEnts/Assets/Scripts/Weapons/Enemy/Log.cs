using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Log : MonoBehaviour, IWeapon {

	public Player player;
	public List<BaseStat> Stats { get; set; }
	public CharacterStats CharacterStats { get; set; }

	private Animator enemyAnimator;
	[SerializeField] int weaponDamage;

	// sound effects
	[SerializeField] private AudioSource audioSource;
	[SerializeField] private AudioClip entStrike;

	// Use this for initialization
	void Start () {
		enemyAnimator = GameObject.FindGameObjectWithTag ("Enemy").GetComponent<Animator> ();
	}
	
	public void PerformAttack()  {
		enemyAnimator.SetTrigger ("Log_Attack");
	}

	void OnTriggerEnter(Collider col)  {
		if (col.tag == "Player")  {
			Debug.Log ("Have struck player");
			if (GamePreferences.GetFXState() == 1)  {
				audioSource.PlayOneShot (entStrike, 1f);
			}
			player.TakeDamage (weaponDamage);
		}
	}
}
