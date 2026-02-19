using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    [SerializeField] private Transform _feetTransform;
    [SerializeField] private Vector2 _groundCheck;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _jumpStrength = 7f;

    [SerializeField] private float _extraGravity = 700f;
    [SerializeField] private float _gravityDelay = .5f;
    
    private float _airTime;
    private PlayerInputScript _playerInput;
    private FrameStruct _frameStruct;

    //private bool _isGrounded = false;
    private Movement _movement;

    private Rigidbody2D _rigidBody;

    public void Awake() {
        if (Instance == null) { Instance = this; }

        _rigidBody = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInputScript>();
        _movement = GetComponent<Movement>();
    }

    private void Update()
    {
        GatherInput();
        Movement();
        Jump();
        HandleSpriteFlip();
        GravityDelay();
    }

    void FixedUpdate()
    {
        ExtraGravity();
    }

    private bool CheckGrounded()
    {
        Collider2D isGrounded = Physics2D.OverlapBox(_feetTransform.position, _groundCheck, 0f, _groundLayer);
        return isGrounded;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(_feetTransform.position, _groundCheck);    
    }

    private void GravityDelay()
    {
        if(!CheckGrounded())
            _airTime += Time.deltaTime;
        else
            _airTime = 0f;
    }

    private void ExtraGravity()
    {
        if(_airTime > _gravityDelay)
            _rigidBody.AddForce(new Vector2(0f, -_extraGravity * Time.deltaTime));
    }

    public bool IsFacingRight()
    {
        return transform.eulerAngles.y == 0;
    }

    private void GatherInput()
    {
        // float moveX = Input.GetAxis("Horizontal");
        // _movement = new Vector2(moveX * _moveSpeed, _rigidBody.linearVelocity.y);
    
        _frameStruct = _playerInput.FrameStruct;
    }

    private void Movement() {
        _movement.SetDirection(_frameStruct.Move.x);
    }

    private void Jump()
    {
        if(!_frameStruct.Jump) {return;}
        if (CheckGrounded()) {
            _rigidBody.AddForce(Vector2.up * _jumpStrength, ForceMode2D.Impulse);
        }
    }

    private void HandleSpriteFlip()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (mousePosition.x < transform.position.x)
        {
            transform.eulerAngles = new Vector3(0f, -180f, 0f);
        }
        else
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
    } 
}
