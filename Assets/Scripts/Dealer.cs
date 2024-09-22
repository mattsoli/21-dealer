using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dealer : MonoBehaviour
{
    public Hand hand = new();

    private bool isDealerTurn = false; // for a card
    public bool IsDealerTurn { get; set; }

    public WinManager.PlayerStates status;

    // Start is called before the first frame update
    void Start()
    {
        status = WinManager.PlayerStates.Active;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsDealerTurn)
        {
            StandTurn();
        }

        if(Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("> Dealer status -> " + status);
        }
    }

    public void StandTurn()
    {
        IsDealerTurn = false;
        GameManager.Instance.currentTm.NextPhase();
    }

    private void OnTriggerEnter(Collider other)
    {
        // When a card is throwed to the Dealer add the Card to Dealer's hand
        if (other.gameObject.CompareTag("Card") && IsDealerTurn)
        {
            PhysicalCard lastCard = other.gameObject.GetComponent<PhysicalCard>();
            hand.AddCard(lastCard);

            Debug.Log("Dealer gets => " + lastCard.cardObject.ToString());

            if (hand.cards.Count == 1)
                InitialSetup();
            else
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
            GameManager.Instance.currentTm.NextPhase();
        }
    }

    private void HandStateCheck()
    {
        if (hand.IsBusted)
        {
            Debug.Log("Dealer BUSTED!");
            status = WinManager.PlayerStates.Bust;
            IsDealerTurn = false;
            GameManager.Instance.currentTm.NextPhase();
            return;
        }
        else if (hand.IsBlackjack)
        {
            Debug.Log("Dealer BLACKJACK!");
            status = WinManager.PlayerStates.Blackjack;
            IsDealerTurn = false;
            GameManager.Instance.currentTm.NextPhase();
            return;
        }
        else
        {
            status = WinManager.PlayerStates.Stand;

            if (hand.Is21) // If Hand is a 21 (not blackjack) the Dealer's turn ends
            {
                IsDealerTurn = false;
                GameManager.Instance.currentTm.NextPhase();
            }
        }
    }

    public void ResetHand()
    {
        status = WinManager.PlayerStates.Active;
        hand.Reset();
        Debug.Log($"Dealer's hand is reset");
    }


}
