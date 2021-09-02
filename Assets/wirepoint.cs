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
                //TODO: Check if endWirepoint is valid
                if (IsConnectionValid())
                {

                }
                newWire.SetPosition(1, endWirepoint.transform.position);
                newWire = null;
            }
            else
            {
                Destroy(newWire.gameObject);
            }
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

    bool IsConnectionValid()
    {
        return true;
        /* wat moet ik hiervoor weten?
         * - Bestaat deze connectie al? 
         * - Wilt het connecten aan zichzelf? hetzelfde component?
         * - ??? 
         * 
         * Hoe bereik ik deze dingen? 
         * 
         */


    }
}
