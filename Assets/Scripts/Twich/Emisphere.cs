using System.Collections;
using System.Collections.Generic;
using Twitch;
using UnityEngine;

public class Emisphere
{
    private Robot _owner;
    public Stack<int> buffer;
    public int active;

    public Emisphere(Robot owner)
    {
        this._owner = owner;
        buffer = new Stack<int>();
        active = -1;
    }
    public void NextAction()
    {
        if (buffer.Count > 0)
        {
            active = buffer.Pop();
            SendAction(active);
        }
        else active = -1;

        SendSpeed();
    }

    public void Command(string pseudo, int paw)
    {
        if (active >= 0)
        {
            buffer.Push(paw);
            if (buffer.Count > _owner.maxBuffer) buffer.Pop();
        }
        else
        {
            active = paw;
            SendAction(paw);
        }

        SendSpeed();
    }

    
    public void SendSpeed()
    {
        _owner.eventCommand.CallSetCurrentSpeed(_owner.type, active,buffer.Count + 1);
    }
    public void SendAction(int paw)
    {
        active = paw;
        _owner.eventCommand.CallActivatePaw(_owner.type, active);
    }
}
