using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	public enum TypeOfPlayer
	{
		PLAYER,
		NPC
	}
    public enum TeamOfPlayer
    {
        TOP,
        BOTTOM
    }

    public TypeOfPlayer typeOfPlayer = TypeOfPlayer.PLAYER;
    public TeamOfPlayer teamOfPlayer = TeamOfPlayer.TOP;
	
		// movement config
    public float gravity = -25f;
    public float runSpeed = 8f;
    public float groundDamping = 20f; // how fast do we change direction? higher means faster
    public float inAirDamping = 5f;
    public float jumpHeight = 3f;

    public int jumpCounter = 0;

    [HideInInspector]
    private float normalizedHorizontalSpeed = 0;

    private CharacterController2D _controller;
    private PlayerController _inputController;
    private Animator _animator;
    private RaycastHit2D _lastControllerColliderHit;
    public Vector3 _velocity;
    
    public bool isAttacking = false;
    
    public float health = 100;
    
	public GameObject particleObject;
	
	private bool _doJump = false;
		
  void Awake()
    {
        _animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController2D>();
        _inputController = GetComponent<PlayerController>();

        // listen to some events for illustration purposes
        _controller.onControllerCollidedEvent += onControllerCollider;
        _controller.onTriggerEnterEvent += onTriggerEnterEvent;
        _controller.onTriggerExitEvent += onTriggerExitEvent;
    }

    #region Event Listeners

    void onControllerCollider(RaycastHit2D hit)
    {
        // bail out on plain old ground hits cause they arent very interesting
        if (hit.normal.y == 1f)
            return;

        // logs any collider hits if uncommented. it gets noisy so it is commented out for the demo
        //Debug.Log( "flags: " + _controller.collisionState + ", hit.normal: " + hit.normal );
    }


    void onTriggerEnterEvent(Collider2D col)
    {
        //Debug.Log("onTriggerEnterEvent: " + col.gameObject.name);
    }


    void onTriggerExitEvent(Collider2D col)
    {
        //Debug.Log("onTriggerExitEvent: " + col.gameObject.name);
    }

    #endregion
    
    // Update is called once per frame
    void Update () {
        // grab our current _velocity to use as a base for all calculations
        _velocity = _controller.velocity;

        if (_controller.isGrounded)
            _velocity.y = 0;

        if (_inputController.RIGHT)// pressed(PlayerController.ACTIONS.RIGHT))//if (Input.GetKey(KeyCode.RightArrow))
        {
            normalizedHorizontalSpeed = 1;
            if (transform.localScale.x < 0f)
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

            if (_controller.isGrounded && !isAttacking)
                _animator.Play(Animator.StringToHash("Run"));
        }
        else if (_inputController.LEFT)// pressed(PlayerController.ACTIONS.LEFT))//if (Input.GetKey(KeyCode.LeftArrow))
        {
            normalizedHorizontalSpeed = -1;
            if (transform.localScale.x > 0f)
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

			if (_controller.isGrounded && !isAttacking)
                _animator.Play(Animator.StringToHash("Run"));
        }
        else
        {
            normalizedHorizontalSpeed = 0;

            if (_controller.isGrounded)
                
                if (_inputController.UP)// pressed(PlayerController.ACTIONS.UP))
                {
                    _animator.Play(Animator.StringToHash("LookUp"));
                }
                else
                {
					if (!isAttacking){
                    	_animator.Play(Animator.StringToHash("idle_1"));
                    }
                }
        }

        // we can only jump whilst grounded
        if (_doJump || (_controller.isGrounded && _inputController.justPressed(PlayerController.ACTIONS.JUMP)))
        {
			_velocity.y = Mathf.Sqrt(2f * jumpHeight * -gravity);
			//_animator.Play(Animator.StringToHash("Jump"));
			_doJump = false;
        }

        // apply horizontal speed smoothing it
        var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
        _velocity.x = Mathf.Lerp(_velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime * smoothedMovementFactor);

        // apply gravity before moving
        _velocity.y += gravity * Time.deltaTime;

        _controller.move(_velocity * Time.deltaTime);
	}
	
	
	public void doJump()
	{
		_doJump = true;
	}
	
	
	void OnTriggerEnter(Collider other) {
		Debug.Log ("on trigger enter parent");
		
	}
	
	void OnCollisionEnter2D(Collision2D col){
		Debug.Log ("attack collision parent");
	}
	
	public void shakeCamera()
	{
		Camera.main.GetComponent<CameraShake>().Shake();
		
		if (particleObject == null) return;
		
		float dirX;
		float dirY;
		GameObject particle;
		for(int i=0;i<100;i++){
			dirX = Random.Range(-1.0f, 1.0f) * 200;
			dirY = Random.Range(0.0f, 1.0f) * 500;
			particle = ((GameObject) Instantiate(particleObject, transform.position, Quaternion.identity));
			particle.GetComponent<Rigidbody2D>().AddForce(new Vector2(dirX, dirY));
			
		}
	}
	
	public void attack(float damage)
	{
		health -= damage;
		if (health <= 0){
			kill();
		}
	}
	
	public void kill()
	{
		if (typeOfPlayer == TypeOfPlayer.NPC)
		{
            if (teamOfPlayer == TeamOfPlayer.TOP)
            {
                GameManager.Instance.KilledTopNPC(gameObject);
            }
            else if (teamOfPlayer == TeamOfPlayer.BOTTOM)
            {
                GameManager.Instance.KilledBottomNPC(gameObject);
            }

			Destroy(gameObject);
		}
	}

}
