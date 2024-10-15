using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] NetworkRunner runner;
    [SerializeField] GameObject readyButton;
    [SerializeField] GameObject readyText;

    public static UIController Instance;

    private PlayerRef player;

    private void Awake()
    {
        Instance = this;
    }

    public void SetPlayerRef(PlayerRef newPlayer)
    {
        player = newPlayer;
    }

    public void SetReady()
    {
        if (RoomController.Instance != null)
        {
            RoomController.Instance.RpcOnPlayerConfirm(player);
            readyButton.SetActive(false);
            readyText.SetActive(true);

        }
    }

    public void StartGame()
    {
        readyText.SetActive(false);
    }
}
