using System.Collections;
using UnityEngine;

public class GamePreferences {

	// PlayerPrefs are found uisng RegEdit
	// for compiled game:
	//  HKEY_CURRENT_USER/Software/<company name>/<game name>
	//  company name and game name are set in Unity's Build Menu under play settings
	// from UnityEditor:
	//  HKEY_CURRENT_USER/Software/Unity/<company name>/<game name>

	//general game options ... audio
	public static string IsMusicOn = "IsMusicOn";								// 0 = false; 1 = true
	public static string MusicVolume = "Music Volume";							// value from 0 to 1
	public static string IsFXOn = "IsFXOn";										// 0 = false; 1 = true
	public static string FXVolume = "FXVolume";									// value from 0 to 1

	// general game options ... graphics
	public static string IsFullscreenOn = "IsFullscreenOn";						// 0 = false; 1 = true
	public static string ScreenResolution = "ScreenResolution";					// 0 = 1280x720, 1 = 1600x900, 2 = 1920x1080
	public static string QualitySetting = "QualitySetting";						// 0 = Low, 1 = Medium, 2 = High, 3 = Ultra

	// tutorial popups
	public static string IsStartOfGame = "IsStartOfGame";						// 0 = false; 1 = true
	public static string IsFirstInventory = "IsFirstInventory";   				// 0 = false; 1 = true
	public static string IsFirstInventoryItem = "IsFirstInventoryItem";			// 0 = false; 1 = true
	public static string IsFirstCombat = "IsFirstCombat";						// 0 = false; 1 = true
	public static string IsFirstCombatEncounter = "IsFirstCombatEncounter";		// 0 = false; 1 = true
	public static string IsFirstQuest = "IsFirstQuest";							// 0 = false; 1 = true
	public static string IsFirstQuestEntry = "IsFirstQuestEntry";				// 0 = false; 1 = true

	// map locations
	public static string IsFirstMap = "IsFirstMap";								// 0 = false; 1 = true
	public static string IsFirstMapEntry = "IsFirstMapEntry";					// 0 = false; 1 = true

	// player enhancements
	public static string IsPukiFound = "IsPukiFound";							// 0 = false; 1 = true
	public static string IsMagicShieldAvailable = "IsMagicShieldAvailable";		// 0 = false; 1 = true
	public static string IsFirePowerAvailable = "IsFirePowerAvailable";			// 0 = false; 1 = true

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

	//---------------------------------------------------------------------------------------
	// Tutorial Popups
	//---------------------------------------------------------------------------------------
	// set and get StartOfGame
	public static void SetStartOfGame(int state)  {
		PlayerPrefs.SetInt (GamePreferences.IsStartOfGame, state);
	}
	public static int GetStartOfGame()  {
		return PlayerPrefs.GetInt (GamePreferences.IsStartOfGame);
	}
	// set and get first inventory
	public static void SetFirstInventory(int state)  {
		PlayerPrefs.SetInt (GamePreferences.IsFirstInventory, state);
	}
	public static int GetFirstInventory()  {
		return PlayerPrefs.GetInt (GamePreferences.IsFirstInventory);
	}
	// set and get first item added to inventory
	public static void SetFirstInventoryItem(int state)  {
		PlayerPrefs.SetInt (GamePreferences.IsFirstInventoryItem, state);
	}
	public static int GetFirstInventoryItem()  {
		return PlayerPrefs.GetInt (GamePreferences.IsFirstInventoryItem);
	}
	// set and get first combat
	public static void SetFirstCombat(int state)  {
		PlayerPrefs.SetInt (GamePreferences.IsFirstCombat, state);
	}
	public static int GetFirstCombat() {
		return PlayerPrefs.GetInt (GamePreferences.IsFirstCombat);
	}
	// set and get first combat encounter
	public static void SetFirstCombatEncounter(int state)  {
		PlayerPrefs.SetInt (GamePreferences.IsFirstCombatEncounter, state);
	}
	public static int GetFirstCombatEncounter() {
		return PlayerPrefs.GetInt (GamePreferences.IsFirstCombatEncounter);
	}
	// set and get first quest
	public static void SetFirstQuest(int state)  {
		PlayerPrefs.SetInt (GamePreferences.IsFirstQuest, state);
	}
	public static int GetFirstQuest()  {
		return PlayerPrefs.GetInt (GamePreferences.IsFirstQuest);
	}
	// set and get first quest entry
	public static void SetFirstQuestEntry(int state)  {
		PlayerPrefs.SetInt (GamePreferences.IsFirstQuestEntry, state);
	}
	public static int GetFirstQuestEntry()  {
		return PlayerPrefs.GetInt (GamePreferences.IsFirstQuestEntry);
	}

	//---------------------------------------------------------------------------------------
	// Map Locations
	//---------------------------------------------------------------------------------------
	// set and get first map
	public static void SetFirstMap(int state)  {
		PlayerPrefs.SetInt (GamePreferences.IsFirstMap, state);
	}
	public static int GetFirstMap()  {
		return PlayerPrefs.GetInt (GamePreferences.IsFirstMap);
	}
	// set and get first map entry
	public static void SetFirstMapEntry(int state)  {
		PlayerPrefs.SetInt (GamePreferences.IsFirstMapEntry, state);
	}
	public static int GetFirstMapEntry()  {
		return PlayerPrefs.GetInt (GamePreferences.IsFirstMapEntry);
	}

	//----------------------------------------------------------------------------------------
	// Player Enhancements
	//----------------------------------------------------------------------------------------
	// set and get puki
	public static void SetPukiFound(int state)  {
		PlayerPrefs.SetInt (GamePreferences.IsPukiFound, state);
	}
	public static int GetPukiFound()  {
		return PlayerPrefs.GetInt (GamePreferences.IsPukiFound);
	}
	// set and get magic shield for puki
	public static void SetMagicShieldForPuki(int state)  {
		PlayerPrefs.SetInt (GamePreferences.IsMagicShieldAvailable, state);
	}
	public static int GetMagicShieldForPuki()  {
		return PlayerPrefs.GetInt (GamePreferences.IsMagicShieldAvailable);
	}
	// set and get fire power for puki
	public static void SetFirePowerForPuki(int state)  {
		PlayerPrefs.SetInt (GamePreferences.IsFirePowerAvailable, state);
	}
	public static int GetFirePowerForPuki()  {
		return PlayerPrefs.GetInt (GamePreferences.IsFirePowerAvailable);
	}
}
