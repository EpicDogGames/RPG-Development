using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainMenuManager : MonoBehaviour {

	public GameObject mainMenuOptions;
	public GameObject loadingScreen;
	public GameObject gameOptions;
	public Slider progressSlider;

	public void Start()  {
		mainMenuOptions.SetActive (true);
		gameOptions.SetActive (false);
	}

	public void PlayGame(string sceneName)  {
		bool startTheCoroutine = true;
		//  check to see if game has been saved
		DirectoryInfo dirInfo = new DirectoryInfo (Application.persistentDataPath + "/" + "Saves");
		if (dirInfo.Exists) {
			Debug.Log ("The directory exists");
			if (System.IO.File.Exists (Application.persistentDataPath + "/Saves/GameSave.xml")) {
				Debug.Log ("The save file exists");
				mainMenuOptions.SetActive (false);
				gameOptions.SetActive (true);
				startTheCoroutine = false;
			}
			else  {
				Debug.Log ("The save file doesn't exist");
				// make sure there aren't any old files if the GameSave.xml isn't present
				if (System.IO.File.Exists (Application.persistentDataPath + "/Player/Inventory.xml")) {
					File.Delete(Application.persistentDataPath + "/Player/Inventory.xml");
				}
				if (System.IO.File.Exists (Application.persistentDataPath + "/Player/PlayerData.xml")) {
					File.Delete(Application.persistentDataPath + "/Player/PlayerData.xml");
				}
				if (System.IO.File.Exists (Application.persistentDataPath + "/Map/MapData.xml")) {
					File.Delete(Application.persistentDataPath + "/Map/MapData.xml");
				}
				string questsDirPath = Application.persistentDataPath + "/" + "Quests";
				string[] questFilesInDirectory = Directory.GetFiles(questsDirPath, "*.xml");
				foreach (string fileInDirectory in questFilesInDirectory)  {
					File.Delete(fileInDirectory);
				}
				string savedQuestsDirPath = Application.persistentDataPath + "/" + "SavedQuests";
				string[] savedQuestFilesInDirectory = Directory.GetFiles(savedQuestsDirPath, "*.xml");
				foreach (string fileInDirectory in savedQuestFilesInDirectory)  {
					File.Delete(fileInDirectory);
				}
				startTheCoroutine = true;
			}
		}
		if (startTheCoroutine) {
			StartCoroutine (LoadSceneAsynchronously (sceneName));
		}
	}

	// this method will clear out any content such as old saved game or old saved quests or old player data (inventory, health, position, etc)
	// resets all the tutorial popups to initial settings
	public void NewGame(string sceneName)  {
		Debug.Log ("You have selected new game");
		// delete old saved game
		DirectoryInfo savedDirInfo = new DirectoryInfo (Application.persistentDataPath + "/" + "Saves");
		if (savedDirInfo.Exists)  {
			if (System.IO.File.Exists (Application.persistentDataPath + "/Saves/GameSave.xml")) {
				File.Delete (Application.persistentDataPath + "/Saves/GameSave.xml");
			}
		}
		// delete old saved quests ... both in-game progress as well from saved game
		DirectoryInfo questsDirInfo = new DirectoryInfo (Application.persistentDataPath + "/" + "Quests");
		if (questsDirInfo.Exists)  {
			string questsDirPath = Application.persistentDataPath + "/" + "Quests";
			string[] filesInDirectory = Directory.GetFiles(questsDirPath, "*.xml");
			foreach (string fileInDirectory in filesInDirectory)  {
				File.Delete(fileInDirectory);
			}
		}
		DirectoryInfo savedQuestsDirInfo = new DirectoryInfo (Application.persistentDataPath + "/" + "SavedQuests");
		if (savedQuestsDirInfo.Exists)  {
			string savedQuestsDirPath = Application.persistentDataPath + "/" + "SavedQuests";
			string[] savedQuestFilesInDirectory = Directory.GetFiles(savedQuestsDirPath, "*.xml");
			foreach (string fileInDirectory in savedQuestFilesInDirectory)  {
				File.Delete(fileInDirectory);
			}
		}
		// delete old player data (inventory, health, position)
		DirectoryInfo playerDirInfo = new DirectoryInfo(Application.persistentDataPath + "/" + "Player") ;
		if (playerDirInfo.Exists)  {
			if (System.IO.File.Exists (Application.persistentDataPath + "/Player/Inventory.xml")) {
				File.Delete(Application.persistentDataPath + "/Player/Inventory.xml");
			}
			if (System.IO.File.Exists (Application.persistentDataPath + "/Player/PlayerData.xml")) {
				File.Delete(Application.persistentDataPath + "/Player/PlayerData.xml");
			}
		}
		// delete old map markers
		DirectoryInfo mapDirInfo = new DirectoryInfo(Application.persistentDataPath + "/" + "Map") ;
		if (mapDirInfo.Exists)  {
			if (System.IO.File.Exists (Application.persistentDataPath + "/Map/MapData.xml")) {
				File.Delete(Application.persistentDataPath + "/Map/MapData.xml");
			}
		}
		GamePreferences.SetStartOfGame(1);
		GamePreferences.SetFirstInventory(0);
		GamePreferences.SetFirstInventoryItem (0);
		GamePreferences.SetFirstCombat(0);
		GamePreferences.SetFirstCombatEncounter (0);
		GamePreferences.SetFirstQuest(0);
		GamePreferences.SetFirstQuestEntry (0);
		GamePreferences.SetFirstMap(0);
		GamePreferences.SetFirstMapEntry (0);
		GamePreferences.SetPukiFound (0);
		StartCoroutine (LoadSceneAsynchronously (sceneName));
	}

	public void ContinueGame(string sceneName)  {
		Debug.Log ("You have selected continue game");
		// delete old in-game quests 
		DirectoryInfo questsDirInfo = new DirectoryInfo (Application.persistentDataPath + "/" + "Quests");
		if (questsDirInfo.Exists)  {
			string questsDirPath = Application.persistentDataPath + "/" + "Quests";
			string[] filesInDirectory = Directory.GetFiles(questsDirPath, "*.xml");
			foreach (string fileInDirectory in filesInDirectory)  {
				File.Delete(fileInDirectory);
			}
		}
		// copy any saved quests into the in-game quest directory
		string sourcePath = Application.persistentDataPath + "/" + "SavedQuests";
		string destinationPath = Application.persistentDataPath + "/" + "Quests";
		if (System.IO.Directory.Exists(sourcePath))  {
			string[] files = System.IO.Directory.GetFiles (sourcePath);
			foreach (string s in files)  {
				string fileName = System.IO.Path.GetFileName (s);
				string destFile = System.IO.Path.Combine (destinationPath, fileName);
				System.IO.File.Copy (s, destFile, true);
			}
		}
		StartCoroutine (LoadSceneAsynchronously (sceneName));
	}

	public void Settings()  {
		SceneManager.LoadScene ("01a_SettingsMenu");
	}

	public void QuitGame()  {
		Application.Quit ();
	}

	IEnumerator LoadSceneAsynchronously(string sceneName)  {
		AsyncOperation operation = SceneManager.LoadSceneAsync (sceneName);
		loadingScreen.SetActive (true);
		while(!operation.isDone)  {
			float progress = Mathf.Clamp01 (operation.progress / 0.9f);
			progressSlider.value = progress;
			yield return null;
		}
	}

}
