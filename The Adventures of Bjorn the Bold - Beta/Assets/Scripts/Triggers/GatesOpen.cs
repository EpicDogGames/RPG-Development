using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatesOpen : MonoBehaviour {

	public Animator gateAnimator;
	public GameObject gateBlocker;

	void OnTriggerEnter(Collider col)  {
		if (col.tag == "Player")  {
			gateAnimator.SetTrigger ("OpenGate");
			Destroy (gameObject);
		}
		Destroy (gateBlocker);
	}
}
