using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenuController : MonoBehaviour
{
    public Slider playerCountSlider;
    public Button startGameButton;
    public Button quitButton;

    private void Start()
    {
        startGameButton.onClick.AddListener(StartGame);
    }

    private void StartGame()
    {
        int playerCount = Mathf.RoundToInt(playerCountSlider.value);
        GameManager.Instance.StartNewGame(playerCount);
    }

    private void Quit()
    {
        GameManager.Instance.QuitGame();
    }
}
