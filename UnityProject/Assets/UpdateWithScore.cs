using UnityEngine;
using System.Collections;

public class UpdateWithScore : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float newX = 0;
		if ((GameManager.getScore()) != 0){
			newX = ((GameManager.getScore()) * 5.94f)/100f; 
		} 
		Debug.Log ("new X = " + newX);
		gameObject.transform.position = new Vector3(newX, gameObject.transform.position.y, gameObject.transform.position.z);
	}
}
