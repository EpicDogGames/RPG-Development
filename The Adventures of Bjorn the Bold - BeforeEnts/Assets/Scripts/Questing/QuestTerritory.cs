using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestTerritory : MonoBehaviour {

	public string questTerritoryName;
	public GameObject questTerritoryUI;

	// Use this for initialization
	void Start () {
		questTerritoryUI.SetActive (false);
	}
	
	void OnTriggerEnter(Collider col)  {
		if (col.tag == "Player")  {
			questTerritoryUI.SetActive (true);
			questTerritoryUI.GetComponent<Text>().text = questTerritoryName;
			//Destroy (gameObject);
		}
	}

	void OnTriggerExit(Collider col)  {
		if (col.tag == "Player")  {
			questTerritoryUI.SetActive (false);
		}
	}
}
