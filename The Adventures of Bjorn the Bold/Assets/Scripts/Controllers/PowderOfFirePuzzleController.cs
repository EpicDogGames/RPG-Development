using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowderOfFirePuzzleController : MonoBehaviour {

	// variable to hold puzzle UI and trigger
	public GameObject kegPuzzleUI;
	public GameObject kegPuzzleTrigger;

	// image object for each keg
	[SerializeField] private GameObject[] keg;
	[SerializeField] private int selectKegID;

	// buttons
	[SerializeField] private GameObject[] kegSelectedBtn;
	[SerializeField] private GameObject[] kegFilledBtn;

	// start, middle and end position of keg movement
	private Vector3 startPos;
	private Vector3 middlePos;
	private Vector3 endPos;

	// available positions for keg to move to
	[SerializeField] private GameObject WP1;
	[SerializeField] private GameObject WP2;
	[SerializeField] private GameObject WP3;
	[SerializeField] private GameObject WP4;
	[SerializeField] private GameObject WP5;
	[SerializeField] private GameObject WP6;

	// dynamic instruction panels
	[SerializeField] GameObject notificationPanel;
	[SerializeField] GameObject notificationPanel_SelectKeg;
	[SerializeField] GameObject notificationPanel_FillKeg;
	[SerializeField] GameObject keg_Buttons;
	[SerializeField] GameObject winPanel;
	 
	// keg weights
	[SerializeField] Text currentWgtValue_8lbKeg, currentWgtValue_5lbKeg, currentWgtValue_3lbKeg;
	private int currentWeight_8lbKeg, maxWeight_8lbKeg;
	private int currentWeight_5lbKeg, maxWeight_5lbKeg;
	private int currentWeight_3lbKeg, maxWeight_3lbKeg;

	// keg animations
	[SerializeField] private Animator keg8lb_Anim;
	[SerializeField] private Animator keg5lb_Anim;
	[SerializeField] private Animator keg3lb_Anim;

	// trigger for explosion effect if puzzle is solved
	[SerializeField]  private GameObject kegExplosionTrigger;
	[SerializeField] private GameObject rewardFromGuardian;

	private float secsToWaitForReset;

	void Start()  {
		// when puzzle starts, only the selected buttons will be active .. fill buttons inactive
		for (int i=0; i<kegSelectedBtn.Length; i++)  {
			kegSelectedBtn[i].SetActive(true);
		}
		for (int i=0; i<kegFilledBtn.Length; i++)  {
			kegFilledBtn [i].SetActive (false);
		}
		// set notification panels
		notificationPanel.gameObject.SetActive (true);
		notificationPanel_SelectKeg.SetActive (true);
		notificationPanel_FillKeg.SetActive (false);
		winPanel.SetActive (false);
		// set all weights and corresponding text
		currentWeight_8lbKeg = 8;
		currentWgtValue_8lbKeg.text = currentWeight_8lbKeg.ToString ();
		currentWeight_5lbKeg = 0;
		currentWgtValue_5lbKeg.text = currentWeight_5lbKeg.ToString ();
		currentWeight_3lbKeg = 0;
		currentWgtValue_3lbKeg.text = currentWeight_3lbKeg.ToString ();
		maxWeight_8lbKeg = 8;
		maxWeight_5lbKeg = 5;
		maxWeight_3lbKeg = 3;
		selectKegID = -99;
		// make sure keg explosion trigger is not active .. only if puzzle solved will it become active
		kegExplosionTrigger.SetActive (false);
		rewardFromGuardian.SetActive (false);
	}

	void Update()  {
		if (currentWeight_8lbKeg == 4 && currentWeight_5lbKeg == 4)  {
			StartCoroutine (SuccessfulDistribution ());
		}
	}

	// method for selecting the 8lb keg
	public void KegSelected_8lb()  {
		for (int i=0; i<kegSelectedBtn.Length; i++)  {
			kegSelectedBtn[i].SetActive (false);
		}	
		for (int i=0; i<kegFilledBtn.Length; i++)  {
			if (kegFilledBtn[i].name == "Btn_8lbKeg_Fill") {
				kegFilledBtn[i].SetActive (false);
			}
			else {
				kegFilledBtn[i].SetActive (true);
			}
		}
		selectKegID = 0;
		startPos = WP1.transform.position;
		endPos = WP2.transform.position;
		StartCoroutine (SingleLerp (keg[selectKegID], startPos, endPos, 1f));
	}

	// method for selecting the 5lb keg
	public void KegSelected_5lb()  {
		for (int i=0; i<kegSelectedBtn.Length; i++)  {
			kegSelectedBtn[i].SetActive (false);
		}	
		for (int i=0; i<kegFilledBtn.Length; i++)  {
			if (kegFilledBtn[i].name == "Btn_5lbKeg_Fill") {
				kegFilledBtn[i].SetActive (false);
			}
			else {
				kegFilledBtn[i].SetActive (true);
			}
		}
		selectKegID = 2;
		startPos = WP5.transform.position;
		endPos = WP6.transform.position;
		StartCoroutine (SingleLerp (keg[selectKegID], startPos, endPos, 1f));
	}

	// method for selecting the 3lb keg
	public void KegSelected_3lb()  {
		for (int i=0; i<kegSelectedBtn.Length; i++)  {
			kegSelectedBtn[i].SetActive (false);
		}	
		for (int i=0; i<kegFilledBtn.Length; i++)  {
			if (kegFilledBtn[i].name == "Btn_3lbKeg_Fill") {
				kegFilledBtn[i].SetActive (false);
			}
			else {
				kegFilledBtn[i].SetActive (true);
			}
		}
		selectKegID = 1;
		startPos = WP3.transform.position;
		endPos = WP4.transform.position;
		StartCoroutine (SingleLerp (keg[selectKegID], startPos, endPos, 1f));
	}

	// method for handling filling the 8lb keg after selecting either the 5lb or 3lb keg
	public void KegFilled_8lb()  {
		// 3lb keg selected to fill 8lb keg
		if (selectKegID == 1)  {
			Debug.Log ("Selected Keg: 3lb Keg (" + selectKegID + ") Filled Keg: 8lb Keg" );	
			if (currentWeight_3lbKeg == 0 || maxWeight_8lbKeg == currentWeight_8lbKeg)  {
				secsToWaitForReset = 1f;
				startPos = WP4.transform.position;
				endPos = WP3.transform.position;
				StartCoroutine (SingleLerp (keg [selectKegID], startPos, endPos, 1f));

			}
			else  {
				int amtToFill = maxWeight_8lbKeg - currentWeight_8lbKeg;
				if (currentWeight_3lbKeg <= amtToFill)  {
					currentWeight_8lbKeg = currentWeight_8lbKeg + currentWeight_3lbKeg;
					currentWeight_3lbKeg = 0;
				}
				else  {
					currentWeight_8lbKeg = currentWeight_8lbKeg + amtToFill;
					currentWeight_3lbKeg = currentWeight_3lbKeg - amtToFill;
				}
				secsToWaitForReset = 7f;
				startPos = WP4.transform.position;
				middlePos = WP2.transform.position;
				endPos = WP3.transform.position;
				StartCoroutine (MultipleLerp (keg [selectKegID], keg3lb_Anim, startPos, middlePos, endPos, 1f));
			}
			StartCoroutine (ResetKegs (secsToWaitForReset));
		}
		// 5lb keg selected to fill 8lb keg
		if (selectKegID == 2)  {
			Debug.Log ("Selected Keg: 5lb Keg (" + selectKegID + ") Filled Keg: 8lb Keg" );	
			if (currentWeight_5lbKeg == 0 || maxWeight_8lbKeg == currentWeight_8lbKeg)  {
				secsToWaitForReset = 1f;
				startPos = WP6.transform.position;
				endPos = WP5.transform.position;
				StartCoroutine (SingleLerp (keg [selectKegID], startPos, endPos, 1f));
			}
			else  {
				int amtToFill = maxWeight_8lbKeg - currentWeight_8lbKeg;
				if (currentWeight_5lbKeg <= amtToFill)  {
					currentWeight_8lbKeg = currentWeight_8lbKeg + currentWeight_5lbKeg;
					currentWeight_5lbKeg = 0;
				}
				else  {
					currentWeight_8lbKeg = currentWeight_8lbKeg + amtToFill;
					currentWeight_5lbKeg = currentWeight_5lbKeg - amtToFill;
				}
				secsToWaitForReset = 7f;
				startPos = WP6.transform.position;
				middlePos = WP2.transform.position;
				endPos = WP5.transform.position;
				StartCoroutine (MultipleLerp (keg [selectKegID], keg5lb_Anim, startPos, middlePos, endPos, 1f));
			}
			StartCoroutine (ResetKegs (secsToWaitForReset));
		}
	}

	// method for handling filling the 5lb keg after selecting the 8lb or 3lb keg
	public void KegFilled_5lb()  {
		// 8lb keg selected to fill 5lb keg
		if (selectKegID == 0)  {
			Debug.Log ("Selected Keg: 8lb Keg (" + selectKegID + ") Filled Keg: 5lb Keg" );
			if (currentWeight_8lbKeg == 0 || maxWeight_5lbKeg == currentWeight_5lbKeg)  {
				secsToWaitForReset = 1f;
				startPos = WP2.transform.position;
				endPos = WP1.transform.position;
				StartCoroutine (SingleLerp (keg [selectKegID], startPos, endPos, 1f));
			}
			else  {
				int amtToFill = maxWeight_5lbKeg - currentWeight_5lbKeg;
				if (currentWeight_8lbKeg <= amtToFill)  {
					currentWeight_5lbKeg = currentWeight_5lbKeg + currentWeight_8lbKeg;
					currentWeight_8lbKeg = 0;
				}
				else  {
					currentWeight_5lbKeg = currentWeight_5lbKeg + amtToFill;
					currentWeight_8lbKeg = currentWeight_8lbKeg - amtToFill;
				}
				secsToWaitForReset = 7f;
				startPos = WP2.transform.position;
				middlePos = WP6.transform.position;
				endPos = WP1.transform.position;
				StartCoroutine (MultipleLerp (keg [selectKegID], keg8lb_Anim, startPos, middlePos, endPos, 1f));
			}
			StartCoroutine (ResetKegs (secsToWaitForReset));
		}
		// 3lb keg selected to fill 5lb keg
		if (selectKegID == 1) {
			Debug.Log ("Selected Keg: 3lb Keg (" + selectKegID + ") Filled Keg: 5lb Keg" );
			if (currentWeight_3lbKeg == 0 || maxWeight_5lbKeg == currentWeight_5lbKeg)  {
				secsToWaitForReset = 1f;
				startPos = WP4.transform.position;
				endPos = WP3.transform.position;
				StartCoroutine (SingleLerp (keg [selectKegID], startPos, endPos, 1f));
			}
			else  {
				int amtToFill = maxWeight_5lbKeg - currentWeight_5lbKeg;
				if (currentWeight_3lbKeg <= amtToFill)  {
					currentWeight_5lbKeg = currentWeight_5lbKeg + currentWeight_3lbKeg;
					currentWeight_3lbKeg = 0;
				}
				else  {
					currentWeight_5lbKeg = currentWeight_5lbKeg + amtToFill;
					currentWeight_3lbKeg = currentWeight_3lbKeg - amtToFill;
				}
				secsToWaitForReset = 7f;
				startPos = WP4.transform.position;
				middlePos = WP6.transform.position;
				endPos = WP3.transform.position;
				StartCoroutine (MultipleLerp (keg [selectKegID], keg3lb_Anim, startPos, middlePos, endPos, 1f));
			}
			StartCoroutine (ResetKegs (secsToWaitForReset));
		}
	}

	// method for handling filling the 3lb keg after selecting either the 5lb or 8lb keg
	public void KegFilled_3lb()  {
		// 8lb keg selected to fill 3lb keg
		if (selectKegID == 0)  {
			Debug.Log ("Selected Keg: 8lb Keg (" + selectKegID + ") Filled Keg: 3lb Keg" );
			if (currentWeight_8lbKeg == 0 || maxWeight_3lbKeg == currentWeight_3lbKeg)  {
				secsToWaitForReset = 1f;
				startPos = WP2.transform.position;
				endPos = WP1.transform.position;
				StartCoroutine (SingleLerp (keg [selectKegID], startPos, endPos, 1f));
			}
			else  {
				int amtToFill = maxWeight_3lbKeg - currentWeight_3lbKeg;
				if (currentWeight_8lbKeg <= amtToFill)  {
					currentWeight_3lbKeg = currentWeight_3lbKeg + currentWeight_8lbKeg;
					currentWeight_8lbKeg = 0;
				}
				else  {
					currentWeight_3lbKeg = currentWeight_3lbKeg + amtToFill;
					currentWeight_8lbKeg = currentWeight_8lbKeg - amtToFill;
				}
				secsToWaitForReset = 7f;
				startPos = WP2.transform.position;
				middlePos = WP4.transform.position;
				endPos = WP1.transform.position;
				StartCoroutine (MultipleLerp (keg [selectKegID], keg8lb_Anim, startPos, middlePos, endPos, 1f));
			}
			StartCoroutine (ResetKegs (secsToWaitForReset));
		}
		// 5lb keg selected to fill 3lb keg
		if (selectKegID == 2)  {
			Debug.Log ("Selected Keg: 5lb Keg (" + selectKegID + ") Filled Keg: 3lb Keg" );
			if (currentWeight_5lbKeg == 0 || maxWeight_3lbKeg == currentWeight_3lbKeg)  {
				secsToWaitForReset = 1f;
				startPos = WP6.transform.position;
				endPos = WP5.transform.position;
				StartCoroutine (SingleLerp (keg [selectKegID], startPos, endPos, 1f));
			}
			else  {
				int amtToFill = maxWeight_3lbKeg - currentWeight_3lbKeg;
				if (currentWeight_5lbKeg <= amtToFill)  {
					currentWeight_3lbKeg = currentWeight_3lbKeg + currentWeight_5lbKeg;
					currentWeight_5lbKeg = 0;
				}
				else  {
					currentWeight_3lbKeg = currentWeight_3lbKeg + amtToFill;
					currentWeight_5lbKeg = currentWeight_5lbKeg - amtToFill;
				}
				secsToWaitForReset = 7f;
				startPos = WP6.transform.position;
				middlePos = WP4.transform.position;
				endPos = WP5.transform.position;
				StartCoroutine (MultipleLerp (keg [selectKegID], keg5lb_Anim, startPos, middlePos, endPos, 1f));
			}
			StartCoroutine (ResetKegs (secsToWaitForReset));		
		}
	}

	// method to handle keg moving up and down when selected
	IEnumerator SingleLerp(GameObject kegToMove, Vector3 startPos, Vector3 endPos, float speed)  {
		notificationPanel_SelectKeg.SetActive (false);
		notificationPanel_FillKeg.SetActive (true);
		float timer = 0f;
		while (timer <= 1f)  {
			timer += Time.deltaTime * speed;
			kegToMove.transform.position = Vector3.Lerp (startPos, endPos, timer);
			yield return new WaitForEndOfFrame ();
		}
		yield return false;
	}

	// method to handle keg moving, rotating, and returning to original position
	IEnumerator MultipleLerp(GameObject kegToMove, Animator kegAnim, Vector3 startPos, Vector3 midPos, Vector3 endPos, float speed)  {
		notificationPanel_SelectKeg.SetActive (false);
		notificationPanel_FillKeg.SetActive (true);
		float timer = 0f;
		while (timer <= 1.5f)  {
			timer += Time.deltaTime * speed;
			kegToMove.transform.position = Vector3.Lerp (startPos, midPos, timer);
			yield return new WaitForEndOfFrame ();
		}
		timer = 0f;
		while(timer <= 2f)  {
			timer += Time.deltaTime * speed;
			kegAnim.SetBool ("Rotate", true);
			yield return new WaitForEndOfFrame ();
		}
		kegAnim.SetBool ("Rotate", false);
		// set all the text values to the current weight values of the kegs
		currentWgtValue_3lbKeg.text = currentWeight_3lbKeg.ToString ();
		currentWgtValue_5lbKeg.text = currentWeight_5lbKeg.ToString ();
		currentWgtValue_8lbKeg.text = currentWeight_8lbKeg.ToString ();
		timer = 0f;
		while(timer <= 1.5f)  {
			timer+= Time.deltaTime * speed;
			kegToMove.transform.position = Vector3.Lerp(midPos, startPos, timer);
			yield return new WaitForEndOfFrame();
		}
		timer = 0f;
		while(timer <= 1.5f)  {
			timer += Time.deltaTime * speed;
			kegToMove.transform.position = Vector3.Lerp(startPos, endPos, timer);
			yield return new WaitForEndOfFrame();
		}
		yield return false;
	}

	// method to handle wait time before resetting all the buttons for selection purposes again
	IEnumerator ResetKegs(float secsToWait)  {
		for (int i=0; i<kegFilledBtn.Length; i++)  {
			kegFilledBtn[i].SetActive (false);
		}
		yield return new WaitForSeconds (secsToWait);
		selectKegID = -99;
		for (int i=0; i<kegSelectedBtn.Length; i++)  {
			kegSelectedBtn[i].SetActive (true);
		}
		notificationPanel_SelectKeg.SetActive (true);
		notificationPanel_FillKeg.SetActive (false);
	}

	// method to handle win situation
	IEnumerator SuccessfulDistribution()  {
		notificationPanel.gameObject.SetActive (false);
		keg_Buttons.SetActive (false);
		for (int i=0; i<kegFilledBtn.Length; i++)  {
			kegFilledBtn [i].SetActive (false);
		}
		for (int i=0; i<kegSelectedBtn.Length; i++)  {
			kegSelectedBtn [i].SetActive (false);
		}
		yield return new WaitForSeconds (3f);
		winPanel.SetActive (true);
		yield return new WaitForSeconds (5f);
		Destroy (kegPuzzleTrigger);
		kegPuzzleUI.SetActive (false);
		// turn the keg explosion trigger on
		kegExplosionTrigger.SetActive (true);
		rewardFromGuardian.SetActive (true);
	}
}
