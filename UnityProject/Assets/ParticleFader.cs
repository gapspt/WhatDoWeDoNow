using UnityEngine;
using System.Collections;

public class ParticleFader : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine("clearParticle");
	}
	
	private IEnumerator clearParticle() {
		yield return new WaitForSeconds(3);
		Destroy(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
