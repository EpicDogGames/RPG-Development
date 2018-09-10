using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spider_Pause : MonoBehaviour {

	private Spider_AttackFleeEvents enemyAttackFlee;
	private NavMeshAgent enemyAgent;
	private float damagePauseTime = 1;

	void OnEnable()  {
		SetInitialReferences ();
		enemyAttackFlee.EventEnemyDie += DisableThis;
		enemyAttackFlee.EventEnemyDeductHealth += PauseEnemy;
	}

	void OnDisable()  {
		enemyAttackFlee.EventEnemyDie -= DisableThis;
		enemyAttackFlee.EventEnemyDeductHealth -= PauseEnemy;	
	}

	void SetInitialReferences()  {
		enemyAttackFlee = GetComponent<Spider_AttackFleeEvents> ();
		enemyAgent = GetComponent<NavMeshAgent> ();
	}

	// when enemy gets hit, must show some reaction
	void PauseEnemy(int dummy)  {
		if (enemyAgent != null)  {
			if (enemyAgent.enabled)  {
				enemyAgent.ResetPath ();
				enemyAttackFlee.isNavPaused = true;
				StartCoroutine (RestartEnemyAgent());
			}
		}
	}

	IEnumerator RestartEnemyAgent()  {
		yield return new WaitForSeconds (damagePauseTime);
		enemyAttackFlee.isNavPaused = false;
	}

	void DisableThis()  {
		StopAllCoroutines ();
	}
}
