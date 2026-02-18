using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 10f;
    private float _moveX;
    private Rigidbody2D _rigidbody;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        Move();
    }

    public void SetDirection(float moveDirection)
    {
        _moveX = moveDirection;
    }

    private void Move()
    {
        Vector2 movement = new Vector2(_moveX * _moveSpeed, _rigidbody.linearVelocity.y);
        _rigidbody.linearVelocity = movement;
    }
}
