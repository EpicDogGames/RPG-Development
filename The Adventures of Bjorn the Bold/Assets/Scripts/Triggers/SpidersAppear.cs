using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpidersAppear : MonoBehaviour {

	public Animator [] spiderAnimator;

	void OnTriggerEnter(Collider col)  {
		if (col.tag == "Player")  {
			for (var i = 0; i < spiderAnimator.Length; i++) {
				spiderAnimator [i].SetTrigger ("Appear");
			}
			Destroy (gameObject);
		}
	}
}
