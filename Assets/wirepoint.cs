using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class wirepoint : MonoBehaviour
{
    [SerializeField] player player;
    [SerializeField] GameObject wirePrefab = null;

    LineRenderer newWire = null;

    private void OnMouseOver()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
    }

    private void OnMouseExit()
    {
        GetComponent<SpriteRenderer>().color = Color.black;
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
            GetComponent<SpriteRenderer>().color = Color.blue;
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

        //Wil ik checken of het hetzelfde component is? opzich zou die wel moeten kunnen.
        //Al wil ik in de toekomst wss wel dat kabels niet onder/ door component heen kunnen lopen
    }
}
