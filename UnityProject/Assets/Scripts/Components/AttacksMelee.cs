using UnityEngine;
using System.Collections;

[RequireComponent (typeof (BoxCollider2D))]
public class AttacksMelee : Attack {
	
	public BoxCollider2D hitbox;
	public float duration = 0.1f;
	public int counter = 0;
	
	override protected void init()
	{
		hitbox = (BoxCollider2D) gameObject.GetComponent<BoxCollider2D>();
		hitbox.enabled = false;
		hitbox.isTrigger = true;
	}
	
	override protected void onUpdate()
	{
		
	}
	
	private IEnumerator clearAttack() {
		yield return new WaitForSeconds(duration);
		hitbox.enabled = false;
	}
	
	private IEnumerator clearAttackCounter() {
		yield return new WaitForSeconds(2);
		counter = 0;
		_player.isAttacking = false;
	}
	
	override public void onAttackTrigger()
	{
		if (!hitbox.enabled){
			Debug.Log("Attacking for " + duration);
			hitbox.enabled = true;
			StopCoroutine("clearAttack");
			StartCoroutine("clearAttack");
		}
		
		counter += 1;
		if (counter < 5){
			Debug.Log("animation attack"+counter);
			_animator.Play(Animator.StringToHash("attack"+counter));
		} else {
			_animator.Play(Animator.StringToHash("attack1"));
		}
		
		_player.isAttacking = true;
		
  		StopCoroutine("clearAttackCounter");
		StartCoroutine("clearAttackCounter");
	}
	
	void OnTriggerEnter2D(Collider2D col)
	{
		Destroy(col.gameObject);
	}
	
}
