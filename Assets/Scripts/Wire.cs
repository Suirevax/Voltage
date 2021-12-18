using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Wire : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] public WirePoint[] wirePoints = new WirePoint[2];
    private LineRenderer _lineRenderer;
    private CapsuleCollider2D _capsuleCollider2D;
    private WireManager _wireManager;

    private bool IsConnected => wirePoints[0] && wirePoints[1];
    public static event EventHandler<Wire> OnWireDeleted;
    public WirePoint StartPoint
    {
        get => wirePoints[0];
        set => wirePoints[0] = value;
    }

    public WirePoint EndPoint
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
        if (!_wireManager) Debug.LogWarning("WireManager missing in scene: OnDestroy");
        OnWireDeleted?.Invoke(this, this);
    }

    private void UpdateLine(object sender, EventArgs e)
    {
        _lineRenderer.SetPositions(new [] {wirePoints[0].transform.position, wirePoints[1].transform.position});
        transform.position = _lineRenderer.GetPosition(0);
        UpdateLineCollider();
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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Destroy(gameObject);
        }
    }

    public void SetEvents()
    {
        StartPoint.OnTransformChanged += UpdateLine;
        EndPoint.OnTransformChanged += UpdateLine;
    }
}