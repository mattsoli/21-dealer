using System;
using System.Collections.Generic;
using UnityEngine;

public class WinManager : MonoBehaviour
{
    private List<Player> players;
    private Dealer dealer;

    public static event Action<Dictionary<Player, Outcome>> OnWinConditionEvaluated;

    public enum PlayerStates
    {
        Active,
        Blackjack,
        Bust,
        Stand
    }

    public enum Outcome
    {
        Win,
        Lose,
        Push
    }

    private void OnEnable()
    {
        TurnManager.OnEndRound += EvaluateWinConditions;
    }

    private void OnDisable()
    {
        TurnManager.OnEndRound -= EvaluateWinConditions;
    }
        private void Awake()
    {
        dealer = FindObjectOfType<Dealer>();
        players = new List<Player>(FindObjectsOfType<Player>());
    }

    private void EvaluateWinConditions()
    {
        Debug.Log("=== Win Conditions Evaluation ===");

        Dictionary<Player, Outcome> results = new Dictionary<Player, Outcome>();

        // For each player in game, evaluate the status with the possible outcomes
        foreach (Player player in players)
        {
            Outcome outcome = DetermineOutcome(player, dealer);
            results[player] = outcome; // Fullfill a dictionary with all the players in game evaluated
        }

        OnWinConditionEvaluated?.Invoke(results);
    }

    private Outcome DetermineOutcome(Player player, Dealer dealer)
    {
        // Determines every single outcome can be between a Player and the Dealer

        Debug.Log("Player_" + player.playerId + " -> " + player.status);

        Debug.Log("Dealer -> " + dealer.state);

        if (player.status == PlayerStates.Bust)
            return Outcome.Lose;

        if (player.status == PlayerStates.Blackjack && dealer.state != PlayerStates.Blackjack)
            return Outcome.Win;

        if (player.status == PlayerStates.Blackjack && dealer.state == PlayerStates.Blackjack)
            return Outcome.Push;

        if (dealer.state == PlayerStates.Bust && player.status != PlayerStates.Bust)
            return Outcome.Win;

        if (player.hand.Is21 && dealer.state == PlayerStates.Blackjack)
            return Outcome.Lose;

        if (player.hand.score > dealer.hand.score)
            return Outcome.Win;

        if (player.hand.score < dealer.hand.score)
            return Outcome.Lose;

        return Outcome.Push;
    }
}
