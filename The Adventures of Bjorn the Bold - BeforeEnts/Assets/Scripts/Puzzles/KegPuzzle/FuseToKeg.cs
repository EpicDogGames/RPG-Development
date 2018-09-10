using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuseToKeg : MonoBehaviour {

	[SerializeField] private Transform[] wayPoints;
	[SerializeField] private float moveSpeed = 2f;
	private int waypointIndex = 0;

	[SerializeField] private AudioSource audioSource;
	[SerializeField] private AudioClip kegFuse;

	// Use this for initialization
	void Start () {
		transform.position = wayPoints [waypointIndex].transform.position;
		if (GamePreferences.GetFXState() == 1)  {
			audioSource.PlayOneShot (kegFuse, 0.5f);
		}
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
