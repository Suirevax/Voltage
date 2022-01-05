using UnityEngine;

public class OrGate : MonoBehaviour
{
    private WirePoint _outputWirePoint;
    private WirePoint _inputWirePoint1;
    private WirePoint _inputWirePoint2;

    private void Awake()
    {
        _outputWirePoint = transform.GetChild(0).GetComponent<WirePoint>();
        _inputWirePoint1 = transform.GetChild(1).GetComponent<WirePoint>();
        _inputWirePoint2 = transform.GetChild(2).GetComponent<WirePoint>();
        Wire.OnWireDeleted += OnEvent;
        WirePoint.OnWireAdded += OnEvent;
    }

    private void OnEvent(object sender, object other)
    {
        _outputWirePoint.Sourcing = _inputWirePoint1.Power || _inputWirePoint2.Power;
    }
}
