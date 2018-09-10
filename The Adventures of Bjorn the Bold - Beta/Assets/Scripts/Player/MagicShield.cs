using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicShield : MonoBehaviour {

	[SerializeField] GameObject MagicSphere;
	[SerializeField] Image fillWaitImage;

	// variable handles how long shield will be on
	private bool shieldOn = false;

	// variable handles how long must wait before shield can be activated again
	private bool shieldActivated = false;
	private int fadeImage = 0;

	void Update()  {
		if (Input.GetKeyDown(KeyCode.Alpha1)) {
			if (GamePreferences.GetPukiFound () == 1 && GamePreferences.GetMagicShieldForPuki() == 1) {
				if (GamePreferences.GetFirePowerForPuki() == 1)  {
					bool isFirePowerOn = GameObject.FindGameObjectWithTag ("Player").GetComponent<ThunderPower> ().CheckCurrentThunderPowerStatus ();
					if (!isFirePowerOn)  {
						shieldActivated = true;
						StartCoroutine (CreateMagicShield ());					
					}
				}
				else  {
					shieldActivated = true;
					StartCoroutine (CreateMagicShield ());
				}
			}
		}
		if (shieldActivated) {
			CheckToFade ();
			CheckInput ();
		}
	}

	public bool CheckCurrentMagicShieldStatus()  {
		return shieldOn;
	}

	void CheckInput()  {
		fadeImage = 1;
	}

	void CheckToFade()  {
		if (FadeAndWait (fillWaitImage, 0.05f)) {
			fadeImage = 0;
			shieldActivated = false;
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
			shieldOn = false;
		}
		return faded;
	}

	IEnumerator CreateMagicShield()  {
		if (shieldOn)  {
			yield break;
		}
		shieldOn = true;
		MagicSphere.SetActive (true);
		yield return new WaitForSeconds (8f);
		MagicSphere.SetActive (false);

	}
}
