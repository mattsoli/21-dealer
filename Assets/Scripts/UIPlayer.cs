using UnityEngine;
using TMPro;

public class UIPlayer : MonoBehaviour
{
    public TMP_Text playerNameText;
    public TMP_Text statusText;
    public TMP_Text scoreText;

    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = transform.parent.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            playerNameText.text = player.playerName;
            scoreText.text = GetScoreText();

            statusText.text = GetStateText();
        }
    }

    private string GetStateText()
    {
        string text;

        if (player.roundState == Player.RoundState.None)
            text = "";
        else
        {
            text = player.roundState.ToString();

            // Player state text color
            if (player.roundState == Player.RoundState.Hit)
                statusText.color = Color.cyan;
            else if (player.roundState == Player.RoundState.Stand)
                statusText.color = Color.yellow;
            else if(player.roundState == Player.RoundState.Bust)
                statusText.color = Color.red;
            else
                statusText.color = Color.gray;
        }

        return text;
    }

    private string GetScoreText()
    {
        string text = player.hand.score.ToString();

        if (player.hand.Is21)
            scoreText.color = Color.cyan;
        else if (player.hand.IsBusted)
            scoreText.color = Color.yellow;
        else if (player.hand.IsBlackjack)
        {
            scoreText.color = Color.green;
            text = "BLACKJACK";
        } 
        else
            scoreText.color = new Color(229, 229, 229);

        return text;
    }

}
