using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inverter : MonoBehaviour
{
    private WirePoint _outputWirePoint;
    private WirePoint _inputWirePoint;

    private void Awake()
    {
        _outputWirePoint = transform.GetChild(0).GetComponent<WirePoint>();
        _inputWirePoint = transform.GetChild(1).GetComponent<WirePoint>();
        Wire.OnWireDeleted += OnEvent;
        WirePoint.OnWireAdded += OnEvent;
    }

    private void OnEvent(object sender, object other)
    {
        _outputWirePoint.Sourcing = !_inputWirePoint.Power;
    }
}
