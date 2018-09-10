using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {

	// this script is called from SplashScreen since this scene only happens once

	public static MusicManager Instance;
	public AudioClip[] levelMusicChangeArray;

	private AudioSource audioSource;

	void Awake () {
		MakeSingleton ();
		audioSource = GetComponent<AudioSource> ();
	}

	void MakeSingleton()  {
		if (Instance != null)  {
			Destroy (gameObject);
		}
		else  {
			Instance = this;
			DontDestroyOnLoad (gameObject);
		}
	}

	void OnLevelWasLoaded(int level)  {
		AudioClip thisLevelMusic = levelMusicChangeArray [level];
		if (thisLevelMusic)  {
			audioSource.clip = thisLevelMusic;
			audioSource.loop = true;
			if (GamePreferences.GetMusicState () == 1) {
				audioSource.Play ();
				audioSource.volume = GamePreferences.GetMusicVolume ();
			}
		}
	}

	public void PlayMusic(bool play)  {
		if (play)  {
			if (!audioSource.isPlaying)  {
				audioSource.Play ();
			}
		}
		else  {
			if (audioSource.isPlaying)  {
				audioSource.Stop ();
			}
		}
	}

	public void ChangeVolume(float volume)  {
		audioSource.volume = volume;
	}

}