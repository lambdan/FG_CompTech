using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class SpawnerAuthoring : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public float radius = 10;
    public float frequency = 0.1f;
    public int many = 100;
    
    
    class Baker : Baker<SpawnerAuthoring>
    {
        public override void Bake(SpawnerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            
            AddComponent(entity, new SpawnerEntity
            {
                enemyEntity = GetEntity(authoring.EnemyPrefab, TransformUsageFlags.Dynamic),
                frequency = authoring.frequency,
                amount = authoring.many,
                radius = authoring.radius
            });
        }
    }
}

struct SpawnerEntity : IComponentData
{
    public Entity enemyEntity;
    public float frequency;
    public int amount;
    public float radius;
}

