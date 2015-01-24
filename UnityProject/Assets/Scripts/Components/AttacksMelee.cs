using UnityEngine;
using System.Collections;

[RequireComponent (typeof (BoxCollider2D))]
public class AttacksMelee : Attack {
	
	public BoxCollider2D hitbox;
	public float duration = 0.1f;
	
	override protected void init()
	{
		hitbox = (BoxCollider2D) gameObject.GetComponent<BoxCollider2D>();
		hitbox.enabled = true;
	}
	
	override protected void onUpdate()
	{
		
	}
	
	private IEnumerator clearAttack(float waitTime) {
		yield return new WaitForSeconds(waitTime);
		hitbox.enabled = false;
	}
	
	override public void onAttackTrigger()
	{
		if (!hitbox.enabled){
			Debug.Log("Attacking for " + duration);
			hitbox.enabled = true;
			StartCoroutine(clearAttack(duration));
		}
	}
	
	void OnTriggerEnter(Collider other) {
		Debug.Log ("on trigger enter");
		Destroy(other.gameObject);
	}
	
	void OnCollisionEnter2D(Collision2D col){
		Debug.Log ("attack collision");
		Destroy(col.gameObject);
	}
	
}
