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
    private Vector3 _destination;

    void Awake()
    {
        _weapon = GetComponentInChildren<Weapon>();
        _health = GetComponent<Health>();
        
    }

    void Start()
    {
        _destination = FindObjectOfType<PlayerShip>().transform.position;
    }
    
    void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, _destination, MovementSpeed * Time.fixedDeltaTime);
    }

    public void Damage(float dmg)
    {
        _health.ChangeHealth(-dmg);
        if (_health.IsDead())
        {
            GameManager.Instance.AddKill();
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D col) // Damage ship?
    {
        IDamageable id = col.gameObject.GetComponent<IDamageable>();
        if (id != null)
        {
            id.Damage(1.0f);
        }
    }
}
