using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThunderPower : MonoBehaviour {

	[SerializeField] GameObject thunderPowerEffect;
	[SerializeField] GameObject ThunderPowerZone;
	[SerializeField] Image fillWaitImage;

	// variable to handle how long thunder power will be on
	private bool thunderPowerOn = false;

	// variable handles how long must wait before thunder power can be used again
	private bool thunderPowerUsed = false;
	private int fadeImage = 0;
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Alpha2))  {
			if (GamePreferences.GetPukiFound() == 1 && GamePreferences.GetFirePowerForPuki() == 1)  {
				if (GamePreferences.GetMagicShieldForPuki () == 1) {
					bool isMagicShieldOn = GameObject.FindGameObjectWithTag ("Player").GetComponent<MagicShield> ().CheckCurrentMagicShieldStatus ();
					if (!isMagicShieldOn) {
						thunderPowerUsed = true;
						StartCoroutine (CreateThunderPower ());
					}
				}
				else  {
					thunderPowerUsed = true;
					StartCoroutine (CreateThunderPower ());
				}
			}
		}
		if (thunderPowerUsed)  {
			CheckToFade ();
			CheckInput ();
		}
	}

	public bool CheckCurrentThunderPowerStatus()  {
		return thunderPowerOn;
	}

	void CheckInput()  {
		fadeImage = 1;
	}

	void CheckToFade()  {
		if (FadeAndWait(fillWaitImage, 0.25f))  {
			fadeImage = 0;
			thunderPowerUsed = false;
		}
	}

	bool FadeAndWait(Image fadeImg, float fadeTime)  {
		bool faded = false;
		if (fadeImg == null)
			return faded;
		if (!fadeImg.gameObject.activeInHierarchy)  {
			fadeImg.gameObject.SetActive (true);
			fadeImg.fillAmount = 1f;
		}
		fadeImg.fillAmount -= fadeTime * Time.deltaTime;
		if (fadeImg.fillAmount <= 0f)  {
			fadeImg.gameObject.SetActive (false);
			faded = true;
			thunderPowerOn = false;
		}
		return faded;
	}

	IEnumerator CreateThunderPower()  {
		if (thunderPowerOn)  {
			yield break;
		}
		thunderPowerOn = true;
		for (int i=0; i<6; i++)  {
			Vector3 pos = Vector3.zero;
			if (i == 0)  {
				pos = new Vector3 (transform.position.x + 1.5f, transform.position.y - 2f, transform.position.z);
			} else if (i == 1)  {
				pos = new Vector3 (transform.position.x - 1.5f, transform.position.y - 2f, transform.position.z);
			} else if (i == 2)  {
				pos = new Vector3 (transform.position.x + .75f, transform.position.y - 2f, transform.position.z + 1.5f);
			} else if (i == 3)  {
				pos = new Vector3 (transform.position.x - .75f, transform.position.y - 2f, transform.position.z + 1.5f);
			} else if (i == 4)  {				
				pos = new Vector3 (transform.position.x + .75f, transform.position.y - 2f, transform.position.z - 1.5f);
			} else if (i == 5)  {
				pos = new Vector3 (transform.position.x - .75f, transform.position.y - 2f, transform.position.z - 1.5f);
			}
			Instantiate (thunderPowerEffect, pos, Quaternion.identity);
		}
		ThunderPowerZone.SetActive (true);
		yield return new WaitForSeconds (0.5f);
		ThunderPowerZone.SetActive (false);
	}
}
