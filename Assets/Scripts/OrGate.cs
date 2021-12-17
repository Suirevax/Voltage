using UnityEngine;

public class OrGate : MonoBehaviour
{
    [SerializeField] private WirePoint outputWirePoint;
    [SerializeField] private WirePoint inputWirePoint1;
    [SerializeField] private WirePoint inputWirePoint2;

    private void Awake()
    {
        Wire.OnWireDeleted += OnEvent;
        WirePoint.OnWireAdded += OnEvent;
    }

    private void OnEvent(object sender, object other)
    {
        outputWirePoint.Sourcing = inputWirePoint1.Power || inputWirePoint2.Power;
    }
}
