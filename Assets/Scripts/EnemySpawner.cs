using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float SpawnFrequency = 1.0f; // how often to spawn enemies
    public int SpawnsPerFrequency = 2; // how many enemies to spawn every n seconds
    public Vector2 SpawnRadiusRange = new Vector2(8, 12);
    
    [SerializeField] private List<GameObject> EnemyPrefabs = new List<GameObject>();

    
    private PlayerShip _playerShip;

    void Awake()
    {
        _playerShip = FindObjectOfType<PlayerShip>();
    }
    
    void Start()
    {
        StartCoroutine(SpawnCoroutine());
        
        
    }

    Vector3 RandomPoint()
    {
        float randomRadius = Random.Range(SpawnRadiusRange.x, SpawnRadiusRange.y);
        float randomAngle = Random.Range(0f, Mathf.PI * 2);
        float x = transform.position.x + randomRadius * Mathf.Cos(randomAngle);
        float y = transform.position.y + randomRadius * Mathf.Sin(randomAngle);
        return new Vector3(x, y, 0);
    }


    IEnumerator SpawnCoroutine()
    {
        while (!GameManager.Instance.IsGameOver())
        {
            for (int i = 0; i < SpawnsPerFrequency; i++)
            {
                GameObject randomEnemy = EnemyPrefabs[Random.Range(0, EnemyPrefabs.Count)];
                Instantiate(randomEnemy, RandomPoint(), Quaternion.identity);
            }

            GameManager.Instance.AddEnemiesActive(SpawnsPerFrequency);
            
            yield return new WaitForSeconds(SpawnFrequency);
        }
    }
}
