using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private Transform[] spawnPoints;

    public void PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        { 
            Runner.Spawn(_playerPrefab, spawnPoints[Runner.ActivePlayers.Count() - 1].position, spawnPoints[Runner.ActivePlayers.Count() - 1].rotation);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
