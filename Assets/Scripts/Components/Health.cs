using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float StartHealth = 3;

    private float _currentHealth;
    private float _maxHealth;
    private bool _isDead;
    
    void Start()
    {
        _currentHealth = StartHealth;
        _maxHealth = StartHealth;
        _isDead = false;
    }
    
    public float GetCurrentHealth()
    {
        return _currentHealth;
    }

    public float GetMaxHealth()
    {
        return _maxHealth;
    }
    
    public float HealthAsPercentage()
    {
        return _currentHealth / _maxHealth;
    }

    public bool IsDead()
    {
        return _isDead;
    }

    public void ChangeHealth(float delta)
    {
        _currentHealth += delta;

        if (delta < 0)
        {
            Debug.Log(this.gameObject.name + " took damage");
        }
        else
        {
            Debug.Log(this.gameObject.name + " healed");
        }

        if (_currentHealth <= 0)
        {
            Dead();
        }
    }

    
    
    void Dead()
    {
        Debug.Log(this.gameObject.name + " is dead");
        _isDead = true;
    }
}
