using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCamera : MonoBehaviour {

	public float dampTime = 0.15f;
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

		Vector3 p1_pos = p1.transform.position;
		Vector3 p2_pos = p2.transform.position;
		target = (p2_pos + p1_pos) / 2;

		Vector3 point = cam.WorldToViewportPoint(target);
		Vector3 delta = target - cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
		Vector3 destination = transform.position + delta;
		transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);

	}
}
