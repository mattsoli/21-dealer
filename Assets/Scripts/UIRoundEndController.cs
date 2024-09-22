using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static WinManager;

public class UIRoundEndController : MonoBehaviour
{
    public TMP_Text roundResultText;
    public GameObject[] playerInfoPanels;
    public Button nextRound;


    void Start()
    {
        SetTitleText();
        SetNextRoundButton();
    }

    private void SetPlayersPanel(Dictionary<Player, Outcome> outcomes)
    {
        Player[] players = GameManager.Instance.GetPlayerList();

        for (int i = 0; i < players.Length; i++)
        {
            SetPlayerInfoInPanel(playerInfoPanels[i], players[i], outcomes[players[i]]);

            playerInfoPanels[i].SetActive(true);
        }
    }

    private void SetPlayerInfoInPanel(GameObject playerInfopanel, Player player, Outcome outcome)
    {
        TMP_Text playerNameText = playerInfopanel.transform.Find("PlayerNameText").GetComponent<TMP_Text>();
        playerNameText.text = player.playerName;

        TMP_Text scoreText = playerInfopanel.transform.Find("ScoreText").GetComponent<TMP_Text>();
        scoreText.text = player.hand.score.ToString();

        TMP_Text statusText = playerInfopanel.transform.Find("StatusText").GetComponent<TMP_Text>();
        statusText.text = outcome.ToString();

        if (outcome == Outcome.Win)
            statusText.color = Color.green;
        else if (outcome == Outcome.Lose)
            statusText.color = Color.red;
        else
            statusText.color = Color.cyan;
    }        

        private void SetNextRoundButton()
    {
        if (GameManager.Instance.GetRoundPlayed() == GameManager.Instance.roundQuantity)
            nextRound.gameObject.SetActive(false);
        else
            nextRound.onClick.AddListener(() => GameManager.Instance.CheckRoundNumber());
    }

    private void SetTitleText()
    {
        // "Round #/# Results"
        roundResultText.text = $"Round {GameManager.Instance.GetRoundPlayed()}/{GameManager.Instance.roundQuantity} Results";
    }

    private void GetWinResults(Dictionary<Player, Outcome> results) // ---------------> DA TOGLIERE
    {
        GameManager.Instance.GetCurrentTurn(GameManager.RoundTurn.None);

        foreach (var result in results)
        {
            Debug.Log($"{result}");
        }

        SetPlayersPanel(results);
    }

    private void OnEnable()
    {
        WinManager.OnWinConditionEvaluated += GetWinResults;
    }

    private void OnDisable()
    {
        WinManager.OnWinConditionEvaluated -= GetWinResults;
    }
}
