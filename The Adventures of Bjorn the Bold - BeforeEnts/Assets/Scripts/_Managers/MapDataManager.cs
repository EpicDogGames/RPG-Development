using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class MapDataManager {
	// save map locations by name
	[XmlRoot("MapDetails")]
	// public class for map location by name
	public class MapInfo  {
		public string mapItem;
	}
	// class for holding root data
	public class MapDetails {
		// map item
		public List<MapInfo> MI = new List<MapInfo>();
	}
	//
	public MapDetails MapD = new MapDetails();
	// save map locations (by guardian name) to XML file
	public void Save(string FileName)  {
		// save location
		XmlSerializer Serializer = new XmlSerializer (typeof(MapDetails));
		FileStream Stream = new FileStream (FileName, FileMode.Create);
		Serializer.Serialize (Stream, MapD);
		Stream.Close ();
	}
	// load map locations (by guardian name) from XMl file
	public void Load(string FileName)  {
		XmlSerializer Serializer = new XmlSerializer (typeof(MapDetails));
		FileStream Stream = new FileStream (FileName, FileMode.Open);
		MapD = Serializer.Deserialize (Stream) as MapDetails;
		Stream.Close ();
	}
}
