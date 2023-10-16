using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Random = Unity.Mathematics.Random;


[UpdateBefore(typeof(TransformSystemGroup))]
public partial struct EnemyMoveSystem : ISystem
{
    private uint _randomCounter;

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
        var playerPos = SystemAPI.GetSingleton<Player>().Transform.Position;
        
        foreach (var enemyTransform in SystemAPI.Query<RefRW<LocalTransform>>().WithAll<Enemy>())
        {
            // set direction towards player
            var dir = math.normalizesafe(playerPos - enemyTransform.ValueRO.Position);
            
            // jitter
            if (config.EnemyJitter)
            {
               dir += Random.CreateFromIndex(++_randomCounter).NextFloat(-1f, 1f) * new float3(1, 1, 0); 
            }

            enemyTransform.ValueRW.Position += dir * SystemAPI.Time.DeltaTime * config.EnemySpeed;
        }
    }




}