using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour {

	public GameObject loadingScreen;
	public Slider progressSlider;

	public void PlayGame(string sceneName)  {
		//SceneManager.LoadScene("02_MainScene");
		StartCoroutine(LoadSceneAsynchronously(sceneName));
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
