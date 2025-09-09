using System;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public struct ScoreBoardEntity : INetworkSerializable
{
    public ulong ClinetId;
    public FixedString32Bytes PlayerName;
    public int Value;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        //serializer.SerializeValue
    }
}

public struct ScoreBoardEntityState : INetworkSerializable, IEquatable<ScoreBoardEntityState>
{
    public ulong ClientId;
    public FixedString32Bytes PlayerName;
    public int Value;

    public bool Equals(ScoreBoardEntityState other)
    {
        return ClientId == other.ClientId &&
            PlayerName.Equals(other.PlayerName);
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref ClientId);
        serializer.SerializeValue(ref Value);
    }
}
