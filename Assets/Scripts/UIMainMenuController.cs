using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenuController : MonoBehaviour
{
    public Slider playerCountSlider;
    public Slider roundToPlay;

    private void Start()
    {
        
    }

    public void StartGame()
    {
        int playerCount = Mathf.RoundToInt(playerCountSlider.value);
        int roundCount = Mathf.RoundToInt(roundToPlay.value);

        GameManager.Instance.StartNewGame(playerCount, roundCount);
    }

    public void Quit()
    {
        GameManager.Instance.QuitGame();
    }
}
