using UnityEngine;
using System.Collections;

public class ShowBubbleText : MonoBehaviour {
	
	float visibleTimer = 0;
	float elapsed = 0;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.parent.transform.localScale.x == -1 && transform.localScale.x == 1) {
			transform.localScale = new Vector3(-1, 1, 1);
			transform.localPosition = new Vector3(0.48f, 0.17f, 0);
		} else if (transform.parent.transform.localScale.x == 1 && transform.localScale.x == -1){
			transform.localScale = new Vector3(1, 1, 1);
			transform.localPosition = new Vector3(-0.42f, 0.17f, 0);
		}	
		
		if (visibleTimer > 0){
			visibleTimer += Time.deltaTime;
			if (visibleTimer > 2){
				gameObject.GetComponent<SpriteRenderer>().enabled = false;
				visibleTimer = 0;
			}
		} else {
			elapsed += Time.deltaTime;
			if (elapsed > 1){
				float trigger = Random.Range(0,500);
				if (trigger < 50){
					gameObject.GetComponent<SpriteRenderer>().enabled = true;
					visibleTimer = 0.001f;
				}
				elapsed = 0;
			}
		}
	}
}
