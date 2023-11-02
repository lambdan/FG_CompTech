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
        state.RequireForUpdate<Config>();
        state.RequireForUpdate<Player>();
        state.RequireForUpdate<Enemy>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var config = SystemAPI.GetSingleton<Config>();
        var playerPos = SystemAPI.GetComponentRO<LocalTransform>(SystemAPI.GetSingleton<Player>().Entity).ValueRO.Position;

        var job = new EnemyMovementJob
        {
            DeltaTime = SystemAPI.Time.DeltaTime,
            PlayerPosition = playerPos,
            Speed = config.EnemySpeed
        };
        job.ScheduleParallel();

    }


    [WithAll(typeof(Enemy))]
    [BurstCompile]
    public partial struct EnemyMovementJob : IJobEntity
    {
        public float DeltaTime;
        public float3 PlayerPosition;
        public float Speed;
        
        public void Execute(ref LocalTransform enemyTransform)
        {
            var dir = math.normalizesafe(PlayerPosition - enemyTransform.Position);
            enemyTransform.Position += (dir * Speed * DeltaTime);
        }
    }



}