using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomController : NetworkBehaviour
{
    public static RoomController Instance;

    private Dictionary<PlayerRef, bool> _playerStates = new Dictionary<PlayerRef, bool>();

    [Networked] public bool isGameStart { get; set; } = false;

    private void Awake()
    {
        Instance = this;
    }

    public override void Spawned()
    {
        RpcAddPlayer(Runner.LocalPlayer);
        UIController.Instance.SetPlayerRef(Runner.LocalPlayer);
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RpcAddPlayer(PlayerRef player)
    {
        _playerStates.TryAdd(player, false);

        foreach (var state in _playerStates)
        {
            Debug.Log($"Player ref {state.Key}: {state.Value}");
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RpcOnPlayerConfirm(PlayerRef playerRef)
    {
        if (!_playerStates.ContainsKey(playerRef)) return;

        _playerStates[playerRef] = true;

        if (Runner.ActivePlayers.Count() < 2) return;

        var everyoneIsReady = true;

        foreach (var state in _playerStates)
        {
            if(state.Value == false)
                everyoneIsReady = false;

            Debug.Log($"Player ref {state.Key}: {state.Value}");
        }

        if(everyoneIsReady)
        {
            isGameStart = true;
            foreach (var actualPlayerRef in _playerStates.Keys)
            { 
                RpcSpawnPlayer(actualPlayerRef);
            }
        }
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RpcSpawnPlayer(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {
            PlayerSpawner.Instance.SpawnPlayer();
            UIController.Instance.StartGame();
        }
    }
}
