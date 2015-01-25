using UnityEngine;
using System.Collections;

public class ParticleFader : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine("clearParticle");
		StartCoroutine(FadeTo (0, 1f));
	}
	
	private IEnumerator clearParticle() {
		yield return new WaitForSeconds(1);
		Destroy(gameObject);
	}
	
	IEnumerator FadeTo(float aValue, float aTime)
	{
		float alpha = transform.renderer.material.color.a;
		for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
		{
			Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha,aValue,t));
			transform.renderer.material.color = newColor;
			yield return null;
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
}
