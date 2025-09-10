using UnityEngine;

public class IMatchEndEvent : IEvent
{
    public Team team; // Who win?

    public IMatchEndEvent(Team team)
    {
        this.team = team;
    }
}
