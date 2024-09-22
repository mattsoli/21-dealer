using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static WinManager;

public class TurnManager : MonoBehaviour
{
    public List<Player> players = new(); // List of players in game
    public Dealer dealer; // Dealer reference
    public Deck deck; // The deck to draw cards from

    public enum GamePhase // All the phases of a game
    {
        None,
        Setup,
        InitialDeal,
        MiddleDeal,
        EndRound
    }

    public GamePhase currentPhase;

    private Dictionary<GamePhase, Action> phasesDictionary;


    // Event declarations for each phase
    public static event Action OnSetup;
    public static event Action OnInitialDeal;
    public static event Action OnMiddleDeal;
    public static event Action OnEndRound;

    private void Awake()
    {
        //currentPhase = GamePhase.None;
        InitializePhasesDictionary();
    }

    private void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1)) // ------> DEBUG DA TOGLIERE
        {
            NextPhase();
        }

        Debug.Log(currentPhase.ToString());
    }

    public void PlayerWaiting(Player player)
    {
        Debug.Log($"> Player_{player.playerId}'s turn");

        if (player.IsWaiting && !players.Contains(player))
        {
            players.Add(player);
        }

        GameManager.Instance.GetCurrentTurn(GameManager.RoundTurn.Players);
    }

    public void PlayerEndTurn(Player player)
    {
        if (!player.IsWaiting)
        {
            players.Remove(player); // When a Player isn't waiting then remove it from the waiting list
        }

        if (players.Count == 0) // When all the Players played, it's Dealer's turn
        {
            if (currentPhase == GamePhase.InitialDeal)
            {
                dealer.InitialSetup();
            }
            else
            {
                dealer.IsDealerTurn = true;
                Debug.Log("> Dealer's turn");
            }

            GameManager.Instance.GetCurrentTurn(GameManager.RoundTurn.Dealer);
        }
    }

    public void StartNewRound()
    {
        currentPhase = GamePhase.None;
        Debug.Log("=== New Round ===");
        NextPhase();
    }

    public void NextPhase()
    {
        //currentPhase = GetNextPhase(currentPhase);

        currentPhase++;

        if (currentPhase == GamePhase.Setup)
        {
            phasesDictionary[currentPhase]?.Invoke();
            currentPhase++;
        }

        phasesDictionary[currentPhase]?.Invoke();

        Debug.Log($"Current phase: {currentPhase}");
    }

    private void InitializePhasesDictionary()
    {
        phasesDictionary = new Dictionary<GamePhase, Action>
        {
            { GamePhase.Setup, OnSetup },
            { GamePhase.InitialDeal, OnInitialDeal },
            { GamePhase.MiddleDeal, OnMiddleDeal },
            { GamePhase.EndRound, OnEndRound }
        };
    }

    private GamePhase GetNextPhase(GamePhase currentPhase)
    {
        switch (currentPhase)
        {
            case GamePhase.None:
                return GamePhase.Setup;
            case GamePhase.Setup:
                return GamePhase.InitialDeal;
            case GamePhase.InitialDeal:
                return GamePhase.MiddleDeal;
            case GamePhase.MiddleDeal:
                return GamePhase.EndRound;
            case GamePhase.EndRound:
            default:
                return GamePhase.Setup;
        }
    }

}