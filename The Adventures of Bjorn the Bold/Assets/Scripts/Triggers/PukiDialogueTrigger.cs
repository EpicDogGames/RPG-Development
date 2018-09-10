using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PukiDialogueTrigger : MonoBehaviour {

	// this class only called if Puki has been found in a previous "quest" 
	// used to display a different message

	[SerializeField] private GameObject pukiDialogue_Other;

	void OnTriggerEnter(Collider col)  {
		if (col.tag == "Player")  {
			pukiDialogue_Other.SetActive (true);
			StartCoroutine (DisplayPukiDialogue ());
		}
	}

	IEnumerator DisplayPukiDialogue()  {
		yield return new WaitForSeconds (8f);
		pukiDialogue_Other.SetActive (false);
		Destroy (gameObject);
	}
}
