using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateBefore(typeof(BulletDamageSystem))]
public partial struct BulletMovementSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Bullet>();
        state.RequireForUpdate<Config>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var config = SystemAPI.GetSingleton<Config>();
        
        foreach(var (bulletTransform, velocity, entity) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<Velocity>>().WithAll<Bullet>().WithEntityAccess())
        {
            var newPos = bulletTransform.ValueRO.Position + (velocity.ValueRO.Value * SystemAPI.Time.DeltaTime * config.BulletSpeed);

            if (math.abs(newPos.x) > 10 || math.abs(newPos.y) > 10) // off screen?
            {
                SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged).DestroyEntity(entity); // queue ourselves to be destroyed
                continue;
            }

            bulletTransform.ValueRW.Position = newPos;
        }
    }




}