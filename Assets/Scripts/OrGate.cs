using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrGate : MonoBehaviour
{
    [SerializeField] private WirePoint outputWirePoint;
    [SerializeField] private WirePoint inputWirePoint1;
    [SerializeField] private WirePoint inputWirePoint2;
    

    private void Update()
    {
        outputWirePoint.Sourcing = inputWirePoint1.Power || inputWirePoint2.Power;
    }
}
