using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour {

	// this class is for test purposes only ... not to be used in the game 

	// variables for rune puzzle
	public static bool RunePuzzleIsDisplayed = false;
	public GameObject runePuzzleUI;
	// variables for guardian search puzzle
	public static bool GuardianSearchPuzzleIsDisplayed = false;
	public GameObject guardianSearchPuzzleUI;
	// variables for powder of fire puzzle
	public static bool PowderOfFirePuzzleIsDisplayed = false;
	public GameObject powderOfFirePuzzleUI;
	// variables for cave maze puzzle
	public static bool CaveMazePuzzleIsDisplayed = false;
	public GameObject caveMazePuzzleUI;

	void DisplayRunePuzzleOn()  {
		runePuzzleUI.SetActive (true);
		RunePuzzleIsDisplayed = true;
	}

	public void DisplayRunePuzzleOff()  {
		runePuzzleUI.SetActive (false);
		Time.timeScale = 1f;
		RunePuzzleIsDisplayed = false;
	}

	void DisplayGuardianSearchPuzzleOn()  {
		guardianSearchPuzzleUI.SetActive (true);
		GuardianSearchPuzzleIsDisplayed = true;
	}

	public void DisplayGuardianSearchPuzzleOff()  {
		guardianSearchPuzzleUI.SetActive (false);
		Time.timeScale = 1f;
		GuardianSearchPuzzleIsDisplayed = false;
	}

	void DisplayPowderOfFirePuzzleOn()  {
		powderOfFirePuzzleUI.SetActive (true);
		PowderOfFirePuzzleIsDisplayed = true;
	}

	public void DisplayPowderOfFirePuzzleOff()  {
		powderOfFirePuzzleUI.SetActive (false);
		Time.timeScale = 1f;
		PowderOfFirePuzzleIsDisplayed = false;
	}

	void DisplayCaveMazePuzzleOn()  {
		caveMazePuzzleUI.SetActive (true);
		CaveMazePuzzleIsDisplayed = true;
	}

	public void DisplayCaveMazePuzzleOff()  {
		caveMazePuzzleUI.SetActive (false);
		Time.timeScale = 1f;
		CaveMazePuzzleIsDisplayed = false;
	}
}
