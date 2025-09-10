using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager instace;

    public int maxPlayers = 2;

    public delegate void AllPlayerJoined();
    public event AllPlayerJoined OnAllPlayersJoined;

    private void Awake()
    {
        if (instace != null && instace != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instace = this;
        }
    }

    void Update()
    {
        if (!IsServer) { return; }

        if (NetworkManager.Singleton.ConnectedClients.Count == maxPlayers)
        {
            OnAllPlayersJoined?.Invoke();
        }
    }
}
