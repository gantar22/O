using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCamera : MonoBehaviour {

											// ZOOM SETTINGS
	private float dampTime = 0.01f; 		// How fast the camera should move to center itself
	private float zoomSpeed = 0.01f; 		// How fast the camera should zoom in and out
	private float lowerZoomLimit = 0.3f; 	// How close together the players have to be before the camera starts zooming in
	private float upperZoomLimit = 0.8f;	// How far apart the players have to be before the camera starts zooming out
	private float minSize = 17;				// Minimum orthographic size for the camera, so it doesn't zoom in super close

	private Vector3 velocity = Vector3.zero;
	private Camera cam;
	private Vector3 target;

	private GameObject p1, p2;

	void Start() {
		cam = GetComponent<Camera>();
	}

	// Update is called once per frame
	void Update () 
	{
		if (p1 == null)
			p1 = GameObject.Find ("Player 1(Clone)");
		if (p2 == null)
			p2 = GameObject.Find ("Player 2(Clone)");

		// Setting target position to be centered between players
		Vector3 p1_pos = p1.transform.position;
		Vector3 p2_pos = p2.transform.position;
		target = (p2_pos + p1_pos) / 2;

		Vector3 point = cam.WorldToViewportPoint(target);
		Vector3 delta = target - cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
		Vector3 destination = transform.position + delta;

		// Setting target z position to fit both players in screen
		Vector3 p1_Vpos = cam.WorldToViewportPoint (p1_pos);
		Vector3 p2_Vpos = cam.WorldToViewportPoint (p2_pos);
		float p1_x = 2.0f*Mathf.Abs (p1_Vpos.x - 0.5f);
		float p1_y = 2.0f*Mathf.Abs (p1_Vpos.y - 0.5f);
		float p2_x = 2.0f*Mathf.Abs (p2_Vpos.x - 0.5f);
		float p2_y = 2.0f*Mathf.Abs (p2_Vpos.y - 0.5f);

		if (p1_x > upperZoomLimit || p1_y > upperZoomLimit || p2_x > upperZoomLimit || p2_y > upperZoomLimit) {
			cam.orthographicSize += zoomSpeed;
		} else if (cam.orthographicSize > minSize && (p1_x < lowerZoomLimit && p2_x < lowerZoomLimit && p1_y < lowerZoomLimit && p2_y < lowerZoomLimit)) {
			cam.orthographicSize -= zoomSpeed;
		}

		transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);

	}
}
