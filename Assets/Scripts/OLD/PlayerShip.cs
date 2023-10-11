using System;
using System.ComponentModel;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class PlayerShip : MonoBehaviour, IDamageable, IHealable
{
    public float MovementSpeed = 2f;
    public float RotationSpeed = 200f;

    private Weapon _weapon;
    private Health _health;
    private HitInvincibility _hitInvincibility;
    
    private World _world;
    private EntityManager _entityManager;
    private Entity _playerPosEntity;
    private bool _gotEntity = false;
    
    

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
        _gotEntity = false;
        
        // get reference to playerpos entity data
        _world = World.DefaultGameObjectInjectionWorld;
        _entityManager = _world.EntityManager;
        
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

        if (Input.GetKeyDown(KeyCode.F))
        {
            Damage(999);
        }

    }

    private void FixedUpdate()
    {
        // to tell enemy entities where we are
        // https://docs.unity.cn/Packages/com.unity.entities@0.50/api/Unity.Entities.EntityQuery.SetSingleton.html

        // if (!_gotEntity)
        // {
        //     EntityQuery eq = _entityManager.CreateEntityQuery(new ComponentType[] { typeof(PlayerState) });
        //
        //     if (eq.TryGetSingletonEntity<PlayerState>(out Entity eee))
        //     {
        //         _playerPosEntity = eee;
        //         _gotEntity = true;
        //     }
        //     else
        //     {
        //         Debug.Log("failed to get entity");
        //     }
        // }
        // else
        // {
        //     // update player pos in entity
        //     _entityManager.SetComponentData(_playerPosEntity, new PlayerState()
        //         {
        //             PlayerAlive =  _health.IsAlive(), 
        //             PlayerPos = transform.position
        //         }
        //     );
        // }
        
        
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
