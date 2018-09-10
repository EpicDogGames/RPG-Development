using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PukiTriggerAtCave : MonoBehaviour {

	// handles turning on Puki on the player
	//  only will happen after the puki orb hits the trigger
	//  dialogue box about Puki will be open and then closed

	[SerializeField] GameObject pukiOrb;
	[SerializeField] GameObject pukiSpirit;
	[SerializeField] GameObject pukiExplosion;
	[SerializeField] GameObject pukiDialogue;

	void OnTriggerEnter(Collider col)  {
		if (col.tag == "Orb")  {
			pukiOrb.SetActive (false);
			StartCoroutine (TurnOnPuki());
		}
	}

	IEnumerator TurnOnPuki()  {
		pukiExplosion.SetActive (true);
		yield return new WaitForSeconds (1f);
		pukiSpirit.SetActive (true);
		pukiDialogue.SetActive (true);
		yield return new WaitForSeconds (5f);
		pukiDialogue.SetActive (false);
		Destroy (gameObject);
	}
}
