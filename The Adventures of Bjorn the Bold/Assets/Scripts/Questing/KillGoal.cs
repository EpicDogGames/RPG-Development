using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillGoal : Goal {

	public KillGoal(Quest quest, string goalType, string enemyID, string description,  bool goalCompleted, int currentAmount, int requiredAmount)  {
		this.Quest = quest;
		this.GoalType = goalType;
		this.TargetID = enemyID;
		this.Description = description;
		this.GoalCompleted = goalCompleted;
		this.CurrentAmount = currentAmount;
		this.RequiredAmount = requiredAmount;
	}

	public override void Init()  {
		base.Init ();
		// set an event listener for whenever enemy dies and determine if it matters to the goal
		CombatEvents.OnEnemyDeath += EnemyDied;
	}

	void EnemyDied(IEnemy enemy)  {
		// check enemy ID to determine if it is part of a quest and needs updating a goal
		if (enemy.ID == this.TargetID)  {
			// rogue = goblins in the grove 
			// 1 = spiders in the graveyard (this is not part of the quest though so it won't be used
			if (enemy.ID == "rogue") {
				QuestController.Instance.GiveQuestItem ("rogue");
			}
			Debug.Log("checking killed enemy");
			this.CurrentAmount++;
			Evaluate ();
		}
	}
}
