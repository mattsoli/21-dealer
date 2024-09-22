using System;
using System.Collections.Generic;
using UnityEngine;

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
        InitializePhasesDictionary();
    }

    public void PlayerWaiting(Player player)
    {
        Debug.Log($"> Player_{player.playerId}'s turn");

        if (player.IsWaiting && !players.Contains(player))
        {
            players.Add(player); // Record all Players are waiting
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
                dealer.IsDealerTurn();
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

}