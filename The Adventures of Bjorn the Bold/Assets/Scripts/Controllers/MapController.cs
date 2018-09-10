using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour {

	public static MapController Instance { get; set; }

	[SerializeField] GameObject marker_GuardianOfTheRoad;
	[SerializeField] GameObject marker_GuardianOfTheVillage;
	[SerializeField] GameObject marker_GuardianOfTheSacredForest;
	[SerializeField] GameObject marker_GuardianOfTheGrove;
	[SerializeField] GameObject marker_GuardianOfTheWillows;
	[SerializeField] GameObject marker_GuardianOfTheMysticalWell;
	[SerializeField] GameObject marker_GuardianOfTheChurch;
	[SerializeField] GameObject marker_GuardianOfTheCrossroads;
	[SerializeField] GameObject marker_GuardianOfTheFarm;
	[SerializeField] GameObject marker_GuardianOfTheForest;
	[SerializeField] GameObject marker_GuardianOfTheShield;
	[SerializeField] GameObject marker_GuardianOfTheSword;
	[SerializeField] GameObject marker_GuardianOfTheLair;

	public List<string> mapMarkers = new List<string> ();

	private MapDataManager MDM = null;

	// Use this for initialization
	void Start () {
		if (Instance != null && Instance != this)  {
			Destroy (gameObject);
		}
		else  {
			Instance = this;
		}
		MDM = new MapDataManager ();
		LoadOldMapLocations ();
	}

	void LoadOldMapLocations()  {
		if (System.IO.File.Exists(Application.persistentDataPath + "/Map/MapData.xml"))  {
			MDM.Load (Application.persistentDataPath + "/Map/MapData.xml");
			// restore any locations
			for (int i=0; i<MDM.MapD.MI.Count; i++)  {
				AddGuardianMarkerToMap (MDM.MapD.MI [i].mapItem);
			}
		}
		else  {
			GamePreferences.SetFirstMapEntry(1);
		}
	}
	
	public void AddGuardianMarkerToMap(string npcName)  {
		Debug.Log ("NPC Name: " + npcName);
		// turn on map marker
		if (npcName == "Guardian of the Road")  {
			marker_GuardianOfTheRoad.SetActive (true);
		}
		if (npcName == "Guardian of the Village")  {
			marker_GuardianOfTheVillage.SetActive (true);
		}
		if (npcName == "Guardian of the Sacred Forest")  {
			marker_GuardianOfTheSacredForest.SetActive (true);
		}
		if (npcName == "Guardian of the Grove")  {
			marker_GuardianOfTheGrove.SetActive (true);
		}
		if (npcName == "Guardian of the Willows")  {
			marker_GuardianOfTheWillows.SetActive (true);
		}
		if (npcName == "Guardian of the Well")  {
			marker_GuardianOfTheMysticalWell.SetActive (true);
		}
		if (npcName == "Guardian of the Church")  {
			marker_GuardianOfTheChurch.SetActive (true);
		}
		if (npcName == "Guardian of the Crossroads")  {
			marker_GuardianOfTheCrossroads.SetActive (true);
		}
		if (npcName == "Guardian of the Farm")  {
			marker_GuardianOfTheFarm.SetActive (true);
		}
		if (npcName == "Guardian of the Forest")  {
			marker_GuardianOfTheForest.SetActive (true);
		}
		if (npcName == "Guardian of the Shield")  {
			marker_GuardianOfTheShield.SetActive (true);
		}
		if (npcName == "Guardian of the Sword")  {
			marker_GuardianOfTheSword.SetActive (true);
		}
		if (npcName == "Guardian of the Lair")  {
			marker_GuardianOfTheLair.SetActive (true);
		}
		// this handles calling tutorial popup about map
		if (GamePreferences.GetFirstMapEntry() == 1)  {
			GamePreferences.SetFirstMap (1);
		}
		// determine if this map marker hasn't been added to the list already
		bool mapMarkerInList = false;
		if (mapMarkers.Count != 0) {
			foreach (string marker in mapMarkers) {
				if (marker == npcName) {
					mapMarkerInList = true;
				}
			}
			if (!mapMarkerInList) {
				mapMarkers.Add (npcName);
			}
		} else {
			mapMarkers.Add (npcName);
		}
		Debug.Log ("Number of map markers: " + mapMarkers.Count);
	}

	public void CreateMapDataXMLFile()  {
		// clear map data
		MDM.MapD.MI.Clear ();
		// determine if there are any map markers to be saved
		if (mapMarkers.Count != 0)  {
			foreach (string marker in mapMarkers)  {
				// create map item structure
				MapDataManager.MapInfo MI = new MapDataManager.MapInfo ();
				MI.mapItem = marker;
				MDM.MapD.MI.Add (MI);
			}
			// save map details
			MDM.Save (Application.persistentDataPath + "/Map/MapData.xml");
		}
	}
}
