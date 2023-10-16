using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Random = Unity.Mathematics.Random;


[UpdateBefore(typeof(TransformSystemGroup))]
public partial struct EnemyMoveSystem : ISystem
{
    private uint randomCounter;
    private float3 dir;
    
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Player>();
        state.RequireForUpdate<Enemy>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var config = SystemAPI.GetSingleton<Config>();
        
        RefRO<LocalTransform> playerTransform = default;

        foreach (var transform in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<Player>())
        {
            playerTransform = transform;
        }


        foreach (var (transform,entity) in SystemAPI.Query<RefRW<LocalTransform>>().WithAll<Enemy>().WithEntityAccess())
        {
            var playerPos = playerTransform.ValueRO.Position;
            dir = math.normalizesafe(playerPos - transform.ValueRO.Position);
            
            // jitter
            if (config.EnemyJitter)
            {
               dir += Random.CreateFromIndex(++randomCounter).NextFloat(-1f, 1f) * new float3(1, 1, 0); 
            }

            transform.ValueRW.Position += dir * SystemAPI.Time.DeltaTime * config.EnemySpeed;

            var distanceToPlayer = math.distance(playerPos, transform.ValueRO.Position);
            if (distanceToPlayer < 0.1f)
            {
                // decrement player HP
                
                // TODO there must be a better way !!!
                foreach(var h in SystemAPI.Query<RefRW<PlayerHealth>>().WithAll<Player>())
                {
                    if (h.ValueRO.LastDamage + 1 < SystemAPI.Time.ElapsedTime) // TODO feels cursed to do player damage cooldown here
                    {
                        h.ValueRW.Health -= 1;
                        h.ValueRW.LastDamage = SystemAPI.Time.ElapsedTime;
                    }
                }
            }
        }
    }




}