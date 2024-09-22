using UnityEngine;
using UnityEngine.UI;

public class UIDeck : MonoBehaviour
{
    public GameObject deckPanel;
    public GameObject shufflePanel;

    public Button shuffleMenuButton;
    public Button shuffleButton;
    public Button cutButton;

    private Deck deck;
        private void OnEnable()
    {
        TurnManager.OnInitialDeal += CloseAllPanel;
    }

    private void OnDisable()
    {
        TurnManager.OnInitialDeal -= CloseAllPanel;
    }

    private void Awake()
    {
        deck = this.transform.parent.gameObject.GetComponent<Deck>();
    }

    void Start()
    {
        OpenDeckPanel();
    }

    // Functionality Management
    public void ShuffleDeck()
    {
        deck.Shuffle();
        OpenDeckPanel();
    }

    public void CutDeck()
    {
        deck.Cut();
        OpenDeckPanel();
    }

    // Panels Management
    public void CloseAllPanel()
    {
        deckPanel.SetActive(false);
        shufflePanel.SetActive(false);
    }

    public void OpenShufflePanel()
    {
        deckPanel.SetActive(false);
        shufflePanel.SetActive(true);
    }

    private void OpenDeckPanel()
    {
        deckPanel.SetActive(true);
        shufflePanel.SetActive(false);
    }
}
