using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float DamageAmount = 1.0f;
    public float Speed = 3.0f;

    private GameObject _instigator;
    private Rigidbody2D _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    
    public void SetInstigator(GameObject instigator)
    {
        _instigator = instigator;
    }

    public void SetDirection(Vector2 dir)
    {
        _rb.AddForce(dir * Speed, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (_instigator != col.gameObject)
        {
            IDamageable id = col.gameObject.GetComponent<IDamageable>();
            if (id != null)
            {
                id.Damage(DamageAmount);
                Destroy(this.gameObject);
            }
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
