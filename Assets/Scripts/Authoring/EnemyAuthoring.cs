using UnityEngine;
using Unity.Entities;

public class EnemyAuthoring : MonoBehaviour
{
    
    class Baker : Baker<EnemyAuthoring>
    {
        public override void Bake(EnemyAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Enemy()
            {
            });
        }
    }
}

public struct Enemy : IComponentData
{
}