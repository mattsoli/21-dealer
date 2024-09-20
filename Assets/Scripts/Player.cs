using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int playerId;
    public float decisionThreshold = 16;
    public float standProbability = 0.5f; // Chance to stand if over threshold
    public Hand hand = new();

    private bool isInitialSetup = false; // Control for the first 2 cards of the game
    public bool IsInitialSetup { get; private set; }

    private bool isWaiting = false; // for a card
    public bool IsWaiting { get; private set; }

    public WinManager.PlayerStatus status;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("> Player_" + playerId + " status -> " + status);
        }
    }

    private void OnEnable()
    {
        TurnManager.OnInitialDeal += InitialSetup;
        TurnManager.OnMiddleDeal += Decide;

        status = WinManager.PlayerStatus.Active;
    }

    private void OnDisable()
    {
        TurnManager.OnInitialDeal -= InitialSetup;
        TurnManager.OnMiddleDeal -= Decide;
    }

    private void OnTriggerEnter(Collider other)
    {
        // When a card is throwed to the player and is waiting for it, add the card to player's hand
        if (other.gameObject.CompareTag("Card") && IsWaiting)
        {
            PhysicalCard lastCard = other.gameObject.GetComponent<PhysicalCard>();
            hand.AddCard(lastCard);

            Debug.Log("Player_" + playerId + " gets => " + lastCard.cardObject.ToString());

            if (hand.cards.Count < 3 && IsInitialSetup)
            {
                InitialSetup();
            }
            else
            {
                Decide();
            }
        }
    }

    private void Decide()
    {
        TurnManager tm = FindAnyObjectByType<TurnManager>();

        if (hand.IsBusted) // Player Busted
        {
            Debug.Log("Player_" + playerId + "BUSTED!");
            status = WinManager.PlayerStatus.Bust;
            IsWaiting = false;
            tm.PlayerEndTurn(this);
            return;
        }

        float dynamicProbability = CalculateStandProbability(hand.score); // Dynamic probability based on how 21 is near
        float randomValue = Random.Range(0f, 1f);

        // Player decides to ask another card is the score is less than the decision threshold
        if (hand.score > decisionThreshold && randomValue < dynamicProbability) // If true, Player stands
        {
            Stand();
            tm.PlayerEndTurn(this);
        }
        else
        {
            Hit();
            tm.PlayerWaiting(this);
        }

    }

    private void InitialSetup()
    {
        Debug.Log("Player_" + playerId + " is waiting the initial setup...");

        TurnManager tm = FindObjectOfType<TurnManager>();

        IsWaiting = true;
        IsInitialSetup = true;

        tm.PlayerWaiting(this);

        if (hand.cards.Count == 2)
        {
            IsWaiting = false;
            IsInitialSetup = false;

            tm.PlayerEndTurn(this);
        }

    }

    private void Hit()
    {
        IsWaiting = true;
        Debug.Log("Player_" + playerId + " asks new card...");
    }

    private void Stand()
    {
        IsWaiting = false;

        if (hand.IsBlackjack)
            status = WinManager.PlayerStatus.Blackjack;
        else
            status = WinManager.PlayerStatus.Stand;

        Debug.Log("Player_" + playerId + " stands...");
    }

    public void ResetHand()
    {
        hand.cards.Clear();
        Debug.Log("Player_" + playerId + " hand is reset");
    }

    // Return a chance to Stand if score is near to 21
    private float CalculateStandProbability(int score)
    {
        if (score >= 21) return 1f; // Always stop if the score is 21

        int difference = 21 - score;
        float probability = 1f - (difference / 10f); // More score is near to 21, higher is the chance to Stand

        probability = Mathf.Pow(probability, 2); // Chance grows squared
        probability = Mathf.Clamp(probability, 0f, 1f);

        Debug.Log($"Player_{playerId} has a {probability}% to Stand");
        return probability;
    }

}
