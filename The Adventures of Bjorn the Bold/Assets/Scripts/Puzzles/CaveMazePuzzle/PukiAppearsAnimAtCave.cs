using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PukiAppearsAnimAtCave : MonoBehaviour {

	[SerializeField] private Transform[] wayPoints;
	[SerializeField] private float moveSpeed = 6f;
	private int waypointIndex = 0;

	private bool pukiAnimEnds;

	// Use this for initialization
	void Start () {
		transform.position = wayPoints [waypointIndex].transform.position;
		pukiAnimEnds = false;
	}

	// Update is called once per frame
	void Update () {
		Move ();
	}

	private void Move()  {
		if (waypointIndex <= wayPoints.Length -1)  {
			transform.position = Vector3.MoveTowards (transform.position, wayPoints [waypointIndex].transform.position, moveSpeed * Time.deltaTime);
			if (transform.position == wayPoints[waypointIndex].transform.position)  {
				waypointIndex += 1;
			}  
		}
	}
}
