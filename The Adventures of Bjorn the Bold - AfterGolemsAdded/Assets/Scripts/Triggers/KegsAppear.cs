using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KegsAppear : MonoBehaviour {

	public GameObject kegPuzzleTrigger;

	void Start()  {
		kegPuzzleTrigger.gameObject.SetActive (false);
	}

	void OnTriggerEnter(Collider col)  {
		if (col.tag == "Player")  {
			kegPuzzleTrigger.gameObject.SetActive (true);
			Destroy (gameObject);
		}
	}
}
