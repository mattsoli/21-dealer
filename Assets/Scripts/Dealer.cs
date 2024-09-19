using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dealer : MonoBehaviour
{
    public Hand hand = new();

    private bool isInitialSetup = false; // Control for the first 2 cards of the game
    public bool IsInitialSetup { get; private set; }

    private bool isDealerTurn = false; // for a card
    public bool IsDealerTurn { get; set; }

    private TurnManager tm;

    // Start is called before the first frame update
    void Start()
    {
        tm = FindObjectOfType<TurnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsDealerTurn)
        {
            IsDealerTurn = false;
            tm.NextPhase();
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        // when a card is throwed to the player and is waiting for it, add the card to player's hand
        if (other.gameObject.CompareTag("Card") && IsDealerTurn)
        {
            PhysicalCard lastCard = other.gameObject.GetComponent<PhysicalCard>();
            hand.AddCard(lastCard);

            Debug.Log("Dealer gets => " + lastCard.cardObject.ToString());

            if (hand.cards.Count == 1)
                InitialSetup();

            ScoreCheck();
        }
    }

    public void InitialSetup()
    {
        if (hand.cards.Count < 1)
        {
            Debug.Log("Dealer is waiting the initial setup...");
            IsDealerTurn = true;
        }
        else
        {
            IsDealerTurn = false;
            tm.NextPhase();
        }

    }

    private void ScoreCheck()
    {
        if (hand.isBusted)
        {
            Debug.Log("Dealer BUSTED!");
            IsDealerTurn = false;
            tm.NextPhase();
            return;
        }
        else if (hand.isBlackjack)
        {
            Debug.Log("Dealer BLACKJACK!");
            IsDealerTurn = false;
            tm.NextPhase();
            return;
        }
        else
        { }
    }


}
