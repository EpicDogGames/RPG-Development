using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fangs : MonoBehaviour, IWeapon {

	public Player player;
	public List<BaseStat> Stats { get; set; }
	public CharacterStats CharacterStats { get; set; }

	private Animator enemyAnimator;
	[SerializeField] int weaponDamage;

	// sound effects
	[SerializeField] private AudioSource audioSource;
	[SerializeField] private AudioClip spiderStrike;

	// Use this for initialization
	void Start () {
		enemyAnimator = GameObject.FindGameObjectWithTag ("Enemy").GetComponent<Animator> ();
	}
	
	public void PerformAttack()  {
		enemyAnimator.SetTrigger ("Fang_Attack");
	}

	void OnTriggerEnter(Collider col)  {
		if (col.tag == "Player")  {
			Debug.Log ("Have struck player");
			if (GamePreferences.GetFXState() == 1)  {
				audioSource.PlayOneShot (spiderStrike, GamePreferences.GetFXVolume());
			}
			player.TakeDamage (weaponDamage);
		}
	}
}
