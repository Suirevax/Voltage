using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wire : MonoBehaviour
{
    public WirePoint startPoint;
    public WirePoint endPoint;

    private void OnDestroy()
    {
        GameObject.Find("WireManager").GetComponent<WireManager>().WireDeleted(this);
        //if(startPoint) startPoint.wireConnections.Remove(this);
        //if(endPoint) endPoint.wireConnections.Remove(this);
    }
}
