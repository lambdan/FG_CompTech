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
    private World _world;

    
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
        _kills = 0;
        _gameOver = false;
        UpdateKillsText();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) // restart
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Application.Quit();
        }
    }

    public void ChangeEnemiesActive(int n)
    {
        _activeEnemies += n;
        UpdateActiveEnemiesText();
    }
    
    public void AddKill()
    {
        _kills += 1;
        //
        // if (_kills % 10 == 0)
        // {
        //     PlayerShip.Instance.Heal(1); // heal every 10 kills
        // }

        // if (_kills % 100 == 0)
        // {
        //     PlayerShip.Instance.AddMaxHealth(1); // add max health every 100 kills
        // }
        
        UpdateKillsText();
    }

    private void UpdateActiveEnemiesText()
    {
        var q = new EntityQueryBuilder(Allocator.Temp).WithAll<EnemyMoveSpeed>().Build(_world.EntityManager).CalculateEntityCount();
        _activeEnemiesText.text = "Active Enemies: " + q;
    }

    public void UpdateKillsText()
    {
        _killsText.text = "Kills: " + _kills;
    }

    public void UpdateHealthText(float currentHealth, float maxHealth)
    {
        _healthText.text = "Health: " + currentHealth + "/" + maxHealth;
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
        GUI.Label(new Rect(10, 10, 100, 20), (1 / Time.smoothDeltaTime).ToString("f0"));
    }

    private void FixedUpdate()
    {
        UpdateActiveEnemiesText();
    }
}
