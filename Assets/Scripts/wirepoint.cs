using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.EventSystems;

public class wirepoint : MonoBehaviour
{
    [SerializeField] GameObject wirePrefab = null;

    public List<Wire> wireConnections = new List<Wire>();
    LineRenderer newWire = null;
    
    //Electric Values
    [SerializeField] private bool power;
    [SerializeField] public bool sourcing;
    public bool Power
    {
        get => power;
        set
        {
            GetComponent<SpriteRenderer>().color = value ? Color.green : Color.red;
            power = value;
        }
    }

    private void Awake()
    {
        Power = false;
        sourcing = false;
    }

    private void Update()
    {
        Power = sourcing || IsConnectionPowered();
    }

    private bool IsConnectionPowered()
    {
        foreach (var connection in wireConnections)
        {
            if (connection.endPoint != this && connection.startPoint != this)
            {
                Debug.LogWarning("Wirepoint not connected to wire: " + connection.gameObject.name);
            }
 
            if (connection.startPoint == this)
            {
                if (connection.endPoint.Power)
                {
                    return true;
                }
            }
            else //connection.endPoint == this
            {
                if (connection.startPoint.Power)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void OnMouseOver()
    {
        // GetComponent<SpriteRenderer>().color = Color.red;
    }

    private void OnMouseExit()
    {
        // GetComponent<SpriteRenderer>().color = Color.black;
    }

    private void OnMouseUp()
    {
        if (newWire)
        {
            var endWirepoint = GetHoveringWirePoint();
            if (endWirepoint)
            {
                if (IsConnectionValid(endWirepoint.GetComponent<wirepoint>()))
                {
                    newWire.SetPosition(1, endWirepoint.transform.position);
                    Wire wire = newWire.GetComponent<Wire>();
                    wire.startPoint = this;
                    wire.endPoint = endWirepoint.GetComponent<wirepoint>();
                    wireConnections.Add(wire);
                    wire.endPoint.wireConnections.Add(wire);
                    newWire = null;
                    return;
                }
            }

            Destroy(newWire.gameObject);
        }
    }



    private void OnMouseDrag()
    {
        if (!newWire)
        {
            //GetComponent<SpriteRenderer>().color = Color.blue;
            newWire = Instantiate(wirePrefab, GameObject.Find("Wires").transform).GetComponent<LineRenderer>();
            newWire.SetPosition(0, transform.position);
            newWire.SetPosition(1, transform.position);
        }
        else
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = transform.position.z;
            newWire.SetPosition(1, mousePos);
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

    bool IsConnectionValid(wirepoint endWirePoint)
    {
        //Check if connection already exists
        var wiresObject = GameObject.Find("Wires");
        var wireList = wiresObject.GetComponentsInChildren<Wire>();

        foreach(Wire wire in wireList)
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
