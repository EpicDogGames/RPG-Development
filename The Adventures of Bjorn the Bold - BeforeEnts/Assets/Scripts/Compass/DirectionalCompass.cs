using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DirectionalCompass : MonoBehaviour {

	public Vector3 NorthDirection;
	public Transform Player;
	public RectTransform NorthArrow;

	// Update is called once per frame
	void Update () {
		ChangeNorthDirection();
	}

	public void ChangeNorthDirection()  {
		NorthDirection.z = Player.eulerAngles.y * -1;
		//Debug.Log ("North Direction: " + NorthDirection);
		NorthArrow.localEulerAngles = NorthDirection;
	}
}
