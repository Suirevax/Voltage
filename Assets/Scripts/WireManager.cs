using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.XR;

public class WireManager : MonoBehaviour
{
    private readonly List<List<Wire>> _wireNodes = new List<List<Wire>>();

    //TODO: change it so powered state is only updated when it needs to. When wire added/ deleted, prompted by the player(?). Maybe even only for the nodes that changed
    private void Update()
    {
        //power on/ disable wireNodes
        foreach (var wireNode in _wireNodes)
        {
            ChangePowerWireNode(IsNodeSourced(wireNode), wireNode);
        }
    }



    public void CreatedWire(Wire newWire)
    {
        //check in what node the wire belongs in node & if it already exists
        var inWireNodes = CheckIfInNodes(newWire);

        //add wire to node or create new node
        if (inWireNodes.Count == 0)
        {
            //create new node
            _wireNodes.Add(new List<Wire>{newWire});
        }else if (inWireNodes.Count == 1)
        {
            //add to exisitng node
            inWireNodes[0].Add(newWire);
        }
        else
        {
            //join nodes together
            for (int i = 1; i < inWireNodes.Count; i++)
            {
                inWireNodes[0].AddRange(inWireNodes[i]);
                _wireNodes.Remove(inWireNodes[i]);
            }
            inWireNodes[0].Add(newWire);
        }
    }

    private List<List<Wire>> CheckIfInNodes(Wire newWire)
    {
        var inNodes = new List<List<Wire>>();
        if (_wireNodes.Count == 0) return inNodes;
        foreach (var wireNode in _wireNodes)
        {
            foreach (var wire in wireNode)
            {
                //check if wire is already part of an existing node
                if (wire.startPoint == newWire.startPoint || wire.endPoint == newWire.endPoint ||
                    wire.startPoint == newWire.endPoint || wire.endPoint == newWire.startPoint)
                {
                    inNodes.Add(wireNode);
                }
            }
        }
        
        return inNodes;
    }

    private static bool IsNodeSourced(List<Wire> wireNode)
    {
        foreach (var wire in wireNode)
        {
            if (wire.startPoint.Sourcing || wire.endPoint.Sourcing) return true;
        }
        return false;
    }
    
    private static void ChangePowerWireNode(bool value, List<Wire> wireNode)
    {
        foreach (var wire in wireNode)
        {
            wire.endPoint.Power = value;
            wire.startPoint.Power = value;
        }
    }

    public void WireDeleted(Wire wire)
    {
        if(wire.endPoint) wire.endPoint.Power = false;
        if(wire.startPoint) wire.startPoint.Power = false;

        for (int i = 0; i < _wireNodes.Count; i++)
        {
            _wireNodes[i].Remove(wire);
            if (_wireNodes[i].Count == 0) _wireNodes.RemoveAt(i--);
        }
    }
}
