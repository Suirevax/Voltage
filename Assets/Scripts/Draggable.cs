using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    private void Awake()
    {
        if(!GetComponent<BoxCollider2D>())
            Debug.LogWarning("Draggable object '" + gameObject.name + "' is missing collider");
    }

    private void OnMouseDrag()
    {
        var newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        newPos.z = 0;
        gameObject.transform.position = newPos;
    }
}
