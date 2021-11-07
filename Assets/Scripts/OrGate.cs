using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrGate : MonoBehaviour
{
    [SerializeField] private wirepoint outputWirepoint;
    [SerializeField] private wirepoint inputWirepoint1;
    [SerializeField] private wirepoint inputWirepoint2;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        outputWirepoint.Sourcing = inputWirepoint1.Power || inputWirepoint2.Power;
        //outputWirepoint.Power = inputWirepoint1.Power || inputWirepoint2.Power;
    }
}
