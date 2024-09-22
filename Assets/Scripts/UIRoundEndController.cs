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

    private void Start()
    {
        SetNextRoundButton();
    }

    // Set the each panel with the Players info
    private void SetPlayersPanel(Dictionary<Player, Outcome> outcomes)
    {
        SetTitleText();
        ToggleRoundButton();

        Player[] players = GameManager.Instance.GetPlayerList();

        for (int i = 0; i < players.Length; i++)
        {
            SetPlayerInfoInPanel(playerInfoPanels[i], players[i], outcomes[players[i]]);

            playerInfoPanels[i].SetActive(true);
        }
    }

    private void SetPlayerInfoInPanel(GameObject playerInfopanel, Player player, Outcome outcome)
    {
        // Set the Player's name text
        TMP_Text playerNameText = playerInfopanel.transform.Find("PlayerNameText").GetComponent<TMP_Text>();
        playerNameText.text = player.playerName;

        // Set the Player's final score text
        TMP_Text scoreText = playerInfopanel.transform.Find("ScoreText").GetComponent<TMP_Text>();
        scoreText.text = player.hand.score.ToString();

        // Set the Player's outcome
        TMP_Text outcomeText = playerInfopanel.transform.Find("OutcomeText").GetComponent<TMP_Text>();
        outcomeText.text = outcome.ToString();

        // Coloring the outcome based on itself
        if (outcome == Outcome.Win)
            outcomeText.color = Color.green;
        else if (outcome == Outcome.Lose)
            outcomeText.color = Color.red;
        else
            outcomeText.color = Color.cyan;
    }

    private void ToggleRoundButton()
    {
        if (GameManager.Instance.GetRoundPlayed() == GameManager.Instance.roundQuantity)
            nextRound.gameObject.SetActive(false);
    }

    private void SetNextRoundButton()
    {
        nextRound.onClick.AddListener(() => GameManager.Instance.CheckRoundNumber());
    }

    private void SetTitleText()
    {
        // "Round #/# Results"
        roundResultText.text = $"Round {GameManager.Instance.GetRoundPlayed()}/{GameManager.Instance.roundQuantity} Results";
    }

    private void GetWinResults(Dictionary<Player, Outcome> results)
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
