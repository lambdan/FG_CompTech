using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class EnemyShip : MonoBehaviour, IDamageable
{
    public float MovementSpeed = 1.5f;
    private Health _health;
    private HitInvincibility _hitInvincibility;

    void Awake()
    {
        _health = GetComponent<Health>();
        _hitInvincibility = GetComponent<HitInvincibility>();
    }
    
    void FixedUpdate()
    {
        if (PlayerShip.Instance == null)
        {
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, PlayerShip.Instance.transform.position, MovementSpeed * Time.fixedDeltaTime);

    }

    public void Damage(float dmg)
    {
        if (_hitInvincibility != null && _hitInvincibility.IsActive())
        {
            return;
        }
        
        _health.TakeDamage(dmg);
        
        if (_health.IsDead())
        {
            GameManager.Instance.AddKill();
            Destroy(this.gameObject);
        }
        else
        {
            if (_hitInvincibility != null)
            {
                _hitInvincibility.StartInvincibility();
            }
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
