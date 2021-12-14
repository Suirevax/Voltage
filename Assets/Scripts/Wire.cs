using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wire : MonoBehaviour
{
    //TODO: put startpoint & endpoint in single list
    //public WirePoint startPoint;
    //public WirePoint endPoint;
    [SerializeField] public WirePoint[] wirePoints = new WirePoint[2];
    //private const int PositionCount = 2;
    private LineRenderer _lineRenderer;
    private CapsuleCollider2D _capsuleCollider2D;
    private WireManager _wireManager;

    private bool IsConnected => wirePoints[0] && wirePoints[1];
    
    public WirePoint startPoint
    {
        get => wirePoints[0];
        set => wirePoints[0] = value;
    }

    public WirePoint endPoint
    {
        get => wirePoints[1];
        set => wirePoints[1] = value;
    }

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        _wireManager = GameObject.Find("WireManager").GetComponent<WireManager>();
        if(!_wireManager) Debug.LogWarning("WireManager missing in scene: Awake");
        
    }

    private void OnDestroy()
    {
        if(!_wireManager) Debug.LogWarning("WireManager missing in scene: OnDestroy");
        _wireManager.WireDeleted(this);
    }

    private void Update()
    {
        //TODO: Not update itself every time!! Event stuff!! The wires barely move anyway
        if (IsConnected)
        {
            _lineRenderer.SetPositions(new [] {wirePoints[0].transform.position, wirePoints[1].transform.position});
            transform.position = _lineRenderer.GetPosition(0);
            UpdateLineCollider();
        }
        
        //TODO: Don't have every wire cast a ray itself. Make central user input manager which does that
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
