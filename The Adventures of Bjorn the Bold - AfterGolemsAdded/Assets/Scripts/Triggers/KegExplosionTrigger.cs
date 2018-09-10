using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KegExplosionTrigger : MonoBehaviour {

	void OnTriggerExit(Collider col)  {
		if (col.tag == "Player")  {
			StartCoroutine (DestroyTheTrigger ());
		}
	}

	IEnumerator DestroyTheTrigger()  {
		yield return new WaitForSeconds (30f); 
		Destroy (gameObject);		
	}
}
