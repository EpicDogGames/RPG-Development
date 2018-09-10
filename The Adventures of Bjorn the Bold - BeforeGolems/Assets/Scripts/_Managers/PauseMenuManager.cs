using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class PauseMenuManager : MonoBehaviour {

	public static bool GameIsPaused = false;
	public GameObject pauseMenuUI;
	public GameObject settingsMenuUI;
	public GameObject saveGameMenuUI;

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))  {
			if (GameIsPaused)  {
				Resume ();
			}
			else  {
				Pause ();
			}
		}
	}

	public void Resume()  {
		pauseMenuUI.SetActive (false);
		settingsMenuUI.SetActive (false);
		Time.timeScale = 1f;
		GameIsPaused = false;
	}

	void Pause()  {
		pauseMenuUI.SetActive (true);
		Time.timeScale = 0f;
		GameIsPaused = true;
	}

	public void Settings()  {
		settingsMenuUI.SetActive (true);
		Time.timeScale = 0f;
		GameIsPaused = true;
	}

	public void SaveGame()  {
		Debug.Log ("Save the game");
		GameObject.FindGameObjectWithTag ("Player").GetComponent<Player>().SavePlayerData();
		GamePreferences.SetPukiFound (0);
		InventoryController.Instance.CreateInventoryDataXMLFile ();
		MapController.Instance.CreateMapDataXMLFile ();
		// delete old saved quests 
		DirectoryInfo savedQuestsDirInfo = new DirectoryInfo (Application.persistentDataPath + "/" + "SavedQuests");
		if (savedQuestsDirInfo.Exists)  {
			string savedQuestsDirPath = Application.persistentDataPath + "/" + "SavedQuests";
			string[] savedQuestFilesInDirectory = Directory.GetFiles(savedQuestsDirPath, "*.xml");
			foreach (string fileInDirectory in savedQuestFilesInDirectory)  {
				File.Delete(fileInDirectory);
			}
		}
		// copy any in-game quests into the saved quest directory
		string sourcePath = Application.persistentDataPath + "/" + "Quests";
		string destinationPath = Application.persistentDataPath + "/" + "SavedQuests";
		if (System.IO.Directory.Exists(sourcePath))  {
			string[] files = System.IO.Directory.GetFiles (sourcePath);
			foreach (string s in files)  {
				string fileName = System.IO.Path.GetFileName (s);
				string destFile = System.IO.Path.Combine (destinationPath, fileName);
				System.IO.File.Copy (s, destFile, true);
			}
		}
	}

	public void CheckBeforeQuitting()  {
		pauseMenuUI.SetActive (false);
		saveGameMenuUI.SetActive (true);
	}

	public void SaveGameBeforeQuitting()  {
		Debug.Log ("Save the game before quitting");
		GameObject.FindGameObjectWithTag ("Player").GetComponent<Player>().SavePlayerData();
		GamePreferences.SetPukiFound (0);
		InventoryController.Instance.CreateInventoryDataXMLFile ();
		MapController.Instance.CreateMapDataXMLFile ();
		// delete old saved quests 
		DirectoryInfo savedQuestsDirInfo = new DirectoryInfo (Application.persistentDataPath + "/" + "SavedQuests");
		if (savedQuestsDirInfo.Exists)  {
			string savedQuestsDirPath = Application.persistentDataPath + "/" + "SavedQuests";
			string[] savedQuestFilesInDirectory = Directory.GetFiles(savedQuestsDirPath, "*.xml");
			foreach (string fileInDirectory in savedQuestFilesInDirectory)  {
				File.Delete(fileInDirectory);
			}
		}
		// copy any in-game quests into the saved quest directory
		string sourcePath = Application.persistentDataPath + "/" + "Quests";
		string destinationPath = Application.persistentDataPath + "/" + "SavedQuests";
		if (System.IO.Directory.Exists(sourcePath))  {
			string[] files = System.IO.Directory.GetFiles (sourcePath);
			foreach (string s in files)  {
				string fileName = System.IO.Path.GetFileName (s);
				string destFile = System.IO.Path.Combine (destinationPath, fileName);
				System.IO.File.Copy (s, destFile, true);
			}
		}
		Application.Quit ();
	}

	public void QuitGame()  {
		Application.Quit ();
	}
}
