using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public Transform target;
	public Vector3 offset;

	private float zoomSpeed = 4f;
	private float minZoom = 10f;
	private float maxZoom = 25f;
	private float pitch = 2f;
	private float yawSpeed = 100f;

	private float currentZoom = 10f;
	private float currentYawInput = -43f;

	void Update()  {
		// scrolling mouse wheel in and out sets the zoom level
		if (Input.GetAxisRaw ("Mouse ScrollWheel") != 0 && !EventSystem.current.IsPointerOverGameObject ()) {
			currentZoom -= Input.GetAxis ("Mouse ScrollWheel") * zoomSpeed;
			currentZoom = Mathf.Clamp (currentZoom, minZoom, maxZoom);
		}
		// pushing A or D rotates camera around player with A panning to left and D panning to right
		currentYawInput += Input.GetAxis ("Horizontal") * yawSpeed * Time.deltaTime;
	}

	void LateUpdate () {
		transform.position = target.position - offset * currentZoom;
		transform.LookAt (target.position + Vector3.up * pitch);

		transform.RotateAround (target.position, Vector3.up, currentYawInput);
	}

}
