using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
            scoreText.text = player.hand.score.ToString();

            if (player.status == WinManager.PlayerStatus.Active)
                statusText.text = "Waiting";
            else
                statusText.text = player.status.ToString();
        }
    }
}
