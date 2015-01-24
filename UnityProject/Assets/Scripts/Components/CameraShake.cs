using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
	private Vector3 originPosition;
	public float shake_decay;
	public float shake_intensity;
	private bool shaking = false;
	
	private Vector3 newPosition;
	/*void OnGUI (){
		if (GUI.Button (new Rect (20,40,80,20), "Shake")){
			Shake ();
		}
	}*/
	
	void Update (){
		if (shaking){
			if (shake_intensity > 0){
				newPosition = originPosition + Random.insideUnitSphere * shake_intensity;
				newPosition.z = originPosition.z;
				transform.position = newPosition;
				shake_intensity -= shake_decay;
			}
			
			if (shake_intensity < 0){
				shaking = false;
				transform.position = originPosition;
			}
			
		} 
	}
	
	public void Shake(){
		originPosition = transform.position;
		shake_intensity = 0.3f;
		shake_decay = 0.02f;
		shaking = true;
	}
}