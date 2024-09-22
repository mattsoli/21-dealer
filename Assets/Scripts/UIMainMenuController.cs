using UnityEngine;
using UnityEngine.UI;

public class UIMainMenuController : MonoBehaviour
{
    public Slider playerCountSlider;
    public Slider roundToPlay;

    public void StartGame()
    {
        // Valorize the GameManager parameters with input values
        int playerCount = Mathf.RoundToInt(playerCountSlider.value);
        int roundCount = Mathf.RoundToInt(roundToPlay.value);

        GameManager.Instance.StartNewGame(playerCount, roundCount);
    }

    public void Quit()
    {
        GameManager.Instance.QuitGame();
    }
}
