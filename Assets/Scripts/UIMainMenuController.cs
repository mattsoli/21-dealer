using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenuController : MonoBehaviour
{
    public Slider playerCountSlider;

    private void Start()
    {
        
    }

    public void StartGame()
    {
        int playerCount = Mathf.RoundToInt(playerCountSlider.value);
        GameManager.Instance.StartNewGame(playerCount);
    }

    public void Quit()
    {
        GameManager.Instance.QuitGame();
    }
}
