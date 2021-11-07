using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : MonoBehaviour
{
    [SerializeField] private bool powered;

    private bool Powered
    {
        get => powered;
        set
        {
            GetComponent<SpriteRenderer>().color = value ? Color.yellow : Color.gray;
            powered = value;
        }
    }

    private void Awake()
    {
        powered = false;
    }

    private void Update()
    {
        Powered = GetComponentInChildren<WirePoint>().Power;
    }
}
