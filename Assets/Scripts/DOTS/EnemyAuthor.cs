using UnityEngine;
using Unity.Entities;

public class EnemyAuthor : MonoBehaviour
{
    
    class Baker : Baker<EnemyAuthor>
    {
        public override void Bake(EnemyAuthor authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new EnemyTag()
            {
            });
        }
    }
}

public struct EnemyTag : IComponentData
{
}