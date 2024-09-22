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
        InitialDeal,
        MiddleDeal,
        EndRound
    }

    public GamePhase currentPhase;

    private Dictionary<GamePhase, Action> phasesDictionary;

    // Event declarations for each phase
    //public static event Action OnSetup;
    public static event Action OnInitialDeal;
    public static event Action OnMiddleDeal;
    public static event Action OnEndRound;

    private void Awake()
    {
        phasesDictionary = new Dictionary<GamePhase, Action>
        {
            { GamePhase.InitialDeal, OnInitialDeal },
            { GamePhase.MiddleDeal, OnMiddleDeal },
            { GamePhase.EndRound, OnEndRound }
        };

        currentPhase = GamePhase.None;

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
    }

    public void PlayerWaiting(Player player)
    {
        Debug.Log($"> PLAYER_{player.playerId}'s turn");

        if (player.IsWaiting && !players.Contains(player))
        {
            players.Add(player);
            Debug.Log("Player_" + player.playerId + " in waiting list");
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
        currentPhase++;

        if (currentPhase != GamePhase.None)
            phasesDictionary[currentPhase]?.Invoke();

        Debug.Log("Phase " + currentPhase + " started");
    }


 
}