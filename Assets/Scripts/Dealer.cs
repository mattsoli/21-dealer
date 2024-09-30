using UnityEngine;

public class Dealer : MonoBehaviour
{
    public Hand hand = new();

    public bool isDealerTurn = false;

    public WinManager.PlayerStates state;

    void Start()
    {
        state = WinManager.PlayerStates.Active;

        ToggleEnablingHandCollider(false);
    }

    public void StandTurn() // The Dealer choose to accept their Hand
    {
        isDealerTurn = false;
        GameManager.Instance.currentTm.NextPhase(); // Next phase of the round
    }

    private void OnTriggerEnter(Collider other)
    {
        // When a card is throwed to the Dealer add the Card to Dealer's hand
        if (other.gameObject.CompareTag("Card") && isDealerTurn)
        {
            PhysicalCard lastCard = other.gameObject.GetComponent<PhysicalCard>();
            hand.AddCard(lastCard);

            Debug.Log("Dealer gets => " + lastCard.cardObject.ToString());

            if (hand.cards.Count <= 2) // Situation of the Initial Deal, the Dealer must have 2 Card
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

        Debug.Log("Dealer is waiting the initial setup...");
        isDealerTurn = true;

        if (hand.cards.Count == 2)
        {
            isDealerTurn = false;
            GameManager.Instance.currentTm.NextPhase(); // Initial Setup is finished, go forward
        }
        //else
        //{
        //}
    }

    private void HandStateCheck() // Check the state of the Hand
    {
        if (hand.IsBusted)
        {
            Debug.Log("Dealer BUSTED!");
            state = WinManager.PlayerStates.Bust;
            StandTurn();
            return;
        }
        else if (hand.IsBlackjack)
        {
            Debug.Log("Dealer BLACKJACK!");
            state = WinManager.PlayerStates.Blackjack;
            StandTurn();
            return;
        }
        else
        {
            state = WinManager.PlayerStates.Stand;

            if (hand.Is21) // If Hand is a 21 (not blackjack) the Dealer's stands automatically
            {
                StandTurn();
            }
        }
    }

    public void ResetHand()
    {
        ToggleEnablingHandCollider(false);
        state = WinManager.PlayerStates.Active;
        hand.Reset();
        Debug.Log($"Dealer's hand is reset");
    }
    private void ToggleEnablingHandCollider(bool toggle) // Actives and deactives the Hand collider
    {
        if (toggle)
            gameObject.GetComponent<BoxCollider>().enabled = true;
        else
            gameObject.GetComponent<BoxCollider>().enabled = false;
    }


}
