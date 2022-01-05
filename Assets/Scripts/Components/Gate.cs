using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] private WirePoint wirePointPrefab;
    
    
    public enum GateType {
        Or,
        And,
        Not,
        Nor,
        Nand,
        Xor,
        XNor
    };

    enum InputOrOutput
    {
        Input,
        Output
    }

    public GateType _gateType;

    [SerializeField] public GateType _GateType
    {
        get => _gateType;
        set
        {
            _gateType = value;
            SetGateType(value);
        } 
    }

    // Start is called before the first frame update
    void Update()
    {
        SetGateType(_gateType);
    }

    void SetGateType(GateType gateType)
    {
        foreach (var componentInChild in GetComponentsInChildren<WirePoint>())
        {
            Destroy(componentInChild.gameObject);
        }
        CreateWirePoints(gateType);
    }

    private void CreateWirePoints(GateType gateType)
    {
        // switch (gateType)
        // {
        //     case GateType.Not:
        //         //Create 1 input & 1 output point
        //         CreatePoints(1, 1);
        //         //Instantiate(wirePointPrefab, transform, new Vector3())
        //         break;
        //     default:
        //         //create 2 input & 1 output points
        //         break;
        // }
        CreatePoints(1, 1);

    }

    private void CreatePoints(int InputPoints, int OutputPoints)
    {
        var bounds = GetComponent<BoxCollider2D>().bounds;
        var width = bounds.size.x;
        var startPos = new Vector3(-0.5f, -0.5f, 0);
        var inputInterval = width / (1 + InputPoints);
        for (var i = 1; i <= InputPoints; i++)
        {
            var newWirePoint = Instantiate(wirePointPrefab, transform);
            //startPos.x += inputInterval * i;
            newWirePoint.transform.Translate(startPos, Space.Self);
        }
    }
}
