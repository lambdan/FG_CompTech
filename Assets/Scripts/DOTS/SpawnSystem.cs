using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
public partial struct SpawnSystem : ISystem
{
    private uint _updateCounter;
    private double _lastSpawn;
    private const float TWO_PI = math.PI * 2.0f; // i cant remember what its called lol
    
    public void OnCreate(ref SystemState state)
    {
        // TODO Change to look for player instead
        state.RequireForUpdate<SpawnerEntity>();
        state.RequireForUpdate<PlayerState>();
    }
    
    public void OnUpdate(ref SystemState state)
    {
        if (SystemAPI.Time.ElapsedTime > (_lastSpawn + SystemAPI.GetSingleton<SpawnerEntity>().frequency))
        { 
            var random = Random.CreateFromIndex(_updateCounter++);
            var radius = SystemAPI.GetSingleton<SpawnerEntity>().radius;
            var angle = TWO_PI / SystemAPI.GetSingleton<SpawnerEntity>().amount;
            var playerPos = SystemAPI.GetSingleton<PlayerState>().PlayerPos;

            for (int i=0; i < SystemAPI.GetSingleton<SpawnerEntity>().amount; i++)
            {
                Entity entity = state.EntityManager.Instantiate(SystemAPI.GetSingleton<SpawnerEntity>().enemyEntity);
                var transform = SystemAPI.GetComponentRW<LocalTransform>(entity);

                // set random spawn pos in a circle
                float randomAngle = angle * i;
                float x = radius * math.cos(randomAngle) + playerPos.x;
                float y = radius * math.sin(randomAngle) + playerPos.y;
                transform.ValueRW.Position = new float3(x, y, 0);
                
            }

            _lastSpawn = SystemAPI.Time.ElapsedTime;
        }
    }
    

}