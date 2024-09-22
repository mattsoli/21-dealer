using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dealer : MonoBehaviour
{
    public Hand hand = new();

    public bool isDealerTurn = false;

    public WinManager.PlayerStates status;

    // Start is called before the first frame update
    void Start()
    {
        status = WinManager.PlayerStates.Active;

        ToggleEnablingHandCollider(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StandTurn()
    {
        isDealerTurn = false;
        GameManager.Instance.currentTm.NextPhase();
    }

    private void OnTriggerEnter(Collider other)
    {
        // When a card is throwed to the Dealer add the Card to Dealer's hand
        if (other.gameObject.CompareTag("Card") && isDealerTurn)
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

    public void IsDealerTurn()
    {
        isDealerTurn = true;
    }

    public void InitialSetup()
    {
        ToggleEnablingHandCollider(true);

        if (hand.cards.Count < 1)
        {
            Debug.Log("Dealer is waiting the initial setup...");
            isDealerTurn = true;
        }
        else
        {
            isDealerTurn = false;
            GameManager.Instance.currentTm.NextPhase();
        }
    }

    private void HandStateCheck()
    {
        if (hand.IsBusted)
        {
            Debug.Log("Dealer BUSTED!");
            status = WinManager.PlayerStates.Bust;
            isDealerTurn = false;
            GameManager.Instance.currentTm.NextPhase();
            return;
        }
        else if (hand.IsBlackjack)
        {
            Debug.Log("Dealer BLACKJACK!");
            status = WinManager.PlayerStates.Blackjack;
            isDealerTurn = false;
            GameManager.Instance.currentTm.NextPhase();
            return;
        }
        else
        {
            status = WinManager.PlayerStates.Stand;

            if (hand.Is21) // If Hand is a 21 (not blackjack) the Dealer's turn ends
            {
                isDealerTurn = false;
                GameManager.Instance.currentTm.NextPhase();
            }
        }
    }

    public void ResetHand()
    {
        ToggleEnablingHandCollider(false);
        status = WinManager.PlayerStates.Active;
        hand.Reset();
        Debug.Log($"Dealer's hand is reset");
    }
    private void ToggleEnablingHandCollider(bool toggle)
    {
        if (toggle)
            gameObject.GetComponent<BoxCollider>().enabled = true;
        else
            gameObject.GetComponent<BoxCollider>().enabled = false;
    }


}
