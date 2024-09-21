using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static GameManager;

public class GUIController : MonoBehaviour
{
    public TMP_Text turnText;
    public TMP_Text roundText;

    public GameObject pausePanel;

    public GameObject notifyPanel;
    public TMP_Text notifyText;

    void Start()
    {
        
    }

    void Update()
    {
        UpdateRoundText();
        UpdateTurnText();
        
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
}
