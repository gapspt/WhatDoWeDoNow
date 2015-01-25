using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
	void Update () {
		if (Input.anyKey) {
			Application.LoadLevel (1);
		}
	}
}
