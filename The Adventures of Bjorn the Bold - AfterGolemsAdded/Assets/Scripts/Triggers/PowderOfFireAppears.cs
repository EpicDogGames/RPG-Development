using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowderOfFireAppears : MonoBehaviour {

	// the trigger is near the guardian of the willows .. once hit, the powder of fire is turned on as well as the ent trigger
	// this is to prevent access to these objects before the quest is assigned
	public GameObject powderOfFire;
	public GameObject entTrigger;

	// Use this for initialization
	void Start () {
		powderOfFire.gameObject.SetActive (false);
		entTrigger.gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void OnTriggerEnter(Collider col) {
		if (col.tag == "Player")  {
			powderOfFire.gameObject.SetActive (true);
			entTrigger.gameObject.SetActive (true);
			Destroy (gameObject);
		}
	}
}
