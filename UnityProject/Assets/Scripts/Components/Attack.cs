using UnityEngine;
using System.Collections;

public class Attack : MonoBehaviour {
	
	public string attackTag;
	
	private PlayerController _inputController;
	protected Animator _animator;
	protected Player _player;
	// Use this for initialization
	void Start () {
		_inputController = gameObject.GetComponentInParent<PlayerController>();
		_animator = gameObject.GetComponentInParent<Animator>();
		_player = gameObject.GetComponentInParent<Player>();
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
