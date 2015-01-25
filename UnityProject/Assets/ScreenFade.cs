using UnityEngine;
using System.Collections;
using UiImage = UnityEngine.UI.Image;

public class ScreenFade : MonoBehaviour
{
	void Update ()
	{
		GetComponent<UiImage> ().color = new Color (0, 0, 0, Mathf.Cos (Time.time));
	}
}
