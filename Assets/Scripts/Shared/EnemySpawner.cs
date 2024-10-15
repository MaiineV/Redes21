using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : NetworkBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;

    [SerializeField] private float _spawnerTimer;

    public override void FixedUpdateNetwork()
    {
        if (RoomController.Instance != null && HasStateAuthority
            && RoomController.Instance.isGameStart)
        {
            _spawnerTimer += Runner.DeltaTime;

            if (_spawnerTimer > 5)
            {
                _spawnerTimer = 0;
                Runner.Spawn(_enemyPrefab);
            }
        }
    }
}
