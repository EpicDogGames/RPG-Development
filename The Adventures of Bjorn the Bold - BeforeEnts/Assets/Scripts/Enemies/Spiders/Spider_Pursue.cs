using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spider_Pursue : MonoBehaviour {

	// attack and flee variables
	private Spider_AttackFleeEvents enemyAttackFlee; 
	private NavMeshAgent enemyAgent;
	private float checkPursueRate;
	private float nextPursueCheck;

	void OnEnable()  {
		SetInitialReferences ();
		enemyAttackFlee.EventEnemyDie += DisableThis;
	}

	void OnDisable()  {
		enemyAttackFlee.EventEnemyDie -= DisableThis;
	}

	void Update()  {
		if (Time.time > nextPursueCheck)  {
			nextPursueCheck = Time.time + checkPursueRate;
			TryToChaseTarget();
		}
	}

	void SetInitialReferences()  {
		enemyAttackFlee = GetComponent<Spider_AttackFleeEvents> ();
		enemyAgent = GetComponent<NavMeshAgent> ();
		checkPursueRate = Random.Range (0.1f, 0.2f);
	}

	void TryToChaseTarget()  {
		if (enemyAttackFlee.enemyTarget != null && enemyAgent != null && !enemyAttackFlee.isNavPaused)  {
			enemyAgent.SetDestination (enemyAttackFlee.enemyTarget.position);
			if (enemyAgent.remainingDistance > enemyAgent.stoppingDistance)  {
				enemyAttackFlee.CallEventEnemyWalking ();
				enemyAttackFlee.isOnRoute = true;
			}
		}
	}

	void DisableThis()  {
		if (enemyAgent != null)  {
			enemyAgent.enabled = false;
		}
		this.enabled = false;
	}
}
