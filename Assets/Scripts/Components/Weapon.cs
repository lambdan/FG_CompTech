using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Weapon : MonoBehaviour
{
     [SerializeField] private GameObject _projectilePrefab;
     
     private GameObject _owner;

     private void Awake()
     {
          _owner = transform.parent.gameObject;
     }

     public void Fire()
     {
          GameObject projectileGO = Instantiate(_projectilePrefab, transform.position, Quaternion.identity);
          Projectile projectile = projectileGO.GetComponent<Projectile>();
          projectile.SetInstigator(_owner);
          projectile.SetDirection(transform.up);
     }
}
