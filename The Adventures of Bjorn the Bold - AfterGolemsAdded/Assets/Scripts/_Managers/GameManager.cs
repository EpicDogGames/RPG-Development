using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameManager : MonoBehaviour {

	// this script is called from SplashScreen since it only needs to be called once

	public static GameManager Instance;

	private int screenResolution;
	private bool isFullScreen;
	private DirectoryInfo dirInfo;

	// Use this for initialization
	void Awake () {
		if (Instance != null && Instance != this)  {
			Destroy (gameObject);
		}
		else  {
			Instance = this;
		}
		Debug.Log ("Running the game manager");
		InitializeVariables ();
		InitializeDirectories ();
		ApplyGameSettings ();
	}
	
	void InitializeVariables()  {
		if (!PlayerPrefs.HasKey("Game Initialized"))  {
			Debug.Log ("Game being initialized");
			GamePreferences.SetMusicState (1);
			GamePreferences.SetMusicVolume (0.5f);
			GamePreferences.SetFXState (1);
			GamePreferences.SetFXVolume (0.8f);
			GamePreferences.SetFullscreenState(0);
			GamePreferences.SetScreenResolution(1);
			GamePreferences.SetQualitySetting(2);
			GamePreferences.SetStartOfGame(1);
			GamePreferences.SetFirstInventory(0);
			GamePreferences.SetFirstInventoryItem(0);
			GamePreferences.SetFirstCombat(0);
			GamePreferences.SetFirstCombatEncounter(0);
			GamePreferences.SetFirstQuest(0);
			GamePreferences.SetFirstQuestEntry(0);
			GamePreferences.SetFirstMap (0);
			GamePreferences.SetFirstMapEntry (0);
			GamePreferences.SetPukiFound (0);

			PlayerPrefs.SetInt ("Game Initialized", 123);
		}
		else  {
			Debug.Log ("Game has already been initialized");
			Debug.Log ("Music Settings : " + GamePreferences.GetMusicState () + ", " + GamePreferences.GetMusicVolume ());
			Debug.Log ("FX Settings : " + GamePreferences.GetFXState () + ", " + GamePreferences.GetFXVolume ());
			Debug.Log ("Screen Resolution : " + GamePreferences.GetScreenResolution () + ", " + GamePreferences.GetFullscreenState ());
			Debug.Log ("Quality Settings : " + GamePreferences.GetQualitySetting ());
		}
	}

	void ApplyGameSettings()  {
		// set audio preferences
		if (GamePreferences.GetMusicState() == 1)  {
			MusicManager.Instance.PlayMusic (true);
			MusicManager.Instance.ChangeVolume (GamePreferences.GetMusicVolume ());
		}
		else  {
			MusicManager.Instance.PlayMusic (false);
		}
		// set video preferences
		int screenResolution = GamePreferences.GetScreenResolution ();
		if (screenResolution == 0)  {
			if (GamePreferences.GetFullscreenState () == 1) {	
				Screen.SetResolution (1280, 720, true);
			}
			else  {
				Screen.SetResolution (1280, 720, false);
			}
		}
		else if (screenResolution == 1)  {
			if (GamePreferences.GetFullscreenState () == 2) {
				Screen.SetResolution (1600, 900, true);
			}
			else  {
				Screen.SetResolution (1600, 900, false);
			}
		}
		else  {
			if (GamePreferences.GetFullscreenState () == 2) {
				Screen.SetResolution (1920, 1080, true);
			}
			else  {
				Screen.SetResolution (1920, 1080, false);
			}
		}
		QualitySettings.SetQualityLevel (GamePreferences.GetQualitySetting ());
	}

	void InitializeDirectories()  {
		// set up saves directory under persistentDataPath if it doesn't exist
		dirInfo = new DirectoryInfo (Application.persistentDataPath + "/" + "Saves");
		if (!dirInfo.Exists)  {
			dirInfo.Create ();
		}
		// set up player directory under persistentDataPath if it doesn't exist
		dirInfo = new DirectoryInfo (Application.persistentDataPath + "/" + "Player");
		if (!dirInfo.Exists)  {
			dirInfo.Create ();
		}
		// set up quest directory (for in-game progress) under persistentDataPath if it doesn't exist
		dirInfo = new DirectoryInfo (Application.persistentDataPath + "/" + "Quests");
		if (!dirInfo.Exists)  {
			dirInfo.Create ();
		}
		// set up saved quest directory (for saved game option) under persistentDataPath if it doesn't exist
		dirInfo = new DirectoryInfo (Application.persistentDataPath + "/" + "SavedQuests");
		if (!dirInfo.Exists)  {
			dirInfo.Create ();
		}
		// set up map directory under persistentDataPath if it doesn't exist
		dirInfo = new DirectoryInfo (Application.persistentDataPath + "/" + "Map");
		if (!dirInfo.Exists)  {
			dirInfo.Create ();
		}
	}
}
