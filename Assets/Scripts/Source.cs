using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Source : MonoBehaviour
{
    private void Start()
    {
        GetComponentInChildren<WirePoint>().Sourcing = true;
    }
}
