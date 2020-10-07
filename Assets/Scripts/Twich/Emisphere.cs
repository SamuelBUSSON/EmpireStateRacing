using System.Collections;
using System.Collections.Generic;
using Twitch;
using UnityEngine;

public class Emisphere
{
    private Robot _owner;
    public Stack<Buffer> buffer;
    public Buffer active;

    public Emisphere(Robot owner)
    {
        this._owner = owner;
        buffer = new Stack<Buffer>();
        active = null;
    }
    public void NextAction()
    {
        if (buffer.Count > 0)
        {
            active = buffer.Pop();
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
            buffer.Push(command);
            if (buffer.Count > _owner.maxBuffer) buffer.Pop();
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
        _owner.eventCommand.CallSetCurrentSpeed(_owner.type, paw,buffer.Count + ((stop)?0:1));
    }
    public void SendAction(string pseudo, int paw)
    {
        active = new Buffer(pseudo, paw);
        _owner.eventCommand.CallActivatePaw(_owner.type, pseudo, active.paw);
    }

    public class Buffer
    {
        public string pseudo;
        public int paw;

        public Buffer(string _pseudo, int _paw)
        {
            pseudo = _pseudo;
            paw = _paw;
        }
    }
}
