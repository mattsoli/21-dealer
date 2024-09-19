using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int playerId;
    public float decisionThreshold = 16;
    public Hand hand = new();

    private bool isInitialSetup = false; // Control for the first 2 cards of the game
    public bool IsInitialSetup { get; private set; }

    private bool isWaiting = false; // for a card
    public bool IsWaiting { get; private set; }

    private bool isStanding = false;
    public bool IsStanding { get; private set; }

    private void Update()
    {
        
    }

    private void OnEnable()
    {
        TurnManager.OnInitialDeal += InitialSetup;
        TurnManager.OnMiddleDeal += Decide;
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

        if (hand.isBusted) // Player Busted
        {
            Debug.Log("Player_" + playerId + "BUSTED!");
            IsWaiting = false;
            tm.PlayerEndTurn(this);
            return;
        }

        if (hand.score > decisionThreshold) // player decides to ask another card is the score is less than the decision threshold
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
        IsStanding = false;
        Debug.Log("Player_" + playerId + " asks new card...");
    }

    private void Stand()
    {
        IsStanding = true;
        IsWaiting = false;
        Debug.Log("Player_" + playerId + " stands...");
    }

    public void ResetHand()
    {
        hand.cards.Clear();
        Debug.Log("Player_" + playerId + " hand is reset");
    }


}
