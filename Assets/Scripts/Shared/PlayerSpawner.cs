using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject _enemySpawner;
    [SerializeField] private GameObject _roomController;

    [SerializeField] private GameObject _readyButton;

    public static PlayerSpawner Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {
            _readyButton.SetActive(true);
            if (Runner.ActivePlayers.Count() == 1)
            {
                Runner.Spawn(_roomController);
                Runner.Spawn(_enemySpawner);
            }
            //Runner.Spawn(_playerPrefab, spawnPoints[Runner.ActivePlayers.Count() - 1].position, spawnPoints[Runner.ActivePlayers.Count() - 1].rotation);
        }
    }

    public void SpawnPlayer()
    {
        Runner.Spawn(_playerPrefab, spawnPoints[0].position, spawnPoints[0].rotation);
    }
}
