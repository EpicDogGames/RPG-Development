using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class InventoryDataManager {
	// save inventory items
	[XmlRoot("InventoryDetails")]
	//public class for individual item in inventory
	public class ItemInfo {
		public string inventoryItem;
	}
	// class for holding root data
	public class InventoryDetails {
		// item data
		public List<ItemInfo> II = new List<ItemInfo>();
	}
	//
	public InventoryDetails InvD = new InventoryDetails();
	// save inventory details to XML file
	public void Save(string FileName)  {
		// save inventory details
		XmlSerializer Serializer = new XmlSerializer (typeof(InventoryDetails));
		FileStream Stream = new FileStream (FileName, FileMode.Create);
		Serializer.Serialize (Stream, InvD);
		Stream.Close ();
	}
	// load inventory details from XML file
	public void Load(string FileName)  {
		XmlSerializer Serializer = new XmlSerializer (typeof(InventoryDetails));
		FileStream Stream = new FileStream (FileName, FileMode.Open);
		InvD = Serializer.Deserialize (Stream) as InventoryDetails;
		Stream.Close ();
	}
}
