using Game.Tool;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Move
{
	public class PlayerController : MonoBehaviour
	{
		[SerializeField] private float _speed;

        [Header("Camera")]
        [SerializeField] public Transform _followPointHorizontal;
        [SerializeField] public Transform _followPointVertical;

		[Header("Jump")]
		[SerializeField] private float _jumpForce;
		[SerializeField] private float _fallMutiplier;
		[SerializeField] private Transform _groundCheck;
		[SerializeField] private float _groundCheckRadius;
		[SerializeField] private LayerMask _whatIsGround;
		[SerializeField] private int _extraJumpCount = 0;
		[SerializeField] private GameObject _jumpEffect;

		[Header("Dash")]
		[SerializeField] private float _dashSpeed = 30f;
		[Tooltip("Dash Duration")]
		[SerializeField] private float _dashDuration = 0.1f;
		[Tooltip("Dash ColdTime")]
		[SerializeField] private float _dashColdDuration = 0.2f;
		[SerializeField] private GameObject _dashEffect;

        [Header("Wall Grab Jump")]
        [Tooltip("Wall Detection Offset")]
        [SerializeField] private Vector2 _grabRightOffset = new Vector2(0.16f, 0f);
        [SerializeField] private Vector2 _grabLeftOffset = new Vector2(-0.16f, 0f);
        [SerializeField] private float _grabCheckRadius = 0.24f;
        [SerializeField] private float _slideSpeed = 2.5f;
        [SerializeField] private Vector2 _wallJumpForce = new Vector2(10.5f, 18f);
        [SerializeField] private Vector2 _wallClimbForce = new Vector2(4f, 14f);

        #region Ability
        private bool _canSwim;
        private bool _canDiveWater;

        private bool _canFly;
        private bool _canJumpFar;

        private bool _canBeTiny;

        private bool _canClimb;
        private bool _canJumpFromHighPoint;

        private bool _canDig;
        private bool _canNightVision;
        #endregion



        // controls whether this instance is currently playable or not

        private bool _isGrounded;
		private float _moveInput;
        private float _flyInput;

		private bool _isDashing = false;

		private Animator _animator;
		private Rigidbody2D _rb2D;
        private Collider2D _collider2D;
		private ParticleSystem _dustParticle;
		private bool _facingRight = false;

		private readonly float _groundedRememberDuration = 0.03f;
		private float _groundedRememberTime = 0f;

        private int _extraJumps;
		private float _extraJumpForce;

		private float _dashTime;
		private bool _hasDashedInAir = false;

		private bool _onWall = false;
		private bool _onRightWall = false;
		private bool _onLeftWall = false;

		private bool _wallGrabbing = false;

		private readonly float _wallStickDuration = 0.25f;
		private float _wallStickTime = 0f;

		private bool _wallJumping = false;
		private float _dashColdTime;

        private readonly float _footStepDuration = 0.25f;
        private float _footStepTime = 0f;

		private bool _turnCheck => (_facingRight && _moveInput <= 0) || (!_facingRight && _moveInput >= 0);

		// 0 -> none, 1 -> right, -1 -> left
		private int _onWallSide = 0;
		private int _playerSide = 1;

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _rb2D = GetComponent<Rigidbody2D>();
            _collider2D = GetComponent<Collider2D>();
            _dustParticle = GetComponentInChildren<ParticleSystem>();
        }
        private void OnEnable()
        {
            GameEventManager.MainInstance.AddEventListener<AnimalsSO,Transform>("UpdateParameters", UpdateParameters);
        }
        private void OnDisable()
        {
            GameEventManager.MainInstance.RemoveEvent<AnimalsSO,Transform>("UpdateParameters", UpdateParameters);
        }
        void Start()
		{
			// create pools for particles
			//PoolManager.instance.CreatePool(dashEffect, 2);
			//PoolManager.instance.CreatePool(jumpEffect, 2);

			// if it's the player, make this instance currently playable
			_extraJumps = _extraJumpCount;
			_dashTime = _dashDuration;
			_dashColdTime = _dashColdDuration;
			_extraJumpForce = _jumpForce * 0.7f;
		}

		private void FixedUpdate()
		{
            CameraFollowing();
            Flip();
            if (!_canFly)
            {
                // check if grounded
                CheckGround();
                CheckOnWall();
                CalculateSides();
                CheckWallJump();
                UpdateHorizontalMove();
                FixVelocityForJump();
                UpdateDash();
                UpdateWallGrab();
            }
            else
            {
                UpdateFlyMove();
            }

				// enable/disable dust particles
				/*
				float playerVelocityMag = _rb2D.velocity.sqrMagnitude;
				if(m_dustParticle.isPlaying && playerVelocityMag == 0f)
				{
					m_dustParticle.Stop();
				}
				else if(!m_dustParticle.isPlaying && playerVelocityMag > 0f)
				{
					m_dustParticle.Play();
				}*/		
        }

		private void Update()
		{
            if(_canSwim)
            WhenSwim();
            // horizontal input
            UpdateAnimation();
            _moveInput = GameInputManager.MainInstance.Horizontal;
            if (!_canFly)
            {
                if (!_turnCheck)
                    SetCharacterFootSound();
                ResetJump();
                UpdateGroundOffset();
                DashAll();
                JumpInput();
            }
            else
            {
                _flyInput = GameInputManager.MainInstance.Vertical;
            }
		}
        void WhenSwim()
        {
            _canFly = Physics2D.OverlapPoint(transform.position, 1 << 4) != null;
        }

        void CheckGround()
        {
            _isGrounded = Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, _whatIsGround);
        }
        void CheckOnWall()
        {
            var position = transform.position;
            _onWall = Physics2D.OverlapCircle((Vector2)position + _grabRightOffset, _grabCheckRadius, _whatIsGround)
                      || Physics2D.OverlapCircle((Vector2)position + _grabLeftOffset, _grabCheckRadius, _whatIsGround);
            _onRightWall = Physics2D.OverlapCircle((Vector2)position + _grabRightOffset, _grabCheckRadius, _whatIsGround);
            _onLeftWall = Physics2D.OverlapCircle((Vector2)position + _grabLeftOffset, _grabCheckRadius, _whatIsGround);
        }
        void UpdateHorizontalMove()
        {
            if (_wallJumping)
            {
                _rb2D.velocity = Vector2.Lerp(_rb2D.velocity, (new Vector2(_moveInput * _speed, _rb2D.velocity.y)), 1.5f * Time.fixedDeltaTime);
            }
            else
            {
                if (!_canClimb && _onWall)
                {
                    if (_onLeftWall)
                    {
                        _rb2D.velocity = new Vector2(Mathf.Clamp(_moveInput * _speed, 0,float.PositiveInfinity), _rb2D.velocity.y);
                    }
                    else if (_onRightWall)
                    {
                        _rb2D.velocity = new Vector2(Mathf.Clamp(_moveInput * _speed, float.NegativeInfinity, 0), _rb2D.velocity.y);
                    }
                }
                else
                {
                    _rb2D.velocity = new Vector2(_moveInput*_speed, _rb2D.velocity.y);
                }
                    
            }

        }
        void CheckWallJump()
        {
            if ((_wallGrabbing || _isGrounded) && _wallJumping)
            {
                _wallJumping = false;
            }
        }
        void FixVelocityForJump()
        {
            if (_rb2D.velocity.y < 0f)
            {
                _rb2D.velocity += Vector2.up * Physics2D.gravity.y * (_fallMutiplier - 1) * Time.fixedDeltaTime;
            }
        }
		void UpdateDash()
		{
            if (_isDashing)
            {
                if (_dashTime <= 0f)
                {
                    _isDashing = false;
                    _dashColdTime = _dashColdDuration;
                    _dashTime = _dashDuration;
                    _rb2D.velocity = Vector2.zero;
                }
                else
                {
                    _dashTime -= Time.deltaTime;
                    if (_facingRight)
                        _rb2D.velocity = Vector2.right * _dashSpeed;
                    else
                        _rb2D.velocity = Vector2.left * _dashSpeed;
                }
            }
        }
		void UpdateWallGrab()
		{  
            if (_onWall && !_isGrounded && _rb2D.velocity.y <= 0f && _playerSide == _onWallSide)
            {    
                _wallGrabbing = true;
                if(_canClimb)
                    _rb2D.velocity = new Vector2(_moveInput * _speed, -_slideSpeed);
                else
                    _rb2D.velocity = new Vector2(0, -_slideSpeed);
                _wallStickTime = _wallStickDuration;
            }
            else
            {
                _wallStickTime -= Time.deltaTime;
                if (_wallStickTime <= 0f)
                    _wallGrabbing = false;
            }
            if (_wallGrabbing && _isGrounded)
                _wallGrabbing = false;
        }
		void CameraFollowing()
		{
            GameEventManager.MainInstance.CallEvent("SwitchCamera", _rb2D.velocity.y);
            GameEventManager.MainInstance.CallEvent("LerpYDamping", _rb2D.velocity.y);
        }
        void PlayFootSound()
        {
            GamePoolManager.MainInstance.TryGetPoolItem("FootSound", transform.position, Quaternion.identity);
            _footStepTime = _footStepDuration;
        }
        void SetCharacterFootSound()
        {
            if (_isGrounded && _animator.AnimationAtTag("Motion")&&!_turnCheck)
            {
                _footStepTime -= Time.deltaTime;
                if (_footStepTime < 0f)
                {
                    PlayFootSound();
                }
            }
            else
            {
                _footStepTime = 0f;
            }
        }
		void ResetJump()
		{
            if (_isGrounded)
            {
                _extraJumps = _extraJumpCount;
            }
        }
		void JumpInput()
		{
            // Jumping
            if (GameInputManager.MainInstance.Jump && _extraJumps > 0 && !_isGrounded && !_wallGrabbing)    // extra jumping
            {
                _rb2D.velocity = new Vector2(_rb2D.velocity.x, _extraJumpForce);
                _extraJumps--;
                // jumpEffect
                //PoolManager.instance.ReuseObject(jumpEffect, groundCheck.position, Quaternion.identity);
            }
            else if (GameInputManager.MainInstance.Jump && (_isGrounded || _groundedRememberTime > 0f)) // normal single jumping
            {
                _rb2D.velocity = new Vector2(_rb2D.velocity.x, _jumpForce);
                // jumpEffect
                //PoolManager.instance.ReuseObject(jumpEffect, groundCheck.position, Quaternion.identity);
            }
            else if (GameInputManager.MainInstance.Jump && _wallGrabbing && _moveInput != _onWallSide && _canClimb)      // wall jumping off the wall
            {
                _wallGrabbing = false;
                _wallJumping = true;
                Debug.Log("Wall jumped");

                _rb2D.AddForce(new Vector2(-_onWallSide * _wallJumpForce.x, _wallJumpForce.y), ForceMode2D.Impulse);
            }
            else if (GameInputManager.MainInstance.Jump && _wallGrabbing && _moveInput != 0 && (_moveInput == _onWallSide) && _canClimb)      // wall climbing jump
            {
                _wallGrabbing = false;
                _wallJumping = true;
                Debug.Log("Wall climbed");

                _rb2D.AddForce(new Vector2(-_onWallSide * _wallClimbForce.x, _wallClimbForce.y), ForceMode2D.Impulse);
            }
        }
		void UpdateGroundOffset()
		{
            // grounded remember offset (for more responsive jump)
            _groundedRememberTime -= Time.deltaTime;
            if (_isGrounded)
                _groundedRememberTime = _groundedRememberDuration;
        }

        #region Fly
        void UnApplyGravity()
        {
            _rb2D.gravityScale = 0f;
        }
        void UpdateFlyMove()
        {
            _rb2D.velocity = new Vector2(_moveInput * _speed, _flyInput * _speed);
        }
        #endregion
        #region Dash
        void DashAll()
        {
            if (!_canJumpFar) return;
            DashInput();
            ResetDashInAir();

        }
        void DashInput()
        {
            // if not currently dashing and hasn't already dashed in air once
            if (!_isDashing && !_hasDashedInAir && _dashColdTime <= 0f)
            {
                // dash input (left shift)
                if (GameInputManager.MainInstance.Dash)
                {
                    _isDashing = true;
                    GamePoolManager.MainInstance.TryGetPoolItem("DashSound", transform.position, Quaternion.identity);
                    // dash effect
                    //PoolManager.instance.ReuseObject(dashEffect, transform.position, Quaternion.identity);
                    // if player in air while dashing
                    if (!_isGrounded)
                    {
                        _hasDashedInAir = true;
                    }
                    // dash logic is in FixedUpdate
                }
            }
            _dashColdTime -= Time.deltaTime;
        }

        void ResetDashInAir()
        {
            // if has dashed in air once but now grounded
            if (_hasDashedInAir && _isGrounded)
                _hasDashedInAir = false;
        }

        #endregion

        void UpdateAnimation()
		{
			if (_isDashing)
			{
				_animator.Play(AnimationID.DashID);
				return;
			}
            if (_animator.AnimationAtTag("ATK"))
            {
                return;
            }
            if (_onLeftWall||_onRightWall)
			{
                if (_canClimb)
                {
                    _animator.Play(AnimationID.SlideID);
                }
                else
                {
                    _animator.Play(AnimationID.IdleID);
                }
				return;
			}
            if (_isGrounded && Mathf.Abs(_moveInput) > 0.2f)
            {
                _animator.Play(AnimationID.RunID);
            }
            else if (_isGrounded)
            {
                _animator.Play(AnimationID.IdleID);
            }
            else if (_rb2D.velocity.y > 0.1f)
            {
                _animator.Play(AnimationID.JumpID);
            }
            else if (_rb2D.velocity.y < -0.1f)
            {
                _animator.Play(AnimationID.FallID);
            }
        }
        void Flip()
		{
			if(_isDashing) return;
            if (_onLeftWall)
            {
                transform.eulerAngles = Vector3.zero;
            }
            else if (_onRightWall)
            {
                transform.eulerAngles = new Vector3(0f, 180f, 0f);
            }    
            else if (GameInputManager.MainInstance.Horizontal < 0f)
            {
                transform.eulerAngles = Vector3.zero;
            }
            else if (GameInputManager.MainInstance.Horizontal > 0f)
            {
                transform.eulerAngles = new Vector3(0f, 180f, 0f);
            }
            _facingRight = transform.eulerAngles == new Vector3(0f, 180f, 0f);
        }

        void CalculateSides()
		{
			if (_onRightWall)
				_onWallSide = 1;
			else if (_onLeftWall)
				_onWallSide = -1;
			else
				_onWallSide = 0;

			if (_facingRight)
				_playerSide = 1;
			else
				_playerSide = -1;
		}

        private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 1f);
            Gizmos.DrawWireSphere(_groundCheck.position, _groundCheckRadius);
			Gizmos.DrawWireSphere((Vector2)transform.position + _grabRightOffset, _grabCheckRadius);
			Gizmos.DrawWireSphere((Vector2)transform.position + _grabLeftOffset, _grabCheckRadius);
		}
        public void UpdateParameters(AnimalsSO animalSO,Transform where)
        {
            if (transform != where)
            {
                return;
            }
            _canSwim = animalSO.canSwim;
            _canDiveWater = animalSO.canDiveWater;

            _canFly = animalSO.canFly;
            _canJumpFar = animalSO.canJumpFar;

            _canBeTiny = animalSO.canBeTiny;

            _canClimb = animalSO.canClimb;
            _canJumpFromHighPoint = animalSO.canJumpFromHighPoint;

            _canDig = animalSO.canDig;
            _canNightVision = animalSO.canNightVision;
            
            _jumpForce = animalSO.jumpForce;
            _fallMutiplier = animalSO.fallMutiplier;
            _groundCheckRadius = animalSO.groundCheckRadius;
            _dashSpeed = animalSO.dashSpeed;
            _dashDuration = animalSO.dashDuration;

            _grabLeftOffset= animalSO.grabLeftOffset;
            _grabRightOffset= animalSO.grabRightOffset;
            _grabCheckRadius= animalSO.grabCheckRadius;
            _slideSpeed= animalSO.slideSpeed;  
            _wallJumpForce= animalSO.wallJumpForce;
            _wallClimbForce= animalSO.wallClimbForce;   
        }
	}
}
