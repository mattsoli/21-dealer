using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
        EndGame
    }

    public GamePhase currentPhase;

    private Dictionary<GamePhase, Action> phasesDictionary;

    // Event declarations for each phase
    public static event Action OnInitialDeal;
    public static event Action OnMiddleDeal;
    public static event Action OnEndGame;

    private void Awake()
    {
        currentPhase = GamePhase.None;

        phasesDictionary = new Dictionary<GamePhase, Action>
        {
            { GamePhase.InitialDeal, OnInitialDeal },
            { GamePhase.MiddleDeal, OnMiddleDeal },
            { GamePhase.EndGame, OnEndGame }
        };
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            NextPhase();
        }
    }





    public void PlayerWaiting(Player player)
    {
        Debug.Log("> PLAYER_" + player.playerId);

        if (player.IsWaiting && !players.Contains(player))
        {
            players.Add(player);
            Debug.Log("Player_" + player.playerId + " in waiting list");
        }
    }

    public void PlayerEndTurn(Player player)
    {
        if (!player.IsWaiting)
        {
            players.Remove(player);
        }

        if (players.Count == 0)
        {
            if (currentPhase == GamePhase.InitialDeal)
            {
                dealer.InitialSetup();
            }
            else
            {
                dealer.IsDealerTurn = true;
            }

        }
    }

    public void NextPhase()
    {
        currentPhase++;

        phasesDictionary[currentPhase]?.Invoke();

        Debug.Log("Phase " + currentPhase + " started");
    }


}