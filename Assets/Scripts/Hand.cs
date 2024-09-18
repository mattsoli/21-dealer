using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand
{
    public List<PhysicalCard> cards = new();
    public bool isBusted = false;
    public bool isBlackjack = false;
    public int score = 0;

    public void AddCard(PhysicalCard card)
    {
        if (isBusted || isBlackjack) // the Card can only be picked if the hand is not busted or is not a blackjack
        {
            Debug.LogWarning("Can't draw a card! Hand busted or blackjack");
            return;
        }

        card.isInHand = true; // The PhysicalCard is now in an Hand
        cards.Add(card);
        score = GetScore();

        if (score > 21)
        {
            isBusted = true;
            Debug.Log("Hand busted!");
        }
        else if (score == 21)
        {
            isBlackjack = true;
            Debug.Log("Blackjack!");
        }
    }

    public int GetScore()
    {
        int tempScore = 0;
        int aceCount = 0;

        // First pass: calculate total score and count the Aces
        foreach (PhysicalCard phCard in cards)
        {
            Card card = phCard.cardObject;

            if (card.value == Value.Ace) // Aces in the hand
            {
                tempScore += 11;
                aceCount++;
            }
            else if (card.value > Value.Ten) // face cards in the hand
            {
                tempScore += 10;
            }
            else // all the other cards
            {
                tempScore += (int)card.value;
            }
        }

        // Second pass: adjust Aces if the total score exceeds 21
        while (tempScore > 21 && aceCount > 0)
        {
            tempScore -= 10;  // Convert an Ace value from 11 to 1
            aceCount--;
        }

        Debug.Log($"Get Score: {tempScore}");

        return tempScore;
    }

}
