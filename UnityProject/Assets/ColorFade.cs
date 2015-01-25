using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ColorFade : MonoBehaviour
{
	void Update ()
	{
		GetComponent<Text> ().color = new Color (1 - (Mathf.Cos (Time.time * 0.5f + Mathf.PI / 2) + 1) / 2,
		                                        0.3f,
		                                        (Mathf.Cos (Time.time * 0.5f + Mathf.PI / 2) + 1) / 2,
		                                        1.0f);
	}
}
