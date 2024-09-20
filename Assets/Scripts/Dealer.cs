using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dealer : MonoBehaviour
{
    public Hand hand = new();

    private bool isDealerTurn = false; // for a card
    public bool IsDealerTurn { get; set; }

    public WinManager.PlayerStatus status;

    private TurnManager tm;

    // Start is called before the first frame update
    void Start()
    {
        tm = FindObjectOfType<TurnManager>();

        status = WinManager.PlayerStatus.Active;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsDealerTurn)
        {
            IsDealerTurn = false;
            tm.NextPhase();
        }

        if(Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("> Dealer status -> " + status);
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

            HandStateCheck();
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

    private void HandStateCheck()
    {
        if (hand.IsBusted)
        {
            Debug.Log("Dealer BUSTED!");
            status = WinManager.PlayerStatus.Bust;
            IsDealerTurn = false;
            tm.NextPhase();
            return;
        }
        else if (hand.IsBlackjack)
        {
            Debug.Log("Dealer BLACKJACK!");
            status = WinManager.PlayerStatus.Blackjack;
            IsDealerTurn = false;
            tm.NextPhase();
            return;
        }
        else
        {
            status = WinManager.PlayerStatus.Stand;

            if (hand.Is21) // If Hand is a 21 (not blackjack) the Dealer's turn ends
            {
                IsDealerTurn = false;
                tm.NextPhase();
            }
        }
    }


}
