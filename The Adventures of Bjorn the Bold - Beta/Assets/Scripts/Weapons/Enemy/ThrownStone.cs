using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownStone : MonoBehaviour {

	public Vector3 Direction { get; set; }
	public float Range { get; set; }
	public int Damage { get; set; }
	public GameObject stoneExplosion;

	Vector3 throwingLocation;
	[SerializeField] private AudioSource audioSource;
	[SerializeField] private AudioClip stoneImpact;

	// Use this for initialization
	void Start () {
		throwingLocation = transform.position;
		float speedOfThrow = Random.Range (15f, 50f);
		GetComponent<Rigidbody> ().AddForce (Direction * speedOfThrow);
	}
	
	// Update is called once per frame
	void Update () {
		if (Vector3.Distance(throwingLocation, transform.position) >= Range)  {
			DisintegrateStone ();
		}
	}

	void OnCollisionEnter(Collision col)  {
		if (col.transform.tag == "Player") {
			if (GamePreferences.GetFXState () == 1) {
				audioSource.PlayOneShot (stoneImpact, 0.5f);
			}
			Vector3 temp = transform.position;
			Instantiate (stoneExplosion, temp, Quaternion.identity);
			col.transform.GetComponent<Player> ().TakeDamage (Damage);
		}
		DisintegrateStone ();
	}

	void DisintegrateStone()  {
		Destroy (gameObject);
	}
}
