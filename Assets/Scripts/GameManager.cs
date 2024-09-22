using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // In Game Elements
    public TurnManager turnManagerPrefab;
    public WinManager winManagerPrefab;
    public List<Player> playerPrefabs;
    public int roundQuantity; // How many rounds will have a game

    private PlayerStation[] playerStations;
    private int numberOfPlayers;
    private List<Player> players = new List<Player>();
    private int roundPlayed = 0; // How many rounds are already played

    public TurnManager currentTm;

    // Game States
    public enum GameState
    {
        MainMenu,
        Playing,
        Paused,
        GameOver
    }

    public GameState currentGameState;

    public enum RoundTurn
    {
        None,
        Players,
        Dealer
    }

    public RoundTurn roundTurn;

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
        roundTurn = RoundTurn.None;
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

    public void StartNewGame(int playerCount, int roundToPlay)
    {
        numberOfPlayers = Mathf.Clamp(playerCount, 1, 5);
        roundQuantity = roundToPlay;
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
        turnManagerPrefab.dealer = dealer;
        turnManagerPrefab.deck = deck;

        // Instantiation of Win and Turn Managers
        Instantiate(winManagerPrefab);
        currentTm = Instantiate(turnManagerPrefab) as TurnManager;
    }

    // Method to spawn Players for each PlayerStation
    private void SpawnPlayers()
    {
        players.Clear();

        System.Random rng = new System.Random();
        playerPrefabs = playerPrefabs.OrderBy(x => rng.Next()).ToList(); // Random order of which type of Player prefab will be in the game

        for (int i = 0; i < numberOfPlayers; i++) // Determining how many Players are in this game
        {
            if (i < playerStations.Length)
            {
                Player player = playerStations[i].AssignPlayer(playerPrefabs[i]);
                players.Add(player);
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

        roundPlayed = 0;

        // Load the main menu
        SceneManager.LoadScene("MainMenu");
    }

    public void CheckRoundNumber()
    {
        roundPlayed++; // Increment the quantity of rounds played

        if (roundPlayed < roundQuantity + 1) 
        {
            currentTm.dealer.ResetHand();
            currentTm.StartNewRound(); // Start a new round if the game is not complete
        }
        else // If all the game's rounds are played, reset the scene and come back to the main menu
        {
            Debug.LogError("Round finished");
        }
    }

    public Player[] GetPlayerList()
    {
        Player[] playerArr = players.ToArray();
        return playerArr;
    }

    public int GetRoundPlayed()
    {
        return roundPlayed;
    }

    public void GetCurrentTurn(RoundTurn currentRoundTurn)
    {
        roundTurn = currentRoundTurn;
    }

    public string GetCurrentTurn()
    {
        return roundTurn.ToString();
    }

    public void PauseGame()
    {
        currentGameState = GameState.Paused;
        Time.timeScale = 0f; // Pause time when paused
    }

    public void ResumeGame()
    {
        currentGameState = GameState.Playing;
        Time.timeScale = 1f; // Time flows again
    }

    public void BackToMainMenu()
    {
        currentGameState = GameState.MainMenu;  // Go back to the main menu
        EndGame();
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game is exiting...");
    }

}
