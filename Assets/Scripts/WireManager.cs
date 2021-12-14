using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using UnityEngine;
using UnityEngine.XR;

//TODO: Probably better way to inherit all the functions of List<T>
//This is necessary to show the wirenodes in editor.
[Serializable]
public class Node : IEnumerable<Wire>
{
    [SerializeField] private List<Wire> wires = new List<Wire>();
    
    public void Add(Wire wire) => wires.Add(wire);
    public IEnumerable<Wire> Union(IEnumerable<Wire> newWires) => wires.Union(newWires);
    public void Clear() => wires.Clear();
    public int Count => wires.Count;
    public void AddRange(IEnumerable<Wire> newWires) => wires.AddRange(wires);
    public bool Remove(Wire wire) => wires.Remove(wire);
    //TODO: override operator=
    public void Set(List<Wire> newNode) => wires = newNode;
    public IEnumerator<Wire> GetEnumerator() => wires.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    
}
public class WireManager : MonoBehaviour 
{
    [SerializeField] private List<Node> _wireNodes = new List<Node>();
    
    //TODO: change it so powered state is only updated when it needs to. When wire added/ deleted, prompted by the player(?). Maybe even only for the nodes that changed
    private void Update()
    {
        //power on/off wireNodes
        _wireNodes.ForEach(wireNode => SetPowerWireNode(IsNodeSourced(wireNode), wireNode));
    }

    public void CreatedWire(Wire newWire)
    {
        var inWireNodes = CheckIfInNodes(newWire);
        
        //add wire to node or create new node
        switch (inWireNodes.Count)
        {
            case 0:
                //create new node
                _wireNodes.Add(new Node{newWire});
                break;
            case 1:
                //add to existing node
                inWireNodes[0].Add(newWire);
                break;
            default:
            {
                //join the two nodes together
                //TODO: Check if C# has begin() & end() Iterator stuff
                for (var i = 1; i < inWireNodes.Count; i++)
                {
                    inWireNodes[0].AddRange(inWireNodes[i]);
                    _wireNodes.Remove(inWireNodes[i]);
                }
                inWireNodes[0].Add(newWire);
                break;
            }
        }
    }

    private List<Node> CheckIfInNodes(Wire newWire)
    {
        var inNodes = new List<Node>();
        if (_wireNodes.Count == 0) return inNodes;
        foreach (var wireNode in _wireNodes)
        {
            foreach (var wire in wireNode)
            {
                //check if wire is already part of an existing node
                if ((wire.wirePoints.Contains(newWire.StartPoint) || wire.wirePoints.Contains(newWire.EndPoint))
                    && !inNodes.Contains(wireNode)) 
                    inNodes.Add(wireNode);
            }
        }
        return inNodes;
    }

    private static bool IsNodeSourced(IEnumerable<Wire> wireNode)
    {
        return wireNode.Any(wire => wire.StartPoint.Sourcing || wire.EndPoint.Sourcing);
    }
    
    private static void SetPowerWireNode(bool value, IEnumerable<Wire> wireNode)
    {
        foreach (var wire in wireNode)
        {
            wire.EndPoint.Power = value;
            wire.StartPoint.Power = value;
        }
    }

    public void WireDeleted(Wire wire)
    {
        if(wire.EndPoint) wire.EndPoint.Power = false;
        if(wire.StartPoint) wire.StartPoint.Power = false;
        
        // Find and remove wire
        var parentNode = FindParentNode(wire);
        if (parentNode == null) return;
        parentNode.Remove(wire);

        switch (parentNode.Count)
        {
            //if node empty delete it
            case 0:
                _wireNodes.Remove(parentNode);
                return;
            //If the node consists of only 1 wire it unnecessary to recalculate nodes
            case 1:
                return;
            default:
                var startWireBuff = new Node();
                var wirePointBuff = new List<WirePoint>();
                RecalculateNodeRecursive(wire.StartPoint, ref startWireBuff, ref wirePointBuff);
                
                //If true then it's still one cohesive node so it's not necessary to recalculate the other node.
                if (wirePointBuff.Contains(wire.EndPoint) || startWireBuff.Count == 0) return;
                Debug.Log("End: " + startWireBuff.Count);
                
                _wireNodes.Remove(parentNode);
                _wireNodes.Add(startWireBuff);
        
                var endWireBuff = new Node();
                var endWirePointBuff = new List<WirePoint>();
        
                RecalculateNodeRecursive(wire.EndPoint, ref endWireBuff, ref endWirePointBuff);
        
                _wireNodes.Add(endWireBuff);
                return;
        }
    }
    
    //Recursive function to form a node from a wirePoint
    private void RecalculateNodeRecursive(WirePoint wirePoint, ref Node wireBuff, ref List<WirePoint> wirePointBuff)
    {
        var tmp = FindWiresConnectedToPoint(wirePoint);
        if (tmp.Count == 0) return;
        var newPoints = new List<WirePoint>();
        
        foreach (var wire in tmp)
        {
            if (!wirePointBuff.Contains(wire.EndPoint))
            {
                wirePointBuff.Add(wire.EndPoint);
                newPoints.Add(wire.EndPoint);
            }

            if (!wirePointBuff.Contains(wire.StartPoint))
            {
                wirePointBuff.Add(wire.StartPoint);
                newPoints.Add(wire.StartPoint);
            }
        }
        
        wireBuff.Set((wireBuff.Union(tmp).ToList()));

        foreach (var point in newPoints)
        {
            RecalculateNodeRecursive(point, ref wireBuff, ref wirePointBuff);
        }
    }

    private List<Wire> FindWiresConnectedToPoint(WirePoint wirePoint)
    {
        var allWires = GetComponentsInChildren<Wire>();

        return allWires.Where(wire => wire.wirePoints.Contains(wirePoint)).ToList();
    }

    private Node FindParentNode(Wire wire)
    {
        //TODO: how does this exactly work?
        return _wireNodes.FirstOrDefault(node => node.Contains(wire));
    }
}
