using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour {

	public float destroyTimer = 2f;

	void Start()  {
		Destroy (gameObject, destroyTimer);
	}
}
