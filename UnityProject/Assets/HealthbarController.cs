using UnityEngine;
using System.Collections;

public class HealthbarController : MonoBehaviour {
	
	private Player playerInstance;
	// Use this for initialization
	void Start () {
	}
	
	void Awake() {
		if (playerInstance == null)
		{
			playerInstance = gameObject.GetComponentInParent<Player>();
		}
	}
	// Update is called once per frame
	void Update () {
		if (playerInstance != null){
			if (playerInstance.health <= 0){
				transform.localScale =  new Vector3(0,1,1);
			} else {
				transform.localScale =  new Vector3( playerInstance.health / 100 ,1,1);
			}
		}
	}
}
