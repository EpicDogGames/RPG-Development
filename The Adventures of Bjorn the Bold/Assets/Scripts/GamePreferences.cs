using System.Collections;
using UnityEngine;

public class GamePreferences {

	// PlayerPrefs are found uisng RegEdit
	//  HKEY_CURRENT_USER/Software/<company name>/<game name>
	//  company name and game name are set in Unity's Build Menu udner play settings

	//general game options ... audio
	public static string IsMusicOn = "IsMusicOn";				// 0 = false; 1 = true
	public static string MusicVolume = "Music Volume";			// value from 0 to 1
	public static string IsFXOn = "IsFXOn";						// 0 = false; 1 = true
	public static string FXVolume = "FXVolume";					// value from 0 to 1

	// general game options ... graphics
	public static string IsFullscreenOn = "IsFullscreenOn";		// 0 = false; 1 = true
	public static string ScreenResolution = "ScreenResolution";	// 0 = 960x640, 1 = 1280x720, 2 = 1600x900, 3 = 1920x1080
	public static string QualitySetting = "QualitySetting";		// 0 = Low, 1 = Medium, 2 = High, 3 = Ultra

	//--------------------------------------------------------------------------------------
	// Audio Settings
	//--------------------------------------------------------------------------------------
	// set and get music state
	public static void SetMusicState(int state)  {
		PlayerPrefs.SetInt (GamePreferences.IsMusicOn, state);
	}
	public static int GetMusicState()  {
		return PlayerPrefs.GetInt (GamePreferences.IsMusicOn);
	}	
	// set and get music volume
	public static void SetMusicVolume(float volume)  {
		PlayerPrefs.SetFloat (GamePreferences.MusicVolume, volume);
	}
	public static float GetMusicVolume()  {
		return PlayerPrefs.GetFloat (GamePreferences.MusicVolume);
	}

	// set and get fx state
	public static void SetFXState(int state)  {
		PlayerPrefs.SetInt (GamePreferences.IsFXOn, state);
	}
	public static int GetFXState()  {
		return PlayerPrefs.GetInt (GamePreferences.IsFXOn);
	}	
	// set and get fx volume
	public static void SetFXVolume(float volume)  {
		PlayerPrefs.SetFloat (GamePreferences.FXVolume, volume);
	}
	public static float GetFXVolume()  {
		return PlayerPrefs.GetFloat (GamePreferences.FXVolume);
	}

	//--------------------------------------------------------------------------------------
	// Graphics Settings
	//--------------------------------------------------------------------------------------
	// set and get fullscreen state
	public static void SetFullscreenState(int state)  {
		PlayerPrefs.SetInt (GamePreferences.IsFullscreenOn, state);
	}
	public static int GetFullscreenState()  {
		return PlayerPrefs.GetInt (GamePreferences.IsFullscreenOn);
	}
	// set and get screen resolution
	public static void SetScreenResolution(int resolutionIndex)  {
		PlayerPrefs.SetInt (GamePreferences.ScreenResolution, resolutionIndex);
	}
	public static int GetScreenResolution()  {
		return PlayerPrefs.GetInt (GamePreferences.ScreenResolution);
	}
	// set and get quality setting
	public static void SetQualitySetting(int qualityIndex)  {
		PlayerPrefs.SetInt (GamePreferences.QualitySetting, qualityIndex);
	}
	public static int GetQualitySetting()  {
		return PlayerPrefs.GetInt (GamePreferences.QualitySetting);
	}
}
