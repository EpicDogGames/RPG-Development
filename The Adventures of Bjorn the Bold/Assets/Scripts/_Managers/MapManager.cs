using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour {

	public static bool MapIsDisplayed = false;
	public GameObject mapUI;
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.M))  {
			if (MapIsDisplayed)  {
				DisplayMapOff ();
			}
			else  {
				DisplayMapOn ();
			}
		}
	}

	void DisplayMapOn()  {
		Debug.Log("Pressed M to show map");
		mapUI.SetActive (true);
		Time.timeScale = 0f;
		MapIsDisplayed = true;
	}

	void DisplayMapOff()  {
		Debug.Log ("Pressed M to turn off map");
		mapUI.SetActive (false);
		Time.timeScale = 1f;
		MapIsDisplayed = false;
	}
}
