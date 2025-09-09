using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager instace;

    public int maxPlayers = 2;

    public delegate void AllPlayerJoined();
    public event AllPlayerJoined OnAllPlayersJoined;

    void Awake()
    {
        instace = this;

        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (NetworkManager.Singleton.ConnectedClients.Count == maxPlayers)
        {
            OnAllPlayersJoined?.Invoke();
        }
    }
}
