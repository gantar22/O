using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class transition : MonoBehaviour {
	private float size;
	private float x;
	private IEnumerator move;
	private Camera cam;

	public void activate(float new_size, float new_x){
		size = new_size;
		x = new_x;
		StartCoroutine(move);
	}

	void Start(){
		move = corou();
		cam  = gameObject.GetComponent<Camera>();
	}

	IEnumerator corou(){
		float elapsed = 0f;
		float oldSize = cam.orthographicSize;
		float oldX = transform.position.x;
		Vector3 velo = Vector3.zero;
		while(elapsed < 3.8f){
			Vector3 dest = new Vector3(x,40,transform.position.z);
			transform.position = Vector3.SmoothDamp(transform.position,dest,ref velo,4);
			cam.orthographicSize = Mathf.Lerp(oldSize,size,elapsed / 4);
			yield return null;
		}
	}
}
