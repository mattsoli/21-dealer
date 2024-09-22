
using UnityEngine;
using TMPro;
using static GameManager;

public class GUIController : MonoBehaviour
{
    public TMP_Text turnText;
    public TMP_Text roundText;

    public GameObject pausePanel;
    public GameObject roundEndPanel;

    public GameObject notifyPanel;
    public TMP_Text notifyText;

    public GameObject guiPanel;

    void Update()
    {
        UpdateRoundText();
        UpdateTurnText();
    }

    private void OpenRoundEndPanel()
    {
        guiPanel.SetActive(false);
        roundEndPanel.SetActive(true);
    }

    public void CloseRoundEndPanel()
    {
        guiPanel.SetActive(true);
        roundEndPanel.SetActive(false);
    }

    public void ReturnToMainMenu()
    {
        GameManager.Instance.BackToMainMenu();
    }

    public void QuitGame()
    {
        GameManager.Instance.QuitGame();
    }

    public void TogglePauseMenu()
    {
        if (GameManager.Instance.currentGameState == GameState.Playing)
        {
            pausePanel.SetActive(true);
            GameManager.Instance.PauseGame();
        }
        else if (GameManager.Instance.currentGameState == GameState.Paused)
        {
            pausePanel.SetActive(false);
            GameManager.Instance.ResumeGame();
        }
    }

    private void UpdateTurnText()
    {
        string text = GameManager.Instance.GetCurrentTurn();
        if (text == "None")
            turnText.text = "";
        else
            turnText.text = $"{text} turn";
    }

    private void UpdateRoundText()
    {
        int round = GameManager.Instance.GetRoundPlayed();
        if (round == 0)
            roundText.text = "";
        else
            roundText.text = $"Round {round.ToString()}";
    }

    private void OnEnable()
    {
        TurnManager.OnEndRound += OpenRoundEndPanel;
    }

    private void OnDisable()
    {
        TurnManager.OnEndRound -= OpenRoundEndPanel;
    }


}
