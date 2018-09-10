using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootTextController : MonoBehaviour {

	private static LootText lootTextPrefab;
	private static GameObject lootCanvas;

	public static void Initialize()  {
		// get reference to canvas
		lootCanvas = GameObject.Find ("Canvas");
		// get reference to object
		if (!lootTextPrefab)  {
			lootTextPrefab = Resources.Load<LootText> ("Loot/LootPopupTextParent");
		}
	}

	public static void CreateLootText(string text, Transform location)  {
		LootText instance = Instantiate(lootTextPrefab);
		Vector2 screenPosition = Camera.main.WorldToScreenPoint(location.position);
		instance.transform.SetParent(lootCanvas.transform, false);
		instance.transform.position = screenPosition;
		instance.SetLootText(text);
	}
}
