using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public List<Card> cards = new();

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        SortDeck();

        Debug.Log("Deck initialized");
    }

    public void Shuffle()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            Card temp = cards[i];
            int rand = Random.Range(i, cards.Count); // Shuffle by random order
            cards[i] = cards[rand];
            cards[rand] = temp;
        }

        Debug.Log("Deck shuffled");
    }

    public void Cut()
    {
        List<Card> temp1 = new();
        List<Card> temp2 = new();
        
        int rand = Random.Range(1, cards.Count);

        temp1 = cards.Skip(rand).Take(cards.Count).ToList(); // Half deck to put above
        temp2 = cards.Take(rand).ToList(); // Other half to put below
        
        cards.Clear();
        cards = temp1.Concat(temp2).ToList();
        
        Debug.Log("Deck cutted from position #" + rand);
    }

    public bool IsEmpty()
    {
        return cards.Count == 0;
    }

    private void SortDeck()
    {
        cards = cards.OrderBy(card => card.suit)  // Sort by suit order
                        .ThenBy(card => card.value)  // Sort by value within each suit
                        .ToList();
    }

    public void DisplayDeck()
    {
        foreach (Card card in cards)
        {
            GameObject child = new();
            child.transform.SetParent(this.transform, false);
            child.AddComponent<CardDisplay>().cardObject = card;

            Debug.Log(card);
        }
    }


}
