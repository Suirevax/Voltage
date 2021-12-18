using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider2D))]
public class Draggable : MonoBehaviour, IDragHandler
{
    private void Awake()
    {
        if(!GetComponent<Collider2D>())
            Debug.LogWarning("Draggable object '" + gameObject.name + "' is missing collider");
    }

    public void OnDrag(PointerEventData eventData)
    {
        var newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        newPos.z = transform.position.z;
        gameObject.transform.position = newPos;
    }
}
