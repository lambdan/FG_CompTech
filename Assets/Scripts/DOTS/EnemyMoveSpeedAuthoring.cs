using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public class EnemyMoveSpeedAuthoring : MonoBehaviour
{
    // public float Speed = 2.0f;

    class Baker : Baker<EnemyMoveSpeedAuthoring>
    {
        public override void Bake(EnemyMoveSpeedAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new EnemyMoveSpeed
            {
                // Speed = authoring.Speed
            });
        }
    }
}

public struct EnemyMoveSpeed : IComponentData
{
    // public float Speed;
}