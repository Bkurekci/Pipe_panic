using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private int _damageAmount = 1;

    private Vector2 _fireDirection;
    private Gun _gun;
    private Rigidbody2D _rigidBody;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    public void Init(Gun gun ,Vector2 _bulletSpawnPoint, Vector2 _mousePos)
    {
        _gun = gun;
        transform.position = _bulletSpawnPoint;
        _fireDirection = (_mousePos - _bulletSpawnPoint).normalized;
    }

    private void FixedUpdate()
    {
        _rigidBody.linearVelocity = _fireDirection * _moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Health health = other.gameObject.GetComponent<Health>();
        health?.TakeDamage(_damageAmount);
        _gun.ReleaseBullet(this);
    }
}