using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaveMazePuzzleController : MonoBehaviour {

	// variable to hold puzzle UI
	public GameObject mazePuzzleUI;

	// variable whether bell retrieved from RetrieveTheBell script
	public static bool haveBell = false;

	// variable whether bjorn has successfully exitted the cave with the bell
	public static bool exitCave = false;

	// variable to display bell in spire if it gets found
	[SerializeField] GameObject hideBellInSteeple;

	// dynamic instruction panels
	[SerializeField] GameObject instructionsPanel;
	[SerializeField] GameObject winPanel;

	// object to be moved
	[SerializeField] Transform playerObj;
	[SerializeField] private Animator playerAnim;
	[SerializeField] private float moveSpeed;
	private Vector2 direction;

	// variables to set the correct NPC if puzzle is solved or not
	[SerializeField] private GameObject guardianChurch_NoDialogue;
	[SerializeField] private GameObject guardianChurch_Dialogue;

	// variable to handle if Puki should be added (only if puzzle is solved)
	[SerializeField] private GameObject pukiOrb;
	[SerializeField] private GameObject pukiDialogueTrigger;   // only if Puki had already been found by the well

	// Use this for initialization
	void Start () {
		instructionsPanel.gameObject.SetActive (true);
		winPanel.gameObject.SetActive (false);
		pukiDialogueTrigger.SetActive (false);
		hideBellInSteeple.SetActive (true);
		StartCoroutine (TurnOffInstructions ());
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		GetInput ();
		Move ();

		// determine if the bell has been retrieved and bjorn has exitted the cave
		if (haveBell && exitCave)  {
			winPanel.SetActive (true);
			guardianChurch_NoDialogue.SetActive (true);
			guardianChurch_Dialogue.SetActive (false);
			hideBellInSteeple.SetActive (false);
			StartCoroutine (SuccessfullyRetrievedBell());
		}
	}

	public void Move()  {
		playerObj.transform.Translate (direction * moveSpeed * Time.deltaTime);
		if (direction.x != 0 || direction.y != 0)  {
			Animate (direction);
		}
		else  {
			playerAnim.SetLayerWeight (1, 0);
		}
	}

	private void GetInput()  {
		// set the direction to move the player
		direction = Vector2.zero;
		if (Input.GetKey(KeyCode.UpArrow))  {
			direction += Vector2.up;
		}
		if (Input.GetKey(KeyCode.DownArrow))  {
			direction += Vector2.down;
		}
		if (Input.GetKey(KeyCode.RightArrow))  {
			direction += Vector2.right;
		}
		if (Input.GetKey(KeyCode.LeftArrow))  {
			direction += Vector2.left;
		}
	}

	public void Animate(Vector2 direction)  {
		// change layer weight to switch between idle layer and walk layer
		playerAnim.SetLayerWeight (1, 1);
		// set the x and y direction to determine the correct animation to be played
		playerAnim.SetFloat ("x", direction.x);
		playerAnim.SetFloat ("y", direction.y);
	}

	IEnumerator TurnOffInstructions()  {
		yield return new WaitForSeconds (10f);
		instructionsPanel.gameObject.SetActive (false);
	}

	// method to handle win situation
	IEnumerator SuccessfullyRetrievedBell()  {
		yield return new WaitForSeconds (3f);
		// this turns on Puki Orb under Puki Effects ... 
		// this starts the flaming orb of puki before he attaches to player
		// 0 = false, 1 = true
		if (GamePreferences.GetPukiFound() == 1)  {
			pukiDialogueTrigger.SetActive (true);
			GamePreferences.SetFirePowerForPuki (1);
		}
		else  {
			pukiOrb.SetActive (true);
			GamePreferences.SetPukiFound (1);	
			GamePreferences.SetFirePowerForPuki (1);
		}
		mazePuzzleUI.SetActive (false);
	}
}