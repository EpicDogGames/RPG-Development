using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionGoal : Goal {

	public CollectionGoal(Quest quest, string goalType, string itemID, string description, bool goalCompleted, int currentAmount, int requiredAmount)  {
		this.Quest = quest;
		this.GoalType = goalType;
		this.TargetID = itemID;
		this.Description = description;
		this.GoalCompleted = goalCompleted;
		this.CurrentAmount = currentAmount;
		this.RequiredAmount = requiredAmount;
	}

	public override void Init()  {
		base.Init ();
		// set an event listener for whenever a quest item is picked up
		UIEventController.OnItemAddedToQuest += QuestItemPickedUp;
	}

	void QuestItemPickedUp(Item item)  {
		if (item.ObjectSlug == this.TargetID)  {
			Debug.Log ("checking quest pickup item");
			this.CurrentAmount++;
			Evaluate ();
		}
	}
}
