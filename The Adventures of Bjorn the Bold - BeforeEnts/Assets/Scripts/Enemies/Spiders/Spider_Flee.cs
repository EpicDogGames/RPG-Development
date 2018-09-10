using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spider_Flee : MonoBehaviour {

	public bool isFleeing;
	public bool firstFleeReaction;
	public Transform fleeTarget;

	private Spider_AttackFleeEvents enemyAttackFlee;
	private NavMeshAgent enemyAgent;
	private Transform enemyTransform;
	private NavMeshHit navHit;
	private Vector3 runPosition;
	private Vector3 directionToPlayer;
	private float fleeRange = 15;
	public int hasFled;
	private float checkRate;
	private float nextCheck;

	void OnEnable()  {
		SetInitialReferences ();
		enemyAttackFlee.EventEnemyDie += DisableThis;
		enemyAttackFlee.EventEnemySetNavTarget += SetFleeTarget;
		enemyAttackFlee.EventEnemyHealthLow += IShouldFlee;
		enemyAttackFlee.EventEnemyHealthRecovered += IShouldStopFleeing;
	}

	void OnDisable()  {
		enemyAttackFlee.EventEnemyDie -= DisableThis;
		enemyAttackFlee.EventEnemySetNavTarget -= SetFleeTarget;
		enemyAttackFlee.EventEnemyHealthLow -= IShouldFlee;
		enemyAttackFlee.EventEnemyHealthRecovered -= IShouldStopFleeing;		
	}

	void Update()  {
		if (Time.time > nextCheck)  {
			nextCheck = Time.time + checkRate;
			CheckIfIShouldFlee ();
		}
	}

	void SetInitialReferences()  {
		enemyAttackFlee = GetComponent<Spider_AttackFleeEvents> ();
		enemyTransform = transform;
		enemyAgent = GetComponent<NavMeshAgent> ();
		checkRate = Random.Range (0.3f, 0.4f);
		firstFleeReaction = true;
		hasFled = 0;
	}

	void SetFleeTarget(Transform target)  {
		fleeTarget = target;
	}

	void IShouldFlee()  {
		isFleeing = true;
		if (GetComponent<Spider_Pursue>() != null)  {
			GetComponent<Spider_Pursue>().enabled = false;
		}
	}

	void IShouldStopFleeing()  {
		isFleeing = false;
		firstFleeReaction = true;
		if (GetComponent<Spider_Pursue>() != null)  {
			GetComponent<Spider_Pursue>().enabled = true;
		}		
	}

	void CheckIfIShouldFlee()  {
		if (isFleeing)  {
			if (fleeTarget != null && !enemyAttackFlee.isOnRoute && !enemyAttackFlee.isNavPaused)  {
				if (FleeTarget(out runPosition) && Vector3.Distance(enemyTransform.position, fleeTarget.position) < fleeRange)  {
					enemyAgent.SetDestination (runPosition);
					enemyAttackFlee.CallEventEnemyWalking();
					if (firstFleeReaction)  {
						enemyAgent.Warp (runPosition);
						firstFleeReaction = false;
					}
					hasFled++;
					enemyAttackFlee.isOnRoute = true;
				}
			}
		}
		if (hasFled > 5)  {
			IShouldStopFleeing ();
			hasFled = 0;
		}
	}

	bool FleeTarget(out Vector3 result)  {
		directionToPlayer = enemyTransform.position - fleeTarget.position;
		Vector3 checkPosition = enemyTransform.position + directionToPlayer;
		if (NavMesh.SamplePosition(checkPosition, out navHit, 3f, NavMesh.AllAreas))  {
			result = navHit.position;
			return true;
		}
		else  {
			result = enemyTransform.position;
			return false;
		}
	}

	void DisableThis()  {
		this.enabled = false;
	}

}
