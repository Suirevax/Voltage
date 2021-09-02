using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : MonoBehaviour
{
    //First a simple implementation
    [SerializeField] float requiredVoltage;
    [SerializeField] float requiredAmpere;


    private void Update()
    {
        //if requiredVoltage & requiredAmpere conditions met light is on. else light is off.
    }
}
