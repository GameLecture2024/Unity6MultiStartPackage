using NetworkB;
using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using WebSocketSharp;

public class MatchEnd : NetworkBehaviour
{
    [SerializeField] private GameObject matchPanel;
    [SerializeField] private TextMeshProUGUI matchEndText;

    public void LeaveGame()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            HostSingleton.Instance.GameManager.Shutdown();
        }

        ClientSingleton.Instance.GameManager.Disconnect();
    }

    public override void OnNetworkSpawn()
    {
        Bus<IMatchEndEvent>.OnEvent += HandleMatchEnd;

        matchPanel.SetActive(false);
    }

    public override void OnNetworkDespawn()
    {
        Bus<IMatchEndEvent>.OnEvent -= HandleMatchEnd;
    }

    private void HandleMatchEnd(IMatchEndEvent evt)
    {
        ActivateServerForceUI(true , evt.team.ToString());     
    }

    [ClientRpc]
    public void ActivateUIClientRpc(bool show, string team)
    {
        matchPanel.SetActive(show);

        matchEndText.SetText($"{team.ToString()} WIN!!");
    }

    void ActivateServerForceUI(bool show, string team)
    {
        if (IsServer)
        {
            ActivateUIClientRpc(show, team);
        }
    }
}
