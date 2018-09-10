using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuardianSearchPuzzleController : MonoBehaviour {

	// variable to hold puzzle UI and trigger
	public GameObject guardianPuzzleUI;
	public GameObject guardianPuzzleTrigger;

	// variables for adding buttons to the gameboard
	[SerializeField] private Transform guardianGameBoard;
	[SerializeField] private GameObject guardianPuzzleButton;

	// variables for setting up the tiles of the board and game logic
	[SerializeField] private Sprite[] bgImage;
	[SerializeField] private Sprite[] guardianImage;
	[SerializeField] private int guardianCount = 0;

	// variables to set the correct NPC if puzzle is solved or not
	[SerializeField] private GameObject guardianWell_NoDialogue;
	[SerializeField] private GameObject guardianWell_Dialogue;

	// variable to handle if Puki should be added (only if puzzle is solved)
	[SerializeField] private GameObject pukiOrb;
	[SerializeField] private GameObject lightWellEffects;
	[SerializeField] private GameObject pukiDialogueTrigger;  // only if Puki has already been found at the cave

	// listeners and panels
	public List<Button> guardianPuzzleButtons = new List<Button>();
	public GameObject losePanel;
	public GameObject winPanel;

	void Awake()  {
		for (int i=0; i<36; i++)  {
			GameObject button = Instantiate (guardianPuzzleButton);
			button.name = "" + i;
			button.transform.SetParent (guardianGameBoard, false);
		}
	}

	void Start()  {
		losePanel.SetActive (false);
		winPanel.SetActive (false);
		GetPuzzleButtons ();
		AddListeners ();
	}

	void GetPuzzleButtons()  {
		GameObject[] objects = GameObject.FindGameObjectsWithTag ("GuardianPuzzleButton");
		for (int j=0; j<objects.Length; j++)  {
			guardianPuzzleButtons.Add(objects[j].GetComponent<Button>());
			// red background
			if (j==7 || j==13 || j==14 || j==15 || j==20 || j==21 || j==22 || j==23 || j==27 || j==28 || j==29 || j==32 || j==33 | j==34 || j==35 )  {
				guardianPuzzleButtons[j].image.sprite = bgImage[0];
			}
			// blue background
			if (j==4 || j==5 || j==10 || j==11 || j==16 || j==17) {
				guardianPuzzleButtons[j].image.sprite = bgImage[1];
			}
			// green background
			if (j==6 || j==12 || j==18 || j==24 || j==30)  {
				guardianPuzzleButtons[j].image.sprite = bgImage [2];
			}
			// purple background
			if (j==19 || j==25 || j==26 || j==31)  {
				guardianPuzzleButtons[j].image.sprite = bgImage [3];
			}
			// yellow background
			if (j==0 || j==1 || j==2)  {
				guardianPuzzleButtons[j].image.sprite = bgImage[4];
			}
			// silver background
			if (j==3 || j==8 || j==9)  {
				guardianPuzzleButtons[j].image.sprite = bgImage [5];
			}
		}
	}

	void AddListeners()  {
		foreach (Button btn in guardianPuzzleButtons)  {
			btn.onClick.AddListener (() => PlaceAGuardian ());
		}
	}

	public void PlaceAGuardian()  {
		winPanel.SetActive (false);
		losePanel.SetActive (false);
		Image image = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Image>();
		string objName = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
		string imageName = image.sprite.name;
		// red guardian or background
		if (objName == "7" || objName == "13" || objName == "14" || objName == "15" || objName == "20" || objName == "21" || objName == "22" || objName == "23" || objName == "27" || objName == "28" || objName == "29" || objName == "32" || objName == "33" || objName == "34" || objName == "35") {
			if (imageName == "BG_Red") {
				if (guardianCount < 6) {
					image.sprite = guardianImage [0];
					guardianCount++;
				}
			}
			else  {
				image.sprite = bgImage [0];
				guardianCount--;
			}
		}
		// blue guardian or background
		if (objName == "4" || objName == "5" || objName == "10" || objName == "11" || objName == "16" || objName == "17") {
			if (imageName == "BG_Blue") {
				if (guardianCount < 6) {
					image.sprite = guardianImage [1];
					guardianCount++;
				}
			}
			else  {
				image.sprite = bgImage [1];
				guardianCount--;
			}
		}
		// green guardian or background
		if (objName == "6" || objName == "12" || objName == "18" || objName == "24" || objName == "30") {
			if (imageName == "BG_Green") {
				if (guardianCount < 6) {
					image.sprite = guardianImage [2];
					guardianCount++;
				}
			}
			else  {
				image.sprite = bgImage [2];
				guardianCount--;
			}
		}
		// purple guardian or background
		if (objName == "19" || objName == "25" || objName == "26" || objName == "31") {
			if (imageName == "BG_Purple") {
				if (guardianCount < 6) {
					image.sprite = guardianImage [3];
					guardianCount++;
				}
			}
			else  {
				image.sprite = bgImage [3];
				guardianCount--;
			}
		}
		// yellow guardian or background
		if (objName == "0" || objName == "1" || objName == "2") {
			if (imageName == "BG_Yellow") {
				if (guardianCount < 6) {
					image.sprite = guardianImage [4];
					guardianCount++;
				}
			}
			else  {
				image.sprite = bgImage [4];
				guardianCount--;
			}
		}
		// silver guardian or background
		if (objName == "3" || objName == "8" || objName == "9") {
			if (imageName == "BG_Silver") {
				if (guardianCount < 6) {
					image.sprite = guardianImage [5];
					guardianCount++;
				}
			}
			else  {
				image.sprite = bgImage [5];
				guardianCount--;
			}
		}
		// check solution
		if (guardianCount == 6)  {
			CheckSolution ();
		}
		
	}

	void CheckSolution()  {
		string redGuardian = guardianPuzzleButtons[34].image.sprite.name;
		string blueGuardian = guardianPuzzleButtons[17].image.sprite.name;
		string greenGuardian = guardianPuzzleButtons[18].image.sprite.name;
		string purpleGuardian = guardianPuzzleButtons[26].image.sprite.name;
		string yellowGuardian = guardianPuzzleButtons[1].image.sprite.name;
		string silverGuardian = guardianPuzzleButtons[9].image.sprite.name;
		if (redGuardian == "Ore_Red" && blueGuardian == "Ore_Blue" && greenGuardian == "Ore_Green"  && purpleGuardian == "Ore_Purple" && yellowGuardian == "Ore_Yellow" && silverGuardian == "Ore_Silver")  {
			winPanel.SetActive (true);
			losePanel.SetActive (false);
			guardianWell_NoDialogue.SetActive (true);
			guardianWell_Dialogue.SetActive (false);
			StartCoroutine(TurnOffGuardianSearchPuzzle ());
		}
		else  {
			losePanel.SetActive (true);
			winPanel.SetActive (false);
		}
	}

	IEnumerator TurnOffGuardianSearchPuzzle()  {
		yield return new WaitForSeconds (1f);
		Destroy (guardianPuzzleTrigger);
		// this turns on Puki Orb under Puki Effects ... 
		// this starts the upward spiraling climb of puki before he attaches to player
		// 0 = false, 1 = true
		if (GamePreferences.GetPukiFound() == 1)  {
			pukiDialogueTrigger.SetActive (true);
			GamePreferences.SetMagicShieldForPuki (1);	
		}
		else  {
			pukiOrb.SetActive (true);
			GamePreferences.SetPukiFound (1);	
			GamePreferences.SetMagicShieldForPuki (1);
		}
		guardianPuzzleUI.SetActive (false);
		lightWellEffects.SetActive (false);
	}
}
