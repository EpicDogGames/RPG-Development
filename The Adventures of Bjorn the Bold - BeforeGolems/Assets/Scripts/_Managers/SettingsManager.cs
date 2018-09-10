using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour {

	public GameObject keyboardPanel;
	public GameObject settingsPanel;

	public Toggle isMusicOn;
	public Slider musicVolumeSlider;
	public Toggle isSFxOn;
	public Slider sfxVolumerSlider;

	public Toggle isFullscreenOn;
	public Dropdown screenResolution;
	public Dropdown qualitySetting;

	void Start()  {
		// set up the panel to be displayed
		keyboardPanel.SetActive (true);
		settingsPanel.SetActive (false);
		// initialize the game settings based on game preferences
		isMusicOn.isOn = (GamePreferences.GetMusicState() == 1)? true:false;
		musicVolumeSlider.value = GamePreferences.GetMusicVolume ();
		isSFxOn.isOn = (GamePreferences.GetFXState () == 1) ? true : false;
		sfxVolumerSlider.value = GamePreferences.GetFXVolume ();
		isFullscreenOn.isOn = (GamePreferences.GetFullscreenState () == 1) ? true : false;
		screenResolution.value = GamePreferences.GetScreenResolution ();
		qualitySetting.value = GamePreferences.GetQualitySetting ();
	}

	public void KeyboardSettingsPanel()  {
		keyboardPanel.SetActive (true);
		settingsPanel.SetActive (false);
	}

	public void SettingsPanel()  {
		keyboardPanel.SetActive (false);
		settingsPanel.SetActive (true);
	}

	public void SetMusicToggle()  {
		if (isMusicOn.isOn)  {
			GamePreferences.SetMusicState (1);
			MusicManager.Instance.PlayMusic (true);
			MusicManager.Instance.ChangeVolume (GamePreferences.GetMusicVolume ());
		}
		else {
			GamePreferences.SetMusicState (0);
			MusicManager.Instance.PlayMusic (false);
		}
	}

	public void SetMusicVolume()  {
		GamePreferences.SetMusicVolume (musicVolumeSlider.value);
		MusicManager.Instance.ChangeVolume (musicVolumeSlider.value);
	}

	public void SetSFxToggle()  {
		if (isSFxOn.isOn)  {
			GamePreferences.SetFXState (1);
		}
		else  {
			GamePreferences.SetFXState (0);
		}
	}

	public void SetFXVolume()  {
		GamePreferences.SetFXVolume (sfxVolumerSlider.value);
	}

	public void SetFullScreenToggle()  {
		if (isFullscreenOn.isOn) {
			GamePreferences.SetFullscreenState (1);
			if (GamePreferences.GetScreenResolution() == 0)  {
				Screen.SetResolution (1280, 720, true);
			}
			else if (GamePreferences.GetScreenResolution() == 1)  {
				Screen.SetResolution (1600, 900, true);
			}
			else  {
				Screen.SetResolution (1920, 1080, true);
			}
		}
		else  {
			GamePreferences.SetFullscreenState (0);
			if (GamePreferences.GetScreenResolution() == 0)  {
				Screen.SetResolution (1280, 720, false);
			}
			else if (GamePreferences.GetScreenResolution() == 1)  {
				Screen.SetResolution (1600, 900, false);
			}
			else  {
				Screen.SetResolution (1920, 1080, false);
			}
		}
	}

	public void SetQuality()  {
		GamePreferences.SetQualitySetting (qualitySetting.value);
		QualitySettings.SetQualityLevel (qualitySetting.value);
	}

	public void SetScreenResolution()  {
		GamePreferences.SetScreenResolution (screenResolution.value);
		if (screenResolution.value == 0)  {
			if (GamePreferences.GetFullscreenState () == 0) {
				Screen.SetResolution (1280, 720, false);
			} else {
				Screen.SetResolution (1280, 720, true);
			}
		}
		else if (screenResolution.value == 1)  {
			if (GamePreferences.GetFullscreenState () == 0) {
				Screen.SetResolution (1600, 900, false);
			} else {
				Screen.SetResolution (1600, 900, true);
			}
		}
		else {
			if (GamePreferences.GetFullscreenState () == 0) {
				Screen.SetResolution (1920, 1080, false);
			} else {
				Screen.SetResolution (1920, 1080, true);
			}
		}
	}

	public void BackToMainMenu()  {
		SceneManager.LoadScene("01_MainMenu");
	}
}
