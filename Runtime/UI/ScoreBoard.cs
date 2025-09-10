using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class ScoreBoard : NetworkBehaviour
{
    [SerializeField] TextMeshProUGUI red;
    [SerializeField] TextMeshProUGUI blue;

    NetworkVariable<int> redScore = new();
    NetworkVariable<int> blueScore = new();

    [SerializeField] int matchPoint = 5;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        redScore.OnValueChanged += OnRedChanged;
        blueScore.OnValueChanged += OnBlueChanged;
        Bus<IAddPointEvent>.OnEvent += OnUpdateScore;
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();

        redScore.OnValueChanged -= OnRedChanged;
        blueScore.OnValueChanged += OnBlueChanged;
        Bus<IAddPointEvent>.OnEvent -= OnUpdateScore;
    }

    private void OnBlueChanged(int previousValue, int newValue)
    {
        blue.SetText(blueScore.Value.ToString());
    }

    private void OnRedChanged(int previousValue, int newValue)
    {
       red.SetText(redScore.Value.ToString());
    }

    private void OnUpdateScore(IAddPointEvent evt)
    {
        if(evt.team == Team.Blue)
        {
            redScore.Value++;
        }
        else
        {
            blueScore.Value++;
        }

        if(redScore.Value == matchPoint)
        {
            Bus<IMatchEndEvent>.Raise(new IMatchEndEvent(Team.Red));
        }
        else if(blueScore.Value == matchPoint)
        {
            Bus<IMatchEndEvent>.Raise(new IMatchEndEvent(Team.Blue));
        }
    }
}

