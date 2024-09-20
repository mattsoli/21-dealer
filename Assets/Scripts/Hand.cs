using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand
{
    public HashSet<PhysicalCard> cards = new();
    public int score = 0;

    private bool isBusted = false;
    public bool IsBusted { get; set; }

    private bool isBlackjack = false;
    public bool IsBlackjack { get; set; }

    private bool is21 = false;
    public bool Is21 { get; set; }

    public void AddCard(PhysicalCard card)
    {
        if (IsBusted || IsBlackjack || Is21) // the Card can only be picked if the hand is not busted or is not a blackjack
        {
            Debug.LogWarning("Can't draw a card! Hand busted or blackjack");
            return;
        }
                
        cards.Add(card);
        card.IsInHand = true; // The PhysicalCard is now in an Hand

        HandInfoUpdate();
    }

    private void HandInfoUpdate()
    {
        score = GetScore(); // Calculate the score of this hand

        if (score > 21) // If score > 21 is busted
        {
            IsBusted = true;
            Debug.Log("Hand busted!");
        }
        else if (score == 21)
        {
            if (cards.Count == 2) // If score = 21 with 2 cards is Blackjack
            {
                IsBlackjack = true;
                Debug.Log("Hand Blackjack!");
            }
            else // If score = 21 with more card is 21
            {
                Is21 = true;
                Debug.Log("Hand 21!");
            }
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

        Debug.Log($"Hand Score: {tempScore}");

        return tempScore;
    }

}
