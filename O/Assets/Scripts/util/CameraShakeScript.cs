using UnityEngine;
using System.Collections;

public class CameraShakeScript : MonoBehaviour {

	private IEnumerator coroutine;
	private bool running;
	private bool active;
	private float elapsedTime;
	[SerializeField]
	private float magnitude;
	[SerializeField]
	private float duration;

	// Use this for initialization
	void Start () {
	coroutine = shake();
	running = false;
	active = false;
	}
	
	IEnumerator shake(){
			elapsedTime = 0f;
			Vector3 originalPos = new Vector3(transform.position.x,transform.position.y,transform.position.z);

			while(elapsedTime < duration){

				elapsedTime += Time.deltaTime;
				float percentComplete = elapsedTime / duration;
				float damper = 1.0f - Mathf.Clamp(4.0f * percentComplete - 3.0f, 0.0f,1.0f);

				float PosX = Random.value * 2.0f - 1.0f;
				float PosY = Random.value * 2.0f - 1.0f;

				PosX *= magnitude * damper * damper;
				PosY *= magnitude * damper * damper;
				transform.position = new Vector3(originalPos.x + PosX, originalPos.y  + PosY,originalPos.z);

				//transform.Translate(PosX,PosY,originalPos.z);
				yield return null;


			}


			transform.position = originalPos;
		
	}

	public void activate(float mag,float dur){
		magnitude = mag;
		duration = dur;
		coroutine = shake();
		StartCoroutine(coroutine);

	}


	// Update is called once per frame
	void Update () {

	

	}
}
