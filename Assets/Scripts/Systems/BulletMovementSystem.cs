using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateBefore(typeof(TransformSystemGroup))]
[UpdateBefore(typeof(BulletSpawnerSystem))]
public partial struct BulletMovementSystem : ISystem
{
    private ComponentLookup<Bullet> _bulletLookup;
    
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Bullet>();
        state.RequireForUpdate<Config>();
        _bulletLookup = state.GetComponentLookup<Bullet>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var config = SystemAPI.GetSingleton<Config>();
        _bulletLookup.Update(ref state);
        
        foreach(var (bulletTransform, velocity, entity) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<Velocity>>().WithAll<Bullet>().WithEntityAccess())
        {
            var newPos = bulletTransform.ValueRW.Position + (velocity.ValueRO.Value * SystemAPI.Time.DeltaTime * config.BulletSpeed);

            if (math.abs(newPos.x) > 10 || math.abs(newPos.y) > 10) // off screen?
            {
                _bulletLookup.SetComponentEnabled(entity, false);
                // SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged).DestroyEntity(entity); // queue ourselves to be destroyed
                continue;
            }

            bulletTransform.ValueRW.Position = newPos;
        }
    }




}