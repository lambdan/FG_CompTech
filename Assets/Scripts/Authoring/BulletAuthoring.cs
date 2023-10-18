using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class BulletAuthoring : MonoBehaviour
{
    class Baker : Baker<BulletAuthoring>
    {
        public override void Bake(BulletAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<Bullet>(entity);
            AddComponent<Velocity>(entity);
        }
    }
}

public struct Bullet : IComponentData
{
    // maybe keep instigator here?
}

public struct Velocity : IComponentData
{
    public float3 Value;
}