using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSpidersOnTrigger : MonoBehaviour {

	public Animator enemyAnimator;

	void Start()  {
		if (enemyAnimator == null) {
			enemyAnimator = GetComponent<Animator> ();
			if (enemyAnimator == null) {
				Debug.LogError ("no animator on script", gameObject);
			}
		}
	}

	void OnTriggerEnter(Collider col)  {
		if (col.tag == "Player") {
			Debug.Log ("Player went through collider");
			enemyAnimator.SetTrigger ("Appear");	
		}
	}
}
