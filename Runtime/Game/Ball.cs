using System;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class Ball : NetworkBehaviour
{
    Rigidbody rb;
    [SerializeField] private float bounceForce = 5f;
    [SerializeField] MeshRenderer meshRenderer;
    private NetworkRigidbody networkRB;
    public override void OnNetworkSpawn()
    {
        rb = GetComponent<Rigidbody>();
        networkRB= GetComponent<NetworkRigidbody>();    
        networkRB.SetIsKinematic(true);

        if (IsServer)
            GameManager.instace.OnAllPlayersJoined += HandleGameReady;

        if(!IsServer)
        {
            networkRB.SetIsKinematic(true);
        }
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        if (IsServer)
            GameManager.instace.OnAllPlayersJoined -= HandleGameReady;
    }

    private void HandleGameReady()
    {
        if(IsServer)
            networkRB.SetIsKinematic(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 normal = collision.contacts[0].normal;
        rb.AddForce(normal * bounceForce, ForceMode.Impulse);

        if(collision.collider.CompareTag("Blue"))
        {
            Bus<IAddPointEvent>.Raise(new IAddPointEvent(Team.Blue));
            rb.isKinematic = true;
        }
        else if(collision.collider.CompareTag("Red"))
        {
            Bus<IAddPointEvent>.Raise(new IAddPointEvent(Team.Red));
            networkRB.SetLinearVelocity(Vector3.zero);
            rb.isKinematic = true;
        }
    }

    public void SetBallMove(bool stop)
    {
        rb.isKinematic = stop;
    }

}
