using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class ClientNetworkTransform : NetworkTransform
{
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        CanCommitToTransform = IsOwner;
    }

    public override void OnUpdate()
    {
        CanCommitToTransform = IsOwner;
        base.OnUpdate();
        if(NetworkManager != null)
        {
            if(NetworkManager.IsConnectedClient || NetworkManager.IsListening)
            {
                if(CanCommitToTransform)
                {                  
                   
                }
            }
        }
    }

    protected override bool OnIsServerAuthoritative()
    {
        return false;
    }

    [ClientRpc]
    public void RequestServerOverrideClientRpc(Vector3 newPosition)
    {
        if(IsOwner)
        {
            transform.position = newPosition;
        }
    }

    public void ServerForceMove(Vector3 position)
    {
        if (IsServer)
        {
            RequestServerOverrideClientRpc(position);
        }
    }
}
