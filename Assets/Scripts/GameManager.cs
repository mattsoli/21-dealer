using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // In Game Elements
    public TurnManager turnManager;
    public WinManager winManager;
    public Player playerPrefab;
    public int roundQuantity = 2; // How many rounds will have a game

    private PlayerStation[] playerStations;
    private int numberOfPlayers;
    private List<Player> players = new List<Player>();
    private int roundPlayed = 0; // How many rounds are already played

    // Game States
    public enum GameState
    {
        MainMenu,
        Playing,
        Paused,
        GameOver
    }

    public GameState currentGameState;

    #region Singleton
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("GameManager is NULL");
            return _instance;
        }
    }

    void Awake()
    {
        if (_instance)
            Destroy(gameObject);
        else
            _instance = this;

        DontDestroyOnLoad(this);
    }
    #endregion

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        // Start the game in the Main Menu
        currentGameState = GameState.MainMenu;

    }

    void Update()
    {
        // Check for pause input
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentGameState == GameState.Playing)
            {
                PauseGame();
            }
            else if (currentGameState == GameState.Paused)
            {
                ResumeGame();
            }
        }

        // Check for input to end the game
        if (SceneManager.GetActiveScene().name == "Game" && Input.GetKeyDown(KeyCode.Escape))
        {
            EndGame();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Game")
        {
            InitializeGame();
            currentGameState = GameState.Playing;
        }
        else
        {
            currentGameState = GameState.MainMenu;
        }
    }

    public void StartNewGame(int playerCount)
    {
        numberOfPlayers = Mathf.Clamp(playerCount, 1, 5);
        SceneManager.LoadScene("Game");
    }

    private void InitializeGame()
    {
        playerStations = FindObjectsOfType<PlayerStation>();
        SpawnPlayers();

        // Initialize other game components       
        Dealer dealer = FindObjectOfType<Dealer>();
        Deck deck = FindObjectOfType<Deck>();

        // Set up the TurnManager with the spawned players
        turnManager.dealer = dealer;
        turnManager.deck = deck;

        // Instantiation of Win and Turn Managers
        Instantiate(winManager);
        Instantiate(turnManager);

        // Start the first round
        //turnManager.StartNewRound();
    }

    private void SpawnPlayers()
    {
        players.Clear();
        for (int i = 0; i < numberOfPlayers; i++)
        {
            if (i < playerStations.Length)
            {
                Player player = playerStations[i].AssignPlayer(playerPrefab);
                players.Add(player);
            }
            else
            {
                Debug.LogWarning($"Not enough PlayerStations for player {i}");
            }
        }
    }

    public void EndGame()
    {
        // Clean up the game scene
        foreach (Player player in players)
        {
            Destroy(player.gameObject);
        }
        players.Clear();

        // Load the main menu
        SceneManager.LoadScene("MainMenu");
    }

    public void CheckRoundNumber()
    {
        roundPlayed++; // Increment the quantity of rounds played

        if (roundPlayed == roundQuantity) // If all the game's rounds are played, reset the scene and come back to the main menu
        {
            // SCHERMATA DI FINE PARTITA
        }
        else
        {
            turnManager.StartNewRound(); // Start a new round if the game is not complete
        }
    }


    //// Set the game state and activate/deactivate corresponding UI
    //public void SetGameState(GameState newState)
    //{
    //    currentGameState = newState;

    //    switch (newState)
    //    {
    //        case GameState.MainMenu:
    //            mainMenuUI.SetActive(true);
    //            pauseMenuUI.SetActive(false);
    //            gameUI.SetActive(false);
    //            Time.timeScale = 0f; // Stop time in main menu
    //            break;

    //        case GameState.Playing:
    //            mainMenuUI.SetActive(false);
    //            pauseMenuUI.SetActive(false);
    //            gameUI.SetActive(true);
    //            Time.timeScale = 1f; // Resume time when playing
    //            break;

    //        case GameState.Paused:
    //            mainMenuUI.SetActive(false);
    //            pauseMenuUI.SetActive(true);
    //            gameUI.SetActive(false);
    //            Time.timeScale = 0f; // Pause time when paused
    //            break;

    //        case GameState.GameOver:
    //            // Handle game over state (you could add another UI)
    //            break;
    //    }
    //}
    //private void InitializeGame()
    //{
    //    playerStations = FindObjectsOfType<PlayerStation>();
    //    SpawnPlayers();

    //    // Initialize other game components
    //    TurnManager turnManager = FindObjectOfType<TurnManager>();
    //    WinManager winManager = FindObjectOfType<WinManager>();
    //    Dealer dealer = FindObjectOfType<Dealer>();
    //    Deck deck = FindObjectOfType<Deck>();

    //    // Set up the TurnManager with the spawned players
    //    turnManager.players = new List<Player>(players);
    //    turnManager.dealer = dealer;
    //    turnManager.deck = deck;

    //    // Start the first round
    //    turnManager.StartNewRound();
    //}


    public void PauseGame()
    {
        currentGameState = GameState.Paused;
        //pauseMenuUI.SetActive(true);
        //gameUI.SetActive(false);
        Time.timeScale = 0f; // Pause time when paused
    }

    public void ResumeGame()
    {
        currentGameState = GameState.Playing;
    }

    //public void StartGame()
    //{
    //    for (int i = 0; i < playerStations.Length; i++)
    //    {
    //        Player newPlayer = Instantiate(playerPrefab);
    //        newPlayer.playerId = i;
    //        playerStations[i].AssignPlayer(newPlayer);
    //    }

    //    SetGameState(GameState.Playing); // Start the game from the main menu
    //}

    public void BackToMainMenu()
    {
        //SetGameState(GameState.MainMenu);  // Go back to the main menu
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game is exiting...");
    }

}
