using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class QuestDataManager {
	// save quest details
	[XmlRoot("QuestDetails")]
	// public class for goal within a quest
	public class GoalInfo  {
		public string gType;
		public string gTarget;
		public string gDescription;
		public int gCurrentAmount;
		public int gRequiredAmount;
		public bool gCompleted;		
	}
	// public class for individual quest
	public class QuestInfo  {
		public string qName;
		public string qDescription;
		public string qImageName;
		public bool qCompleted;
	}
	// Class for holding root data
	public class QuestDetails {
		// goal data
		public List<GoalInfo> GI = new List<GoalInfo>();
		// quest data
		public QuestInfo QI = new QuestInfo();
	}
	//
	public QuestDetails QD = new QuestDetails();
	// saves quest details to XML file
	public void Save(string FileName)  {
		// save quest details
		XmlSerializer Serializer = new XmlSerializer (typeof(QuestDetails));
		FileStream Stream = new FileStream (FileName, FileMode.Create);
		Serializer.Serialize (Stream, QD);
		Stream.Close ();
	}
	// load quest details from XML file
	public void Load(string FileName)  {
		XmlSerializer Serializer = new XmlSerializer (typeof(QuestDetails));
		FileStream Stream = new FileStream (FileName, FileMode.Open);
		QD = Serializer.Deserialize (Stream) as QuestDetails;
		Stream.Close ();
	}

}
