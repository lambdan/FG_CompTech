using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private TMP_Text _killsText;
    [SerializeField] private TMP_Text _healthText;
    [SerializeField] private TMP_Text _gameOverText;
    [SerializeField] private TMP_Text _activeEnemiesText;

    private int _enemiesSpawned;
    private int _kills;
    private bool _gameOver;
    
    
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

    public void AddEnemiesActive(int n)
    {
        _enemiesSpawned += n;
        UpdateActiveEnemiesText();
    }
    
    public void AddKill()
    {
        _kills += 1;

        if (_kills % 10 == 0)
        {
            PlayerShip.Instance.Heal(1); // heal every 10 kills
        }

        if (_kills % 100 == 0)
        {
            PlayerShip.Instance.AddMaxHealth(1); // add max health every 100 kills
        }
        
        UpdateKillsText();
        UpdateActiveEnemiesText();
    }

    private void UpdateActiveEnemiesText()
    {
        _activeEnemiesText.text = "Active Enemies: " + (_enemiesSpawned - _kills);
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
}
