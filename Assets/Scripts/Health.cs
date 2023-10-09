using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float StartHealth = 3;

    private float _currentHealth;
    
    void Start()
    {
        _currentHealth = StartHealth;
    }
    
    public float GetCurrentHealth()
    {
        return _currentHealth;
    }
    
    public float HealthAsPercentage()
    {
        return _currentHealth / StartHealth;
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
    }
}
