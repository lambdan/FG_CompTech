using Unity.Entities;
using UnityEngine;

public class ConfigAuthoring : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public GameObject EnemyPrefab;
    public GameObject BulletPrefab;

    public float PlayerSpeed = 1;
    public float PlayerRotationSpeed = 2;

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

}