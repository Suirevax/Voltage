using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wire : MonoBehaviour
{
    public WirePoint startPoint;
    public WirePoint endPoint;
    private const int PositionCount = 2;
    private LineRenderer _lineRenderer;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void OnDestroy()
    {
        GameObject.Find("WireManager").GetComponent<WireManager>().WireDeleted(this);
        //if(startPoint) startPoint.wireConnections.Remove(this);
        //if(endPoint) endPoint.wireConnections.Remove(this);
    }

    private void Update()
    {
        _lineRenderer.SetPositions(new Vector3[PositionCount]
            {startPoint.gameObject.transform.position, endPoint.gameObject.transform.position});
    }
}
