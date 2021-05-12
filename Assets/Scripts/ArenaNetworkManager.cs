using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ArenaNetworkManager : NetworkManager
{
    [SerializeField] private GameObject enemyPrefab;

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);

        SpawnEnemy();
    }

    void SpawnEnemy()
    {
        GameObject newEnemy = Instantiate(enemyPrefab, new Vector3(.82f, 1, 15), Quaternion.identity);
        NetworkServer.Spawn(newEnemy);
    }
}


