using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardianAppears : MonoBehaviour {

	public GameObject guardian;

	// Use this for initialization
	void Start () {
		guardian.gameObject.SetActive (false);	
	}
	
	void OnTriggerEnter(Collider col)  {
		if (col.tag == "Player") {
			guardian.gameObject.SetActive (true);
			Destroy (gameObject);
		}
	}
}
