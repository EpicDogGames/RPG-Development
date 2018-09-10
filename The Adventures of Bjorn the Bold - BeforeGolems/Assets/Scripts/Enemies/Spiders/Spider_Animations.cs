using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spider_Animations : MonoBehaviour {

	private Spider_AttackFleeEvents enemyAttackFlee;
	private Animator enemyAnimator;

	void OnEnable()  {
		SetInitialReferences ();
		enemyAttackFlee.EventEnemyDie += DisableAnimator;
		enemyAttackFlee.EventEnemyWalking += SetAnimationToWalk;
		enemyAttackFlee.EventEnemyReachedNavTarget += SetAnimationToIdle;
		enemyAttackFlee.EventEnemyAttack += SetAnimationToAttack;
		enemyAttackFlee.EventEnemyDeductHealth += SetAnimationToStruck;
	}

	void OnDisable()  {
		enemyAttackFlee.EventEnemyDie -= DisableAnimator;
		enemyAttackFlee.EventEnemyWalking -= SetAnimationToWalk;
		enemyAttackFlee.EventEnemyReachedNavTarget -= SetAnimationToIdle;
		enemyAttackFlee.EventEnemyAttack -= SetAnimationToAttack;
		enemyAttackFlee.EventEnemyDeductHealth -= SetAnimationToStruck;	
	}

	void SetInitialReferences()  {
		enemyAttackFlee = GetComponent<Spider_AttackFleeEvents> ();
		enemyAnimator = GetComponent<Animator> ();
	}

	// method to handle switching enemy into walk mode if player has been detected and needs to be chased
	void SetAnimationToWalk()  {
		if (enemyAnimator != null)  {
			if (enemyAnimator.enabled)  {
				enemyAnimator.SetBool ("isPursuing", true);
			}
		}
	}

	// method to handle switching enemy into idle .. either has reached player or has lost player in detection range
	void SetAnimationToIdle()  {
		if (enemyAnimator != null)  {
			if (enemyAnimator.enabled)  {
				enemyAnimator.SetBool ("isPursuing", false);
			}
		}
	}

	// method to handle switching enemy into attack from any state
	void SetAnimationToAttack()  {
		if (enemyAnimator != null)  {
			if (enemyAnimator.enabled)  {
				enemyAnimator.SetTrigger("Attack");
			}
		}	
	}

	void SetAnimationToStruck(int dummy)  {
		if (enemyAnimator != null)  {
			if (enemyAnimator.enabled)  {
				enemyAnimator.SetTrigger ("Struck");
			}
		}
	}

	void DisableAnimator()  {
		if (enemyAnimator != null)  {
			enemyAnimator.enabled = false;
		}
	}
}
