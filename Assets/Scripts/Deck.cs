using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public List<Card> cards = new();

    public GameObject physicalCardPrefab; // Reference of the PhysicalCard prefab (Assets/Prefabs)

    private List<Card> discards = new();

    private bool isDraggingCard = false; // Check if there's a card currently being dragged


    private void OnEnable()
    {
        TurnManager.OnInitialDeal += Initialize;
    }

    private void OnDisable()
    {
        TurnManager.OnInitialDeal -= Initialize;
    }

    void Start()
    {
        Sort();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) // ---------------> DEBUG DA TOGLIERE
        {
            Initialize();
        }

    }

    public void Initialize()
    {
        if (discards.Count > 0)
        {
            // Put the discarded cards on the deck's bottom
            cards.AddRange(discards);
            discards.Clear();

            List<PhysicalCard> physicalDiscards = new List<PhysicalCard>(FindObjectsOfType<PhysicalCard>());

            foreach (PhysicalCard pc in physicalDiscards)
            {
                Destroy(pc.gameObject);
            }
        }

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

    private void Sort()
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
            child.AddComponent<PhysicalCard>().cardObject = card;

            Debug.Log(card);
        }
    }


    public void StopDraggingCard()
    {
        isDraggingCard = false;
    }

    private void OnMouseDown()
    {
        if (isDraggingCard)
            return;

        // Check if the deck has cards left and no card is currently being dragged
        if (cards != null && cards.Count > 0)
        {
            // Instantiate the first card from the deck when clicking on the deck
            InstantiateFirstCard();
        }
        else if (cards == null || cards.Count == 0)
        {
            Debug.LogError("No Card available in the deck!");
        }
    }

    // Instantiate the first card in the deck and allow it to be dragged
    private void InstantiateFirstCard()
    {
        Card firstCard = cards[0]; // Get the first card from the list

        if (physicalCardPrefab == null) // Check if the PhysicalCard prefab is assigned
        {
            Debug.LogError("No PhysicalCard prefab!");
            return;
        }

        Vector3 spawnPoint = this.transform.position;
        spawnPoint.y = 1f;

        // Instantiate the card at the deck's position
        GameObject instantiatedPhysicalCard = Instantiate(physicalCardPrefab, spawnPoint, Quaternion.identity);

        instantiatedPhysicalCard.GetComponent<PhysicalCard>().cardObject = firstCard; // Add the Card to the instantiated PhysicalCard
        instantiatedPhysicalCard.GetComponent<PhysicalCard>().CardRenderer(); // Renderize the card

        cards.RemoveAt(0); // Remove the card from the deck list
        discards.Add(firstCard); // Add the removed card inside the discards

        isDraggingCard = true; // The player is dragging a card

    }

}
