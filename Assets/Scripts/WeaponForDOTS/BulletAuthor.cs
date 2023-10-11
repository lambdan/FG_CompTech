using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class BulletAuthor : MonoBehaviour
{
    // public GameObject BulletPrefab;
    
    class Baker : Baker<BulletAuthor>
    {
        public override void Bake(BulletAuthor author)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            
            AddComponent(entity, new BulletTag
            {
            });
        }
    }
}

public struct BulletTag : IComponentData
{
}