using Unity.Entities;
using UnityEngine;

public class ConfigAuthoring : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject PlayerPrefab;
    public GameObject EnemyPrefab;
    public GameObject BulletPrefab;

    [Header("Player Params")]
    public float PlayerSpeed = 1;
    public float PlayerRotationSpeed = 2;

    [Header("Enemy Spawning")]
    public int EnemySpawnAmount = 10;
    public float EnemySpawnFrequency = 1.0f;
    public float EnemySpawnRadius = 5.0f;

    [Header("Enemy Params")]
    public float EnemySpeed = 1.5f;
    public bool EnemyJitter = false;

    class Baker : Baker<ConfigAuthoring>
    {
        public override void Bake(ConfigAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            
            AddComponent(entity, new Config
            {
                PlayerPrefab = GetEntity(authoring.PlayerPrefab, TransformUsageFlags.Dynamic),
                EnemyPrefab = GetEntity(authoring.EnemyPrefab, TransformUsageFlags.Dynamic),
                BulletPrefab = GetEntity(authoring.BulletPrefab, TransformUsageFlags.Dynamic),
                PlayerSpeed = authoring.PlayerSpeed,
                PlayerRotationSpeed = authoring.PlayerRotationSpeed,
                EnemySpawnAmount = authoring.EnemySpawnAmount,
                EnemySpawnFrequency = authoring.EnemySpawnFrequency,
                EnemySpawnRadius = authoring.EnemySpawnRadius,
                EnemySpeed = authoring.EnemySpeed,
                EnemyJitter = authoring.EnemyJitter
                
            });
        }
    }
}

public struct Config : IComponentData
{
    public Entity PlayerPrefab;
    public Entity EnemyPrefab;
    public Entity BulletPrefab;
    public float PlayerSpeed;
    public float PlayerRotationSpeed;
    public int EnemySpawnAmount;
    public float EnemySpawnFrequency;
    public float EnemySpawnRadius;
    public float EnemySpeed;
    public bool EnemyJitter;

}