using System;
using Unity.Netcode;
using UnityEngine;

public class BallSpawner : NetworkBehaviour
{
    [SerializeField] private Ball ballPrefab;


    public override void OnNetworkSpawn()
    {
        if (!IsServer) { return; }

        Bus<IAddPointEvent>.OnEvent += HandlePointed;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsServer) { return; }

        Bus<IAddPointEvent>.OnEvent -= HandlePointed;
    }

    private void HandlePointed(IAddPointEvent evt)
    {
        ballPrefab.SetBallMove(false);
        SpawnCoin(evt.team);
    }

    private void SpawnCoin(Team team)
    {
        //Ball ballInstance = Instantiate(ballPrefab, GetSpawnPoint(team), Quaternion.identity);
        //
        //ballInstance.GetComponent<NetworkObject>().Spawn();

        ballPrefab.transform.position = GetSpawnPoint(team);
    }

    private Vector3 GetSpawnPoint(Team team)
    {

        if(team == Team.Blue)
        {
            return new Vector3(-4, 0, 0);
        }
        else
        {
            return new Vector3(4, 0, 0);
        }
    }
}
