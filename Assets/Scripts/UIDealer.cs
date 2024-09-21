using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIDealer : MonoBehaviour
{
    public GameObject standPanel;
    public Button standButton;

    public GameObject startPanel;
    public Button startButton;

    public TMP_Text scoreText;

    private Dealer dealer;
    private TurnManager tm;

    // Start is called before the first frame update
    void Start()
    {
        dealer = transform.parent.GetComponent<Dealer>();
        tm = FindObjectOfType<TurnManager>();

        CloseStandPanel();
        OpenStartPanel();
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = GetScoreText();

        if (tm.currentPhase == TurnManager.GamePhase.MiddleDeal && dealer.IsDealerTurn)
            OpenStandPanel();
    }

    public void StandRound()
    {
        dealer.StandAndStart();
        CloseStandPanel();
        CloseStartPanel();
    }

    private void OpenStandPanel()
    {
        standPanel.SetActive(true);
    }

    private void OpenStartPanel()
    {
        startPanel.SetActive(true);
    }

    private void CloseStandPanel()
    {
        standPanel.SetActive(false);
    }

    private void CloseStartPanel()
    {
        startPanel.SetActive(false);
    }

    private string GetScoreText()
    {
        string text = dealer.hand.score.ToString();

        if (dealer.hand.Is21)
            scoreText.color = Color.cyan;
        else if (dealer.hand.IsBusted)
            scoreText.color = Color.yellow;
        else if (dealer.hand.IsBlackjack)
        {
            scoreText.color = Color.green;
            text = "BLACKJACK";
        }

        return text;
    }
}
