using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

	public float bulletSpeed = 5.0f;	//bullet speed
	private Rigidbody rb;

	void Start () {
		rb = GetComponent<Rigidbody>();
		rb.velocity = Vector3.forward * bulletSpeed;
	}


	public GameObject ImpactEffect;	//hit EffectPrefab

	// The processing when making a hit
	void OnTriggerEnter(Collider col){

		GameObject hitEffPrefab = Instantiate(ImpactEffect, transform.position, Quaternion.identity) as GameObject;
		hitEffPrefab.transform.localScale = new Vector3 (this.transform.localScale.x,this.transform.localScale.y,this.transform.localScale.z);
		Destroy(gameObject);

	}

}

