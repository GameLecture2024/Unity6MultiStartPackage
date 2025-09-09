using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PointHandler : NetworkBehaviour
{
    [SerializeField] NetworkObject playerPrefab;
    List<LocalPlayer> players = new();

    public override void OnNetworkSpawn()
    {
        if (!IsServer) { return; }
        LocalPlayer.OnPlayerSpawned += HandleSpawned;
        Bus<IAddPointEvent>.OnEvent += OnPointed;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsServer) { return; }
        LocalPlayer.OnPlayerSpawned -= HandleSpawned;
        Bus<IAddPointEvent>.OnEvent -= OnPointed;
    }


    private void OnPointed(IAddPointEvent evt)
    {
        MovePoint();
    }

    [ServerRpc(RequireOwnership = false)]
    public void MoveServerRpc(ulong clientId, Vector3 targetPos)
    {
        if (!IsServer) return;

        if(NetworkManager.Singleton.ConnectedClients.TryGetValue(clientId, out var client))
        {
            var LocalPlayer = client.PlayerObject.GetComponent<LocalPlayer>();
            if (LocalPlayer != null)
            {
                LocalPlayer.SetPosition(targetPos);
            }
        }
    }

    private void MovePoint()
    {
        foreach (var player in players)
        {
            if (player.TeamID.Value == 1)
            {
                // red 
                MoveServerRpc(player.OwnerClientId, new Vector3(7, 0, 0));
                Debug.Log($"TeamID1ÀÇ ID : {player.OwnerClientId}");
            }
            else
            {
                MoveServerRpc(player.OwnerClientId, new Vector3(-7, 0, 0));
                Debug.Log($"TeamID2ÀÇ ID : {player.OwnerClientId}");
            }
        }
    }

    [ClientRpc]
    private void MoveClientRpc()
    {
        if (IsOwner) { return; }

        MovePoint();
    }

    private void HandleSpawned(LocalPlayer player)
    {
        var findPlayers = FindObjectsByType<LocalPlayer>(FindObjectsSortMode.None);
        foreach(var p in findPlayers)
        {
            players.Add(p);
        }
    }


}
