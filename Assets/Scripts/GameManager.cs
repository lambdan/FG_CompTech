using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private TMP_Text _killsText;
    [SerializeField] private TMP_Text _healthText;
    [SerializeField] private TMP_Text _gameOverText;
    [SerializeField] private TMP_Text _activeEnemiesText;

    private int _activeEnemies;
    private int _kills;
    private bool _gameOver;
    private int _activeEnemiesCount;
    
    private World _world;
    private EntityManager _entityManager;


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }

        Instance = this;
    }

    void Start()
    {
        _world = World.DefaultGameObjectInjectionWorld;
        _entityManager = _world.EntityManager;
        
        _kills = 0;
        _gameOver = false;
        UpdateKillsText();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) // restart
        {
            CleanAndRestartECS();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Application.Quit();
        }
    }
    
    public void AddKill()
    {
        _kills += 1;
        UpdateKillsText();
    }

    private void UpdateActiveEnemiesText()
    {
        _activeEnemiesCount = _entityManager.CreateEntityQuery(typeof(Enemy)).CalculateEntityCount();
        _activeEnemiesText.text = "Active Enemies: " + _activeEnemiesCount;
    }

    public void UpdateKillsText()
    {
        _killsText.text = "Kills: " + _kills;
    }

    public void UpdateHealthText()
    {
        // TODO cache stuff
        EntityQuery q = _entityManager.CreateEntityQuery(typeof(Player));
        if (q.TryGetSingleton<Player>(out Player p))
        {
            _healthText.text = "Health: " + p.Health;

        }
    }
    
    public void GameOver()
    {
        _gameOver = true;
        _healthText.text = "Dead";
        _gameOverText.gameObject.SetActive(true);
    }

    public bool IsGameOver()
    {
        return _gameOver;
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 20), "FPS: " + (1 / Time.smoothDeltaTime).ToString("f0"));
    }

    private void FixedUpdate()
    {
        UpdateActiveEnemiesText();
        UpdateHealthText();
    }

    public void CleanAndRestartECS()
    {
        // https://forum.unity.com/threads/reset-ecs-world-and-jobs-to-start-fresh-again.1364208/#post-8609088
        _entityManager.CompleteAllTrackedJobs();
        foreach (var system in _world.Systems)
        {
            system.Enabled = false;
        }

        _world.Dispose();
        DefaultWorldInitialization.Initialize("Default World", false);
        if (!ScriptBehaviourUpdateOrder.IsWorldInCurrentPlayerLoop(World.DefaultGameObjectInjectionWorld))
        {
            ScriptBehaviourUpdateOrder.AppendWorldToCurrentPlayerLoop(World.DefaultGameObjectInjectionWorld);
        }

        // SceneManager.LoadScene("Game", LoadSceneMode.Single);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
