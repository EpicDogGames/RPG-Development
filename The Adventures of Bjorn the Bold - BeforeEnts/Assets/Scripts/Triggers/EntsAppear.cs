using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntsAppear : MonoBehaviour {

	// these two gameobjects handle when the ents and their tree cover changed ... only after the powder of fire trigger hit
	public GameObject waitingEnts;
	public GameObject treesInFrontOfEnts;
	public GameObject smokeExplosion;
	// this trigger near the guardian of the willows and makes sure the quest is completed before the powder of fire puzzle can be done
	public GameObject kegsAppear;				

	// Use this for initialization
	void Start () {
		waitingEnts.gameObject.SetActive (false);
		smokeExplosion.gameObject.SetActive (false);
		treesInFrontOfEnts.gameObject.SetActive (true);
		kegsAppear.gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void OnTriggerEnter (Collider col) {
		if (col.tag == "Player")  {
			kegsAppear.gameObject.SetActive (true);
			StartCoroutine (SwitchToEnts ());
		}
	}

	IEnumerator SwitchToEnts()  {
		smokeExplosion.gameObject.SetActive (true);
		yield return new WaitForSeconds (1f); 
		waitingEnts.gameObject.SetActive (true);
		treesInFrontOfEnts.gameObject.SetActive (false);
		Destroy (gameObject);		
	}
}
