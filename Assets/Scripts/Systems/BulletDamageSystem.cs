using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;


public partial struct BulletDamageSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Config>();
        state.RequireForUpdate<Bullet>();
        state.RequireForUpdate<Enemy>();
    }

    public void OnUpdate(ref SystemState state)
    {
        var config = SystemAPI.GetSingleton<Config>();
        
        foreach(var (bulletTransform, bulletEntity) in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<Bullet>().WithEntityAccess())
        {
            foreach (var (enemyTransform, enemyEntity) in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<Enemy>().WithEntityAccess())
            {
                var bulletPos = bulletTransform.ValueRO.Position;
                var enemyPos = enemyTransform.ValueRO.Position;
                
                if (math.abs(bulletPos.x - enemyPos.x) > 0.1f || math.abs(bulletPos.y - enemyPos.y) > 0.1f)
                {
                    continue;
                }
                
                // hitting enemy
                if (config.DestroyBulletOnImpact) { 
                    SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged).DestroyEntity(bulletEntity);
                }
                
                SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged).DestroyEntity(enemyEntity);

            }
        }
        
    }




}