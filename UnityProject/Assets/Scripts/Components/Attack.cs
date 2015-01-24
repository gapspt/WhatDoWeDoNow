using UnityEngine;
using System.Collections;

public class Attack : MonoBehaviour {
	
	private PlayerController _inputController;
	// Use this for initialization
	void Start () {
		_inputController = gameObject.GetComponentInParent<PlayerController>();
		
		init ();
	}
	
	virtual protected void init()
	{
	}
	
	// Update is called once per frame
	void Update () {
		onUpdate();
		
		if (_inputController.justPressed(PlayerController.ACTIONS.ATTACK_1)){
			onAttackTrigger();
		}
	}
	
	virtual protected void onUpdate()
	{}
	
	virtual public void onAttackTrigger()
	{
	}
}
