using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _rb2d;
    private Animator _animator;

    private Vector2 _velocity;
    private float _inputAxis;
    private float _fallSpeedYDampingChangeThreshold;
    [SerializeField, Header("Ground Detection")] protected float _groundDetectionPositionOffset;
    [SerializeField] protected float _detectionRange;
    [SerializeField] private float _moveSpeed = 7f;
    [SerializeField] private float _moveDelay = 2f;

    [SerializeField] private float _maxJumpHeight = 5f;
    [SerializeField] private float _maxJumpTime = 1f;
    public float JumpForce => (2f * _maxJumpHeight) / (_maxJumpTime / 2f);
    public float Gravity => (-2f * _maxJumpHeight) / Mathf.Pow((_maxJumpTime / 2f), 2);
    public Vector2 Velocity => _velocity;
    public bool IsFacingRight => transform.eulerAngles.y == 180f;
    public bool OnGround => GroundDetection();
    public bool IsJumping { get; private set; }
    public bool IsSliding => (_inputAxis > 0f && _velocity.x < 0f) || (_inputAxis < 0f && _velocity.x > 0f);
    public bool IsRunning => Mathf.Abs(_inputAxis) > 0.10f || Mathf.Abs(_velocity.x) > 0.10f;

    // Start is called before the first frame update
    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _rb2d = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        _fallSpeedYDampingChangeThreshold = CameraManager.MainInstance._fallSpeedYDampingChangeThreshold;
    }

    // Update is called once per frame
    private void Update()
    {
        HorizontalMovement();
        if (OnGround)
        {

            GroundMovement();
        }
        else ApplyGravity();

    }

    private void OnEnable()
    {
        _rb2d.isKinematic = false;
    }

    private void OnDisable()
    {
        _rb2d.isKinematic = true;
        _velocity = Vector2.zero;
        IsJumping = false;
    }

    private void FixedUpdate()
    {
        TurnCheck();
        Vector2 position = _rb2d.position;
        position += _velocity * Time.fixedDeltaTime;
        _rb2d.MovePosition(position);
    }
    private void LateUpdate()
    {
        GameEventManager.MainInstance.CallEvent("SwitchCamera", _velocity.y);
        GameEventManager.MainInstance.CallEvent("LerpYDamping", _velocity.y);
    }
    private void TurnCheck()
    {
        if (_inputAxis > 0f)
        {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }
        else if (_inputAxis < 0f)
        {
            transform.eulerAngles = Vector3.zero;
        }
    }
    private bool GroundDetection()
    {
        var detectionPosition = new Vector2(transform.position.x, transform.position.y - _groundDetectionPositionOffset);
        return Physics2D.OverlapCircle(detectionPosition, _detectionRange, 1<<3);
    }
    private void OnDrawGizmos()
    {
        var detectionPosition = new Vector3(transform.position.x, transform.
            position.y - _groundDetectionPositionOffset, transform.position.z);
        Gizmos.DrawWireSphere(detectionPosition, _detectionRange);
    }
    private void HorizontalMovement()
    {

        _inputAxis = Input.GetAxis("Horizontal");
        if (OnGround&&Mathf.Abs(_inputAxis) > 0.2f)
        {
            _animator.Play(AnimationID.RunID);
        }
        else if(OnGround)
        {
            _animator.Play(AnimationID.IdleID);
        }
        else if(_velocity.y>0.1f)
        {
            _animator.Play(AnimationID.JumpID);    
        }
        else if (_velocity.y<-0.1f)
        {
            _animator.Play(AnimationID.FallID);
        }
        _velocity.x = Mathf.MoveTowards(_velocity.x, _inputAxis * _moveSpeed, _moveSpeed * Time.deltaTime * _moveDelay);
        if (_rb2d.Raycast(0.2f, 0.25f, Vector2.right * _velocity.x, LayerMask.GetMask("Ground"), LayerMask.GetMask("Wall"))){
            _velocity.x = 0;
        }
    }

    private void GroundMovement()
    {
        _velocity.y = Mathf.Max(_velocity.y, 0f);
        IsJumping = _velocity.y > 0f;
        if (Input.GetButtonDown("Jump"))
        {
            Jump(JumpForce);
        }
    }

    public void Jump(float jumpForce)
    {
        _velocity.y = jumpForce;
        IsJumping = true;
    }

    private void ApplyGravity()
    {
        bool falling = !Input.GetButton("Jump") || _velocity.y < 0f;
        float multiplier = falling ? 2f : 1f;

        _velocity.y += Gravity * multiplier * Time.deltaTime;
        _velocity.y = Mathf.Max(_velocity.y, Gravity / 2f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }

}
