using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Interactable : MonoBehaviour {

	NavMeshAgent playerAgent;
	public GameObject interactionPoint;

	private bool hasInteracted;
	private bool isEnemy;

	// NPCs have an interaction point that player moves to, thus making sure they face the NPC
	// Enemies don't have an interaction point, they use the interactionInfo point determine in WorldInteraction script so they can be anywhere
	public virtual void MoveToInteraction(NavMeshAgent playerAgent)  {
		isEnemy = gameObject.tag == "Enemy";
		hasInteracted = false;
		this.playerAgent = playerAgent;
		if (!isEnemy) {
			playerAgent.stoppingDistance = 1f;
		} else  {
			playerAgent.stoppingDistance = 1.5f;
		}
	}

	void Update()  {
		if (playerAgent != null && !playerAgent.pathPending && !hasInteracted) {
			if (playerAgent.remainingDistance < playerAgent.stoppingDistance) {
				if (!isEnemy) {
					Interact ();
				} else {	
					FaceTheEnemy ();
				}
				hasInteracted = true;
			}
		}
	}

	void FaceTheEnemy()  {
		playerAgent.updateRotation = false;
		Vector3 lookDirection = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
		playerAgent.transform.LookAt (lookDirection);
		playerAgent.updateRotation = true;
	}

	public virtual void Interact()  {
		// this method is meant to be overridden by other interactables
		Debug.Log ("Interacting with base class.");
	}

}

