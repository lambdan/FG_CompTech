using TMPro;
using Unity.Entities;
using UnityEngine;
using UnityEngine.SceneManagement;

// this did a whole lot of things in non-DOTS version but now its basically just a HUD updater

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private TMP_Text _healthText;
    [SerializeField] private TMP_Text _gameOverText;
    [SerializeField] private TMP_Text _activeEnemiesText;
    [SerializeField] private TMP_Text _bulletsActiveText;

    private int _activeEnemies;
    private bool _gameOver;

    private World _world;
    private EntityManager _entityManager;
    private EntityQuery _playerHealthQuery;
    
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
        _playerHealthQuery = _entityManager.CreateEntityQuery(typeof(PlayerHealth));
        _gameOver = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) // <R>estart
        {
            CleanAndRestartECS();
        }

        if (Input.GetKeyDown(KeyCode.Q)) // <Q>uit
        {
            Application.Quit();
        }
    }
    
    private void UpdateActiveEnemiesText()
    {
        int enemiesActive = _entityManager.CreateEntityQuery(ComponentType.ReadOnly<Enemy>()).CalculateEntityCount();
        _activeEnemiesText.text = "Enemies: " + enemiesActive;
    }
    
    private void UpdateActiveBulletsText()
    {
        int bulletsActive = _entityManager.CreateEntityQuery(ComponentType.ReadOnly<Bullet>()).CalculateEntityCount();
        _bulletsActiveText.text = "Bullets Active: " + bulletsActive;
    }
    
    public void UpdateHealthText()
    {
        if (_playerHealthQuery.TryGetSingleton(out PlayerHealth p))
        {
            _healthText.text = "Health: " + p.Health;
            _gameOver = false;
        }
        else if(!_gameOver){ // we cant find player health anymore + game over was true = we must be dead
            GameOver();
        }

    }
    
    public void GameOver()
    {
        _gameOver = true;
        _healthText.text = "Dead";
        _gameOverText.gameObject.SetActive(true);
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
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
