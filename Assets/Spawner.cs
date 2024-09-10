using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;
using UnityEngine.SceneManagement;

public class Spawner : MonoBehaviour, INetworkRunnerCallbacks
{
    Dictionary<PlayerRef, NetworkObject> _players = new Dictionary<PlayerRef, NetworkObject>();

    [SerializeField] private NetworkObject _playerPrefab;

    [SerializeField] Transform[] spawnPoints;

    NetworkRunner _runner;

    async void StartGame(GameMode gameMode)
    {
        _runner = gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;


        var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
        var sceneInfo = new NetworkSceneInfo();
        if (scene.IsValid)
        {
            sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
        }

        await _runner.StartGame(new StartGameArgs
        {
            GameMode = gameMode,
            SessionName = "Test",
            Scene = scene,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
    }
    
    #region Not Used
    public void OnConnectedToServer(NetworkRunner runner)
    {
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
    }
    #endregion

    #region OnPlayer Events
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if(runner.IsServer)
        {
            var spawnedPlayer = runner
                .Spawn(_playerPrefab, spawnPoints[_players.Count].position, Quaternion.identity, player);
            //spawnedPlayer.GetComponent<Player>().Init();
            _players.Add(player, spawnedPlayer);
        }
    }


    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (_players.ContainsKey(player))
        {
            runner.Despawn(_players[player]);
            _players.Remove(player);
        }
    }
    #endregion

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        var inputData = new NetworkInputData();
        inputData.dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        inputData.jumpInput = Input.GetKeyDown(KeyCode.Space);
        inputData.shootInput = Input.GetMouseButtonDown(0);

        input.Set(inputData);
    }

    #region Buttons
    public void HostGame()
    {
        StartGame(GameMode.Host);
    }

    public void JoinGame()
    {
        StartGame(GameMode.Client);
    }
    #endregion
}
