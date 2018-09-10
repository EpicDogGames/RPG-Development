using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RunePuzzle_Slot : MonoBehaviour, IDropHandler {

	public GameObject item {
		get {
			if (transform.childCount > 0)  {
				return transform.GetChild (0).gameObject;
			}
			return null;
		}
	}

	public void OnDrop(PointerEventData eventData)  {
		if (!item)  {
			RunePuzzle_DragHandler.itemBeingDragged.transform.SetParent (transform);
			ExecuteEvents.ExecuteHierarchy<IHasChanged> (gameObject, null, (x, y) => x.HasChanged ());
		}
	}
}
