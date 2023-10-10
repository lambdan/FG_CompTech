using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float Speed = 5.0f;
    // public float3 PlayerPos;

    class Baker : Baker<EnemyController>
    {
        public override void Bake(EnemyController authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new EnemyData()
            {
                Speed = authoring.Speed
            });
            AddComponent(entity, new PlayerPosition()
            {
            });
        }
    }
}

public struct EnemyData : IComponentData
{
    public float Speed;
}

public struct PlayerPosition : IComponentData
{
    public float3 PlayerPos;
}