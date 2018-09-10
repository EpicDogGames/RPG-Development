using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WorldInteraction : MonoBehaviour {

	private NavMeshAgent playerAgent;
	private Animator animator;
	private GameObject interactedObject;
	private Quaternion playerRotation;

	private int speedId;
	private Vector3 moveToTarget;
	private bool pathReached;

	public enum MoveFSM {
		findPosition,
		move, 
		turnToFace,
		interact
	}

	public MoveFSM moveFSM;

	void Start()  {
		playerAgent = GetComponent<NavMeshAgent> ();
		animator = GetComponent<Animator> ();
		speedId = Animator.StringToHash ("Speed");
		pathReached = false;
	}

	// simple input via mouse button
	void Update()  {
		if (Input.GetMouseButtonDown(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) {
			GetInteraction ();
		}
		MoveStates ();
		CheckForPlayerRotation ();
	}

	public void MoveStates()  {
		switch(moveFSM)  {
		case MoveFSM.findPosition:
			break;
		case MoveFSM.move:
			MoveToEnd ();
			break;
		case MoveFSM.turnToFace:
			TurnToFace ();
			break;
		case MoveFSM.interact:
			break;
		}
	}

	public void MoveToEnd()  {
		if (!playerAgent.pathPending)  {
			if (playerAgent.remainingDistance <= playerAgent.stoppingDistance)  {
				animator.SetFloat (speedId, 0f);
				pathReached = true;
				moveFSM = MoveFSM.turnToFace;
			}
		}	
	}

	public void TurnToFace()  {
		if (interactedObject != null) {
			if (pathReached == true)  {
				Vector3 dir = interactedObject.transform.position - transform.position;
				dir.y = 0;
				playerRotation = Quaternion.LookRotation (dir);
				transform.rotation = Quaternion.Lerp (transform.rotation, playerRotation, 5f * Time.deltaTime);
				if ((playerRotation.eulerAngles - transform.rotation.eulerAngles).sqrMagnitude < 0.01) {
					pathReached = false;
				}
			}	
		}
		else  if (interactedObject == null)  {
			moveFSM = MoveFSM.findPosition;
		}
	}

	// set player interaction based on what the tag says: walk to some point or interact with some object
	void GetInteraction()  {
		DialogueManager.Instance.dialoguePanel.SetActive (false);  // close the dialogue display if player walks away
		Ray interactionRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit interactionInfo;
		if (Physics.Raycast(interactionRay, out interactionInfo, Mathf.Infinity)) {
			interactedObject = interactionInfo.collider.gameObject;
			if (interactedObject.tag == "Enemy")  {
				playerAgent.destination = interactionInfo.point;
				interactedObject.GetComponent<Interactable>().MoveToInteraction (playerAgent);
			}
			else if (interactedObject.tag == "Interactable Object") {
				playerAgent.destination = interactedObject.GetComponent<Interactable>().interactionPoint.transform.position;
				interactedObject.GetComponent<Interactable>().MoveToInteraction(playerAgent);
			}
			else  {
				if (interactedObject != null)  {
					interactedObject = null;
				}
				playerAgent.destination = interactionInfo.point;
				playerAgent.stoppingDistance = 0f;
			}
			pathReached = false;
			animator.SetFloat (speedId, 3f);
			moveFSM = MoveFSM.move;
		}
	}

	// rotate player in place by pushing either spacebar or s key
	void CheckForPlayerRotation()  {
		if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.S))  {
			Vector3 targetPos = Vector3.zero;
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit))  {
				targetPos = new Vector3 (hit.point.x, transform.position.y, hit.point.z);
				transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (targetPos - transform.position), 15f * Time.deltaTime);
			}
		}
	}
}
