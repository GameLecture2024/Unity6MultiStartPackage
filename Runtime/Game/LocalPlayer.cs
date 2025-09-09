using NetworkB;
using System;
using Unity.Collections;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;
using UnityEngine.InputSystem;

public class LocalPlayer : NetworkBehaviour
{
    [SerializeField] float moveSpeed;

    Rigidbody rb;

    public NetworkVariable<int> TeamID = new NetworkVariable<int>(0);

    public NetworkVariable<FixedString32Bytes> PlayerName = new NetworkVariable<FixedString32Bytes>();

    public static event Action<LocalPlayer> OnPlayerSpawned;

    private ClientNetworkTransform networkTransform;

    public override void OnNetworkSpawn()
    {
        networkTransform = GetComponent<ClientNetworkTransform>();
        if (IsServer)
        {
            UserData userData =
                HostSingleton.Instance.GameManager.NetworkServer.GetUserDataByClientId(OwnerClientId);

            TeamID.Value = NetworkManager.Singleton.ConnectedClients.Count % 2 == 0 ? 1 : 2;


            OnPlayerSpawned?.Invoke(this);       
        }

        UpdateTeamVisual(TeamID.Value);
    }


    public void UpdateTeamVisual(int team)
    {
        Renderer playerRenderer = GetComponent<Renderer>();
        if(playerRenderer != null)
        {
            if (team == 1)
            {
                playerRenderer.material.color = Color.red;
            }
            else if (team == 2)
            {
                playerRenderer.material.color = Color.blue;
            }
            else
            {
                playerRenderer.material.color = Color.gray;
            }
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!IsOwner) return;
        Move();
    }

    private void Move()
    {
        float horizon = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 MoveMent = new Vector3(horizon, vertical,0 ).normalized;

        rb.linearVelocity = MoveMent * moveSpeed;
    }

    private void OnPositionChanged(Vector3 previousValue, Vector3 newValue)
    {
        // 클라이언트에서 실제 Transform 위치 업데이트
        transform.position = newValue;
    }

    public void SetPosition(Vector3 newPosition)
    {
        if (!IsServer) return;

        if(networkTransform != null)
        {
            networkTransform.ServerForceMove(newPosition);
        }
        else{
            transform.position = newPosition;
        }
    }
}
