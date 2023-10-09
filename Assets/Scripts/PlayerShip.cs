using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Health))]
public class PlayerShip : MonoBehaviour, IDamageable
{
    public float MovementSpeed = 2f;
    public float RotationSpeed = 200f;

    private Weapon _weapon;
    private Health _health;
    private HitInvincibility _hitInvincibility;
    

    void Awake()
    {
        _weapon = GetComponentInChildren<Weapon>();
        _health = GetComponent<Health>();
        
        _hitInvincibility = GetComponent<HitInvincibility>();
    }

    void Start()
    {
        GameManager.Instance.UpdateHealthText(_health.GetCurrentHealth(), _health.GetMaxHealth());
    }
    
    void Update()
    {
        // input handling
        transform.position += transform.up * (Input.GetAxisRaw("Vertical") * Time.deltaTime * MovementSpeed);
        transform.Rotate(0, 0, Input.GetAxisRaw("Horizontal") * Time.deltaTime * RotationSpeed * -1);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _weapon.Fire();
        }

        // wrap around screen
        if (transform.position.x > 9)
        {
            transform.position = new Vector3(-9, transform.position.y, 0);
        }
        if (transform.position.x < -9)
        {
            transform.position = new Vector3(9, transform.position.y, 0);
        }
        
        if (transform.position.y > 5)
        {
            transform.position = new Vector3(transform.position.x, -5, 0);
        }
        if (transform.position.y < -5)
        {
            transform.position = new Vector3(transform.position.x, 5, 0);
        }

    }

    public void Damage(float dmg)
    {
        if (_hitInvincibility != null && _hitInvincibility.IsActive())
        {
            return;
        }
        
        _health.ChangeHealth(-dmg);

        

        if (_health.IsDead())
        {
            GameManager.Instance.GameOver();
            Destroy(this.gameObject);
        }
        else
        {
            if (_hitInvincibility != null)
            {
                _hitInvincibility.StartInvincibility();
            }
            
            
            GameManager.Instance.UpdateHealthText(_health.GetCurrentHealth(), _health.GetMaxHealth());
            
            // play flashing animation
            // etc
        }
    }
    
    
    
}
