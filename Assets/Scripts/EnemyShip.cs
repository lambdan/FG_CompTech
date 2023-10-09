using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class EnemyShip : MonoBehaviour, IDamageable
{
    public float MovementSpeed = 1.5f;
    // public float RotationSpeed = 200f;

    private Weapon _weapon;
    private Health _health;
    private PlayerShip _playerShip;

    void Awake()
    {
        _weapon = GetComponentInChildren<Weapon>();
        _health = GetComponent<Health>();
        
    }

    void Start()
    {
        _playerShip = FindObjectOfType<PlayerShip>();
    }
    
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _playerShip.transform.position, MovementSpeed * Time.deltaTime);
    }

    public void Damage(float dmg)
    {
        _health.ChangeHealth(-dmg);
        if (_health.IsDead())
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        IDamageable id = col.gameObject.GetComponent<IDamageable>();
        if (id != null)
        {
            id.Damage(1.0f);
        }
    }
}
