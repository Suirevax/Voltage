using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.TerrainAPI;
using UnityEngine.UI;
using UnityEditor;

[ExecuteInEditMode]
public class ComponentButton : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private GameObject componentPrefab;
    private GameObject draggingObject = null;
    private int zPos = 0;

    //TODO: Figure out how to do stuff like this..
    //Would take too much time right now..
    #if UNITY_EDITOR
    public GameObject ComponentPrefab
    {
        get => componentPrefab;
        set
        {
            componentPrefab = value;
            Debug.Log(value.gameObject.GetComponent<SpriteRenderer>().color);
        } 
    }
    #endif

    private void Start()
    {
        var tmp = componentPrefab.GetComponent<SpriteRenderer>();
        GetComponentInChildren<Image>().sprite = tmp.sprite;
        GetComponentInChildren<Image>().color = tmp.color;

        var text = GetComponentInChildren<TMP_Text>();
        text.text = componentPrefab.name;
    }


    public void OnDrag(PointerEventData eventData)
    {
        Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        position.z = zPos;
        draggingObject.transform.position = position;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        position.z = zPos;
        draggingObject = Instantiate(componentPrefab, position, quaternion.identity);
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        draggingObject = null;
    }
}
