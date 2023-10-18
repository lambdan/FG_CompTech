using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.VisualScripting;
using UnityEngine;

[UpdateBefore(typeof(TransformSystemGroup))]
[UpdateBefore(typeof(EnemyMoveSystem))]
public partial struct BulletSpawnerSystem : ISystem
{
    private double _lastFire; // used for cooldown checking
    private ComponentLookup<Bullet> _bulletLookup;
    
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Player>();
        state.RequireForUpdate<Config>();
        _bulletLookup = state.GetComponentLookup<Bullet>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        if (!Input.GetKey(KeyCode.Space))
        {
            // not pressing Space
            return;
        }
        
        var config = SystemAPI.GetSingleton<Config>();
        
        if (_lastFire + config.FireCooldown > SystemAPI.Time.ElapsedTime)
        {
            // still in cooldown period
            return;
        }
        
        _lastFire = SystemAPI.Time.ElapsedTime;
        var playerTransform = SystemAPI.GetComponentRO<LocalTransform>(SystemAPI.GetSingleton<Player>().Entity).ValueRO;

        _bulletLookup.Update(ref state);
        
        foreach (var bulletTag in SystemAPI.Query<Bullet>().WithOptions(EntityQueryOptions.IgnoreComponentEnabledState))
        {
            bool enabled = _bulletLookup.IsComponentEnabled(bulletTag.Entity);
            if (!enabled)
            {
                // Debug.Log("re-using disabled bullet");
                SetBulletDirectionSpeed(state, config, bulletTag.Entity, playerTransform);
                _bulletLookup.SetComponentEnabled(bulletTag.Entity, true);
                return;
            }
        }
        
        // no bullet were available, we have to spawn one
        var newBullet = state.EntityManager.Instantiate(config.BulletPrefab);
        SetBulletDirectionSpeed(state, config, newBullet, playerTransform);
        // Debug.Log("spawned bullet");
    }

    [BurstCompile]
    public void SetBulletDirectionSpeed(SystemState state, Config config, Entity bullet, LocalTransform playerTransform)
    {
        state.EntityManager.SetComponentData(bullet, new LocalTransform
        {
            Position = playerTransform.Position + (playerTransform.Forward() * config.BulletSpawnForwardOffset),
            Rotation = playerTransform.Rotation,
            Scale = 0.1f
        });
        
        state.EntityManager.SetComponentData(bullet, new Velocity
        {
            // set Bullet velocity to be Players forward
            Value = math.mul(playerTransform.Rotation, new float3(0,1,0))
        });
    }
}