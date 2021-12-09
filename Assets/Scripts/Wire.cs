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
    private CapsuleCollider2D _capsuleCollider2D;

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
        _capsuleCollider2D = GetComponent<CapsuleCollider2D>();
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
        //TODO: Not update itself every time!! Event stuff!! The wires barely move anyway
        if (IsConnected)
        {
            _lineRenderer.SetPositions(new Vector3[PositionCount]
                {startPoint.gameObject.transform.position, endPoint.gameObject.transform.position});
            gameObject.transform.position = _lineRenderer.GetPosition(0);
            UpdateLineCollider();
        }
        
        //TODO: Don't have every wire cast a ray itself. Really should look into the event system would probably be way cleaner
        if (Input.GetMouseButtonDown(1))
        {
            var hit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition), Mathf.Infinity);
            if (hit.collider)
            {
                if (hit.collider.gameObject == gameObject)
                {
                    Destroy(gameObject);
                }
            }
        }
        
    }
    

    private void UpdateLineCollider()
    {
        var pos0 = _lineRenderer.GetPosition(0);
        var pos1 = _lineRenderer.GetPosition(1);
        var distance = Vector2.Distance(pos0, pos1);
        
        _capsuleCollider2D.offset = new Vector2(0, distance / 2);
        _capsuleCollider2D.size = new Vector2(_capsuleCollider2D.size.x, distance);
        transform.rotation = Quaternion.Euler(0, 0,
            Vector2.SignedAngle(Vector2.up, pos1 - pos0));
    }
}
