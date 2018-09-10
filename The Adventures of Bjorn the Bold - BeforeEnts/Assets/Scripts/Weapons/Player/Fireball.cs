using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour {

	public Vector3 Direction { get; set; }
	public float Range { get; set; }
	public int Damage { get; set; }
	public GameObject fireballExplosion;

	Vector3 spawnPosition;
	[SerializeField] private AudioSource audioSource;
	[SerializeField] private AudioClip fireballImpact;

	void Start()  {
		spawnPosition = transform.position;
		GetComponent<Rigidbody> ().AddForce (Direction * 50f);
	}

	void Update()  {
		if (Vector3.Distance(spawnPosition, transform.position) >= Range)  {
			Extinguish ();
		}
	}

	void OnCollisionEnter(Collision  col)  {
		if (col.transform.tag == "Enemy") {
			if (GamePreferences.GetFXState() == 1)  {
				audioSource.PlayOneShot (fireballImpact, 1f);
			}
			Vector3 temp = transform.position;
			Instantiate (fireballExplosion, temp, Quaternion.identity);
			col.transform.GetComponent<IEnemy> ().TakeDamage (Damage);
		}
		Extinguish ();
	}

	void Extinguish()  {
		Destroy (gameObject);
	}

}
