using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WellAnimates : MonoBehaviour {

	// a trigger (MysticalWellTrigger) that only has the well animating when you are within the path to find the well

	void OnTriggerEnter(Collider col)  {
		if (col.tag == "Player")  {
			Debug.Log ("You have entered the well trigger area");
			LightWellController.Instance.ActivateDeactivateWell ();
		}
	}

	void OnTriggerExit(Collider col)  {
		if (col.tag == "Player")  {
			Debug.Log ("You have exited the well trigger area");
			LightWellController.Instance.ActivateDeactivateWell ();
		}
	}
}
