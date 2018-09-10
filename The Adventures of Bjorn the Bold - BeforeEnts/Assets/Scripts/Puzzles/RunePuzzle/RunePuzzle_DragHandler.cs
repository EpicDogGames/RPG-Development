using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RunePuzzle_DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

	public static GameObject itemBeingDragged;
	private Vector3 startPosition;
	private Transform startParent;

	public void OnBeginDrag(PointerEventData eventData)  {
		itemBeingDragged = gameObject;
		startPosition = transform.position;
		startParent = transform.parent;
		GetComponent<CanvasGroup> ().blocksRaycasts = false;
		transform.SetParent (transform.root);    // new code
	}

	public void OnDrag(PointerEventData eventData)  {
		transform.position = Input.mousePosition;
	}

	public void OnEndDrag(PointerEventData eventData)  {
		itemBeingDragged = null;
		//if (transform.parent == startParent) {
		//	transform.position = startPosition;
		//}
		// start new code ....
		if (transform.parent == startParent || transform.parent == transform.root)  {
			transform.position = startPosition;
			transform.SetParent (startParent);
		}
		// end new code ...
		GetComponent<CanvasGroup> ().blocksRaycasts = true;
	}
}
