using System;
using System.Collections;
using System.Collections.Generic;
using Twitch;
using UnityEngine;

[SerializeField]
public class Emisphere
{
    private Robot _owner;
    public Buffer active;

    public Emisphere(Robot owner)
    {
        this._owner = owner;
        _owner.buffer = new List<Buffer>();
        active = null;
    }
    public void NextAction()
    {
        if (_owner.buffer.Count > 0)
        {
            active = _owner.buffer[0];
            _owner.buffer.RemoveAt(0);
            SendAction(active.pseudo, active.paw);
            SendSpeed(active.paw);
        }
        else
        {
            SendSpeed(active.paw, true);
            active = null;
        }

    }

    public void Command(string pseudo, int paw)
    {
        Buffer command = new Buffer(pseudo, paw);
        if (active != null)
        {
            _owner.buffer.Add(command);
            if (_owner.buffer.Count > _owner.maxBuffer) _owner.buffer.RemoveAt(0);
        }
        else
        {
            active = command;
            SendAction(pseudo, paw);
        }

        SendSpeed(active.paw);
    }

    
    public void SendSpeed(int paw, bool stop = false)
    {
        _owner.eventCommand.CallSetCurrentSpeed(_owner.type, paw,_owner.buffer.Count + ((stop)?0:1));
    }
    public void SendAction(string pseudo, int paw)
    {
        active = new Buffer(pseudo, paw);
        _owner.eventCommand.CallActivatePaw(_owner.type, pseudo, active.paw);
    }
}
