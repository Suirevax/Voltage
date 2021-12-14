using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Net;
using UnityEngine;
using UnityEngine.EventSystems;

public class WirePoint : MonoBehaviour
{
    [SerializeField] private GameObject wirePrefab = null;

    //public List<Wire> wireConnections = new List<Wire>();
    private LineRenderer _newWire = null;
    
    //Electric Values
    [SerializeField] private bool power;
    [SerializeField] private bool sourcing;
    public bool Power
    {
        get => power;
        set
        {
            GetComponent<SpriteRenderer>().color = value || sourcing ? Color.green : Color.red;
            power = value;
        }
    }

    public bool Sourcing
    {
        get => sourcing;
        set
        {
            GetComponent<SpriteRenderer>().color = value || power ? Color.green : Color.red;
            sourcing = value;
        }
    }

    private void Awake()
    {
        Power = false;
        sourcing = false;
    }

    private void OnMouseUp()
    {
        if (_newWire)
        {
            var endWirePoint = GetHoveringWirePoint();
            if (endWirePoint)
            {
                if (IsConnectionValid(endWirePoint.GetComponent<WirePoint>()))
                {
                    _newWire.SetPosition(1, endWirePoint.transform.position);
                    var wire = _newWire.GetComponent<Wire>();
                    wire.startPoint = this;
                    wire.endPoint = endWirePoint.GetComponent<WirePoint>();
                    GameObject.Find("WireManager").GetComponent<WireManager>().CreatedWire(wire);
                    //wireConnections.Add(wire);
                    //wire.endPoint.wireConnections.Add(wire);
                    _newWire = null;
                    return;
                }
            }

            Destroy(_newWire.gameObject);
        }
    }



    private void OnMouseDrag()
    {
        if (!_newWire)
        {
            //GetComponent<SpriteRenderer>().color = Color.blue;
            _newWire = Instantiate(wirePrefab, GameObject.Find("WireManager").transform).GetComponent<LineRenderer>();
            var position = transform.position;
            _newWire.SetPosition(0, position);
            _newWire.SetPosition(1, position);
        }
        else
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = transform.position.z;
            _newWire.SetPosition(1, mousePos);
        }
    }

    private GameObject GetHoveringWirePoint()
    {

        RaycastHit2D hit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition), Mathf.Infinity);
        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("WirePoint"))
            {
                return hit.collider.gameObject;
            }
        }
        return null;
    }

    private bool IsConnectionValid(WirePoint endWirePoint)
    {
        //Check if connected to itself
        if (endWirePoint == this) return false;
        
        //Check if connection already exists
        var wiresObject = GameObject.Find("WireManager");
        var wireList = wiresObject.GetComponentsInChildren<Wire>();

        foreach(var wire in wireList)
        {
            if((this == wire.startPoint || this == wire.endPoint) &&
              (endWirePoint == wire.startPoint || endWirePoint == wire.endPoint)){
                return false;
            }
        }
        return true;
        
        //TODO:
        //Wil ik checken of het hetzelfde component is? opzich zou dat mogen.
        //Al wil ik in de toekomst wss wel dat kabels niet onder/ door component heen kunnen lopen.
        //Dus ook nog losse wirepoints implementeren
    }
}
