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
		//hitbox.enabled = false;
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
		yield return new WaitForSeconds(3);
		counter = 0;
	}
	
	override public void onAttackTrigger()
	{
		/*if (!hitbox.enabled){
			hitbox.enabled = true;
			StopCoroutine("clearAttack");
			StartCoroutine("clearAttack");
		}*/
		
		counter = (counter + 1)%5 + 1;
		if (!_player.isAttacking && counter <= 5){
			//Debug.Log("animation attack"+counter);
			_animator.Play(Animator.StringToHash("attack"+counter));
		}
		
		_player.isAttacking = true;
		
  		StopCoroutine("clearAttackCounter");
		StartCoroutine("clearAttackCounter");
	}
	
	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == attackTag){
			col.gameObject.GetComponent<Player>().kill();
		}
		//Destroy(col.gameObject);
	}
}
