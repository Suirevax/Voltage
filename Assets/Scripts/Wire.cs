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

    private WireManager WireManager
    {
        get
        {
            var wireManager = GameObject.Find("WireManager").GetComponent<WireManager>();
            if(!wireManager) Debug.LogWarning("WireManager missing in scene");
            return wireManager;
        } 
    } 

    private bool IsConnected => startPoint && endPoint;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void OnDestroy()
    {
        //TODO: should probably be implemented as an event..
        if (WireManager)
        {
            GameObject.Find("WireManager").GetComponent<WireManager>().WireDeleted(this);
        }
    }

    private void Update()
    {
        if (IsConnected)
        {
            _lineRenderer.SetPositions(new Vector3[PositionCount]
                {startPoint.gameObject.transform.position, endPoint.gameObject.transform.position});
        }
    }
}
