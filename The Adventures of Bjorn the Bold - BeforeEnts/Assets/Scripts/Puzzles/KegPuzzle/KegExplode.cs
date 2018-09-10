using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KegExplode : MonoBehaviour {

	public GameObject kegExplosion;
	public GameObject[] objectsToDestroy;

	[SerializeField] private AudioSource audioSource;
	[SerializeField] private AudioClip explosionImpact;

	private void OnTriggerEnter(Collider col)  {
		if (col.tag == "Explosion") {
			if (GamePreferences.GetFXState() == 1)  {
				audioSource.PlayOneShot (explosionImpact, 1f);
			}
			Instantiate (kegExplosion, transform.position, Quaternion.identity);
			Destroy (this.gameObject, 1f);
			foreach (GameObject obj in objectsToDestroy) {
				Destroy (obj, 1f);
			}
		}
	}
}
