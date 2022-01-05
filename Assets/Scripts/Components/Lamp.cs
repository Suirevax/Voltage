using UnityEngine;

public class Lamp : MonoBehaviour
{
    private WirePoint _wirePoint;
    private SpriteRenderer _spriteRenderer;
    private readonly Color _offColor = Color.gray;
    private readonly Color _onColor = Color.yellow;

    private bool Powered => _wirePoint.Power;

    private void Awake()
    {
        _wirePoint = GetComponentInChildren<WirePoint>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.color = _offColor;
        
        Wire.OnWireDeleted += OnEvent;
        WirePoint.OnWireAdded += OnEvent;
    }

    private void OnEvent(object sender, object other)
    {
        if (_spriteRenderer)
            _spriteRenderer.color = _wirePoint.Power ? _onColor : _offColor;
    }
}
