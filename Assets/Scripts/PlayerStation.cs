using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Collections.Specialized.BitVector32;

public class PlayerStation : MonoBehaviour
{
    public int stationId;
    private Transform playerSpawnPoint;
    public Transform handAreaTransform;

    private Player associatedPlayer;
    public GameObject playerCanvas;

    //public Player playerPrefab; // ----------> DEBUG DA TOGLIERE

    private void Awake()
    {
        playerSpawnPoint = transform;

        if (handAreaTransform == null)
        {
            handAreaTransform = transform.Find("HandArea");
            if (handAreaTransform == null)
            {
                handAreaTransform = new GameObject("HandArea").transform;
                handAreaTransform.SetParent(transform);
                handAreaTransform.localPosition = Vector3.forward;
            }
        }
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.P)) // ----------> DEBUG DA TOGLIERE
        //{
        //    AssignPlayer(playerPrefab);
        //}
    }

    public Player AssignPlayer(Player player)
    {
        // Spawns the player to the relative position of the PlayerStation object
        associatedPlayer = Instantiate(player);
        associatedPlayer.playerId = stationId;
        associatedPlayer.transform.position = playerSpawnPoint.position;
        associatedPlayer.transform.rotation = playerSpawnPoint.rotation;

        AssignCanvas();

        AlignPlayerHandCollider();

        return associatedPlayer;
    }

    private void AlignPlayerHandCollider()
    {
        if (associatedPlayer != null && associatedPlayer.hand != null)
        {
            BoxCollider handCollider = associatedPlayer.GetComponent<BoxCollider>();
            if (handCollider != null)
            {
                handCollider.center = handAreaTransform.localPosition;
                handCollider.size = handAreaTransform.localScale;
            }
            else
            {
                Debug.LogWarning("Player.Hand doesn't have a BoxCollider");
            }
        }
    }

    private void AssignCanvas()
    {
        GameObject canvas = Instantiate(playerCanvas) as GameObject;
        canvas.transform.SetParent(associatedPlayer.transform);
        canvas.transform.localPosition = playerCanvas.transform.position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.5f);

        if (handAreaTransform != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(handAreaTransform.position, handAreaTransform.localScale);
        }
    }
}
