using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class EnemyController : MonoBehaviour
{
    public bool HuntPlayer = true;
    public float Speed = 2;
    
     class Baker : Baker<EnemyController>
    {
        public override void Bake(EnemyController authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            
            AddComponent(entity, new EnemyParams
            {
                HuntPlayer = authoring.HuntPlayer,
                Speed = authoring.Speed
            });

            AddComponent(entity, new PlayerState
            {
                PlayerPos = float3.zero,
                PlayerAlive = false
            });

        }


    }
}

public struct EnemyParams : IComponentData
{
    public bool HuntPlayer;
    public float Speed;
}

public struct PlayerState : IComponentData
{
    public float3 PlayerPos;
    public bool PlayerAlive;
}