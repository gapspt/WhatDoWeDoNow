using UnityEngine;
using System.Collections;

[RequireComponent (typeof (BoxCollider2D))]
public class AttacksMelee : Attack {
	
	public BoxCollider2D hitbox;
	public float duration = 0.1f;
	public int counter = 0;
	
	public float attackDamage = 40;
	
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
		yield return new WaitForSeconds(0.5f);
		Debug.Log ("clearAttackCounter");
		counter = 0;
	}
	
	override public void onAttackTrigger()
	{	
		if (!_player.isAttacking){
			counter = (counter + 1)%3;	
			if (counter == 2) {
				_animator.Play(Animator.StringToHash("attack5"));
			}else {
				_animator.Play(Animator.StringToHash("attack1"));
			}
		}
		/*if (!_player.isAttacking && counter <= 5){
			Debug.Log("animation attack"+counter);
			_animator.Play(Animator.StringToHash("attack"+counter));
		}*/
		
		_player.isAttacking = true;
		
  		StopCoroutine("clearAttackCounter");
		StartCoroutine("clearAttackCounter");
	}
	
	void OnTriggerEnter2D(Collider2D col)
	{
   	    //Debug.Log ("colliding with " + col.gameObject.name + " tag:" + col.gameObject.tag + " attack:" + attackTag);
		if (col.gameObject.tag == attackTag){
            Player player = col.gameObject.GetComponent<Player>();
            if (player)
            {
            	if (player.typeOfPlayer == Player.TypeOfPlayer.PLAYER){
            		if (player.tag == "angel"){
            			GameManager.heavenScores();
            		} else {
						GameManager.hellScores();
            		}
            	}
                player.attack(attackDamage);
            }
		}
	}
}
