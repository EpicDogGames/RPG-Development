using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour {

	public GameObject[] tutPops;

	void Update()  {
		// 0 is for start of game info, 1 is for inventory info, 2 is for combat info, 3 is for quest info, 4 for map info
		if (GamePreferences.GetStartOfGame() == 1)  {
			tutPops[0].gameObject.SetActive (true);
			if (Input.GetMouseButtonDown(0))  {
				GamePreferences.SetStartOfGame (0);
				tutPops[0].gameObject.SetActive (false);
			}
		}
		if (GamePreferences.GetFirstInventory() == 1)  {
			tutPops [1].gameObject.SetActive (true);
			StartCoroutine (GetRidOfPopup (1));
		}
		if (GamePreferences.GetFirstCombat() == 1)  {
			tutPops [2].gameObject.SetActive (true);
			StartCoroutine (GetRidOfPopup (2));
		}
		if (GamePreferences.GetFirstQuest() == 1)  {
			tutPops [3].gameObject.SetActive (true);
			StartCoroutine (GetRidOfPopup (3));
		}
		if (GamePreferences.GetFirstMap() == 1)  {
			tutPops [4].gameObject.SetActive (true);
			StartCoroutine (GetRidOfPopup (4));
		}
	}

	IEnumerator GetRidOfPopup(int prefValue)  {
		yield return new WaitForSeconds (5);
		if (prefValue == 1) {
			GamePreferences.SetFirstInventoryItem (0);
			GamePreferences.SetFirstInventory (0);
			tutPops [prefValue].gameObject.SetActive (false);
		}
		if (prefValue == 2)  {
			GamePreferences.SetFirstCombatEncounter (0);
			GamePreferences.SetFirstCombat (0);
			tutPops [prefValue].gameObject.SetActive (false);
		}
		if (prefValue == 3)  {
			GamePreferences.SetFirstQuestEntry (0);
			GamePreferences.SetFirstQuest (0);
			tutPops [prefValue].gameObject.SetActive (false);
		}
		if (prefValue == 4)  {
			GamePreferences.SetFirstMapEntry (0);
			GamePreferences.SetFirstMap (0);
			tutPops [prefValue].gameObject.SetActive (false);
		}
	}
}
