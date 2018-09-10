using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RunePuzzleController : MonoBehaviour, IHasChanged {

	public GameObject runePuzzlePanel;

	[SerializeField] Transform runeSlots;
	[SerializeField] GameObject[] runeTile;
	[SerializeField] GameObject runeBoardPanel;
	[SerializeField] GameObject puzzleTitle;
	[SerializeField] GameObject puzzleWonText;
	[SerializeField] GameObject puzzleLostText;
	[SerializeField] GameObject[] runeSlot;
	private string runesString;
	private string runesSolutionString = " - H - C - F - J - A - I - B - D - G - E - ";

	// Use this for initialization
	void Start () {
		HasChanged ();
	}
	
	public void HasChanged()  {
		if (!puzzleTitle.activeInHierarchy)  {
			puzzleTitle.SetActive (true);
			puzzleWonText.SetActive (false);
			puzzleLostText.SetActive (false);
		}
		System.Text.StringBuilder runeStringBuilder = new System.Text.StringBuilder ();
		runeStringBuilder.Append (" - ");
		foreach (Transform slotTransform in runeSlots)  {
			GameObject item = slotTransform.GetComponent<RunePuzzle_Slot>().item;
			if (item)  {
				runeStringBuilder.Append (item.name);
				runeStringBuilder.Append (" - ");
			}
		}
		runesString = runeStringBuilder.ToString ();
		if (runesString.Length == 43)  {
			CheckTheRunes();
		}
	}

	public void CheckTheRunes()  {
		if (runesString == runesSolutionString)  {
			for (var i = 0; i < runeSlot.Length; i++)  {
				runeSlot [i].GetComponent<Image> ().color = new Color32 (255, 255, 255, 0);
			}
			for (var i = 0; i < runeTile.Length; i++) {
				runeTile[i].GetComponent<Animator> ().Play ("RuneDisappearsAnimation", -1, 0f);
			}
			StartCoroutine(TurnOffRunePuzzle ());
		}
		else  {
			puzzleTitle.SetActive (false);
			puzzleLostText.SetActive (true);
			puzzleWonText.SetActive (false);
		}
	}

	IEnumerator TurnOffRunePuzzle()  {
		yield return new WaitForSeconds (0.5f);
		puzzleTitle.SetActive (false);
		puzzleLostText.SetActive (false);
		puzzleWonText.SetActive (true);
		runeBoardPanel.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Icons/Items/runicSword");
		yield return new WaitForSeconds (2f);
		runePuzzlePanel.SetActive (false);
	}
}

namespace UnityEngine.EventSystems { 
	public interface IHasChanged : IEventSystemHandler { 
		void HasChanged();
	}
}
