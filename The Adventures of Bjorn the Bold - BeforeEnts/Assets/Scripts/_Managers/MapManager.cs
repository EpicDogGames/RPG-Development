using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour {

	public GameObject mapUI;
	bool menuIsActive { get; set; }

	void Start()  {
		mapUI.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.M))  {
			menuIsActive = !menuIsActive;
			mapUI.SetActive (menuIsActive);
		}
	}

	public void MapButton()  {
		menuIsActive = !menuIsActive;
		mapUI.SetActive (menuIsActive);
	}

	public void CloseMapPanel()  {
		menuIsActive = !menuIsActive;
		mapUI.SetActive (menuIsActive);
	}
}
