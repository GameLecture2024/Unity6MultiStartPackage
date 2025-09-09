using UnityEngine;
using NetworkB;
using TMPro;
public class NetworkInfo : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI joinCode;

    private void Start()
    {
        joinCode.SetText($"RoomCode : {HostSingleton.Instance.GameManager.JoinCode}");
    }
}
