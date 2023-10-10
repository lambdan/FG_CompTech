using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


[Serializable]
public struct PlayerTag : IComponentData
{
}


[RequireComponent(typeof(Health))]
public class PlayerShip : MonoBehaviour, IDamageable, IHealable
{
    public float MovementSpeed = 2f;
    public float RotationSpeed = 200f;

    private Weapon _weapon;
    private Health _health;
    private World _world;
    private Entity _playerPosEntity;
    private HitInvincibility _hitInvincibility;

    public static PlayerShip Instance;
    
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        Instance = this;
        
        _weapon = GetComponentInChildren<Weapon>();
        _health = GetComponent<Health>();
        
        _hitInvincibility = GetComponent<HitInvincibility>();
    }

    void Start()
    {
        // get reference to playerpos entity data
        _world = World.DefaultGameObjectInjectionWorld;
        _playerPosEntity = new EntityQueryBuilder(Allocator.Temp).WithAll<PlayerPosition>().Build(_world.EntityManager).GetSingletonEntity();
        
        GameManager.Instance.UpdateHealthText(_health.GetCurrentHealth(), _health.GetMaxHealth());
    }
    
    void Update()
    {
        // input handling
        transform.position += transform.up * (Input.GetAxisRaw("Vertical") * Time.deltaTime * MovementSpeed);
        transform.Rotate(0, 0, Input.GetAxisRaw("Horizontal") * Time.deltaTime * RotationSpeed * -1);
        
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

    private void FixedUpdate()
    {
        // tell enemy entities where we are
        _world.EntityManager.SetComponentData(_playerPosEntity, new PlayerPosition() { PlayerPos = transform.position});
        
        if (Input.GetKey(KeyCode.Space))
        {
            _weapon.Fire();
        }
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
        }
    }


    public void Heal(float amount)
    {
        _health.AddHealth(amount);
        GameManager.Instance.UpdateHealthText(_health.GetCurrentHealth(), _health.GetMaxHealth());
    }

    public void AddMaxHealth(float amount)
    {
        _health.AddMaxHealth(amount);
        _health.AddHealth(999);
        GameManager.Instance.UpdateHealthText(_health.GetCurrentHealth(), _health.GetMaxHealth());
    }
}
