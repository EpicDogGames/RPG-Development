using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveMazeAvailable : MonoBehaviour {

	[SerializeField] private GameObject caveMazeTrigger;
	[SerializeField] private GameObject churchBellAudio;

	// Use this for initialization
	void Start () {
		caveMazeTrigger.gameObject.SetActive (false);
		churchBellAudio.gameObject.SetActive (false);
	}
	
	void OnTriggerEnter(Collider col)  {
		if (col.tag == "Player")  {
			caveMazeTrigger.gameObject.SetActive (true);
			churchBellAudio.gameObject.SetActive (true);
			Destroy (gameObject);
		}
	}
}
