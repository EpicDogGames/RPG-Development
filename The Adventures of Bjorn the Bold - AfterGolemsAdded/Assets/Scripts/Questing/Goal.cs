using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal {

	public Quest Quest { get; set; }
	public string GoalType { get; set; }
	public string TargetID { get; set; }
	public string Description { get; set; }
	public bool GoalCompleted { get; set; }
	public int CurrentAmount { get; set; }
	public int RequiredAmount { get; set; }

	public virtual void Init()  {
		// default init stuff
	}

	public void Evaluate()  {

		if (CurrentAmount >= RequiredAmount)  {
			CompleteTheGoal ();
		}
		else {
			QuestController.Instance.PopulateQuestDataXMLFile (Quest);
		}
	}

	public void CompleteTheGoal()  {
		GoalCompleted = true;
		QuestController.Instance.PopulateQuestDataXMLFile (Quest);
		Quest.CheckGoals ();
	}
		
}
