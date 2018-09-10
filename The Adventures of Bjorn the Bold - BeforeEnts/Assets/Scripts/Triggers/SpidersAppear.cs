using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpidersAppear : MonoBehaviour {

	// this trigger is right after entering the graveyard and causes the spiders to appear
	// only accessed once the gates are opened which is set by a trigger on the guardian of the sacred forest (GraveyardGateTrigger)

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
