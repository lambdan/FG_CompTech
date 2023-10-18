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
    [SerializeField] private TMP_Text _bulletsActiveText;

    private int _activeEnemies;
    private int _kills;
    private bool _gameOver;

    private World _world;
    private EntityManager _entityManager;
    private EntityQuery q;


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
        q = _entityManager.CreateEntityQuery(typeof(PlayerHealth));
        
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
    
    private void UpdateActiveEnemiesText()
    {
        int active = _entityManager.CreateEntityQuery(ComponentType.ReadOnly<Enemy>()).CalculateEntityCount();
        int inactive = _entityManager.CreateEntityQuery(ComponentType.ReadOnly<Enemy>(), ComponentType.ReadOnly<Disabled>()).CalculateEntityCount();
        _activeEnemiesText.text = "Enemies: " + active + " (disabled: " + inactive + ")";
    }
    
    private void UpdateActiveBulletsText()
    {
        var q = _entityManager.CreateEntityQuery(ComponentType.ReadOnly<Bullet>()).CalculateEntityCount();
        _bulletsActiveText.text = "Bullets Active: " + q;
    }

    public void UpdateKillsText()
    {
        _killsText.text = "Kills: " + _kills;
    }

    public void UpdateHealthText()
    {
        // TODO cache stuff (can probably get Player entity, then get HealthSystem from there
        
        if (q.TryGetSingleton<PlayerHealth>(out PlayerHealth p))
        {
            _healthText.text = "Health: " + p.Health;

        }
        else if(!_healthText.text.Contains("X")){ // works because placeholder text is X/3 lol
            GameOver();
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
        GUI.Label(new Rect(10, 10, 200, 50), "FPS: " + (1 / Time.smoothDeltaTime).ToString("f0"));
    }

    private void FixedUpdate()
    {
        UpdateActiveEnemiesText();
        UpdateHealthText();
        UpdateActiveBulletsText();
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
