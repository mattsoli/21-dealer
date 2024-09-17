using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int playerId;
    public float decisionThreshold = 16;
    [SerializeReference] public Hand hand = new();

    private bool isWaiting = false; // for a card
    private bool isStanding = false;

    // Start is called before the first frame update
    void Start()
    {
        Decide();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider coll)
    {
        // when a card is throwed to the player and is waiting for it, add the card to player's hand
        if (coll.gameObject.CompareTag("Card") && isWaiting)
        { 
            Card lastCard = coll.gameObject.transform.parent.GetComponent<PhysicalCard>().cardObject;
            Debug.Log(lastCard);
            hand.AddCard(lastCard);
            Decide();
        }
    }

    public void Decide()
    {
        if (hand.score > decisionThreshold) // player decides to ask another card is the score is less than the decision threshold
        {
            Stand();
        } else
        {
            Hit();
        }

    }

    public void Hit()
    {
        isWaiting = true;
        isStanding = false;
        Debug.Log("Player_" + playerId + " asks new card...");
    }

    public void Stand()
    {
        isStanding = true;
        isWaiting = false;
        Debug.Log("Player_" + playerId + " stands...");
    }

    public void ResetHand()
    {
        hand.cards.Clear();
        Debug.Log("Player_" + playerId + " hand is reset");
    }


}
