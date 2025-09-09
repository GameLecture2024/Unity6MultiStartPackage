using UnityEngine;

public enum Team
{
    Blue, Red
}


public class IAddPointEvent : IEvent
{
    public Team team;

    public IAddPointEvent(Team team)
    {
        this.team = team;
    }
}
