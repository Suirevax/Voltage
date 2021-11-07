using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Source : MonoBehaviour
{
    void Start()
    {
        GetComponentInChildren<wirepoint>().Sourcing = true;
    }
}
