using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class PlayerDataManager {

	[XmlRoot("GameData")]
	// how to define a player's position
	public struct DataTransform
	{
		public float X;
		public float Y;
		public float Z;
		public float RotX;
		public float RotY;
		public float RotZ;
		public float ScaleX;
		public float ScaleY;
		public float ScaleZ;
	}
	// class for holding player position as well as current health and mana levels
	public class PlayerData {
		public DataTransform playerTransform;
		public int playerCurrentHealth;
		public int playerCurrentMana;
	}
	// class for holding root data
	public class GameData {
		public PlayerData PD = new PlayerData();
	}
	//
	public GameData GD = new GameData();
	// saves player data to XML file
	public void Save(string FileName)  {
		// save quest details
		XmlSerializer Serializer = new XmlSerializer (typeof(GameData));
		FileStream Stream = new FileStream (FileName, FileMode.Create);
		Serializer.Serialize (Stream, GD);
		Stream.Close ();
	}
	// load quest details from XML file
	public void Load(string FileName)  {
		XmlSerializer Serializer = new XmlSerializer (typeof(GameData));
		FileStream Stream = new FileStream (FileName, FileMode.Open);
		GD = Serializer.Deserialize (Stream) as GameData;
		Stream.Close ();
	}

}
