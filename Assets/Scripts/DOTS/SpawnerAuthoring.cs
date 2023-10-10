using Unity.Collections;
using Unity.Entities;
using UnityEngine;

// An authoring component is just a normal MonoBehavior that has a Baker<T> class.
public class SpawnerAuthoring : MonoBehaviour
{
    public GameObject Prefab;
    public float frequency = 0.5f;
    public int many = 10;
    
    class Baker : Baker<SpawnerAuthoring>
    {
        public override void Bake(SpawnerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            
            AddComponent(entity, new SpawnerEntity
            {
                enemyEntity = GetEntity(authoring.Prefab, TransformUsageFlags.Dynamic),
                frequency = authoring.frequency,
                amount = authoring.many
            });
        }


    }
}

struct SpawnerEntity : IComponentData
{
    public Entity enemyEntity;
    public float frequency;
    public int amount;
}

