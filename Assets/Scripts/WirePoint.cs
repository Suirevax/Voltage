using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class WirePoint : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] private GameObject wirePrefab = null;

    //public List<Wire> wireConnections = new List<Wire>();
    private LineRenderer _newWire = null;
    
    //Electric Values
    [SerializeField] private bool power;
    [SerializeField] private bool sourcing;

    private SpriteRenderer _spriteRenderer;
    private WireManager _wireManager;
    
    public static event EventHandler<Wire> OnWireAdded;
    public static event EventHandler OnSourcingChanged;
    
    public bool Power
    {
        get => power;
        set
        {
            _spriteRenderer.color = value || sourcing ? Color.green : Color.red;
            power = value;
        }
    }

    public bool Sourcing
    {
        get => sourcing;
        set
        {
            _spriteRenderer.color = value || power ? Color.green : Color.red;
            sourcing = value;
            OnSourcingChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _wireManager = GameObject.Find("WireManager").GetComponent<WireManager>();
        Power = false;
        sourcing = false;
    }

    private GameObject GetHoveringWirePoint()
    {
        Debug.Log("GetHoveringPoint");
        RaycastHit2D hit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition), Mathf.Infinity);
        if (hit.collider != null)
        {
            Debug.Log(hit.collider.name);
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

        return wireList.All(wire => !wire.wirePoints.Contains(this) || !wire.wirePoints.Contains(endWirePoint));

        //TODO:
        //Wil ik checken of het hetzelfde component is? opzich zou dat mogen.
        //Al wil ik in de toekomst wss wel dat kabels niet onder/ door component heen kunnen lopen.
        //Dus ook nog losse wirepoints implementeren
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        //Needs to be here to make OnPointerUpFunction
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("WireWhat");
        if (_newWire)
        {
            var endWirePoint = GetHoveringWirePoint();
            if (endWirePoint)
            {
                if (IsConnectionValid(endWirePoint.GetComponent<WirePoint>()))
                {
                    _newWire.SetPosition(1, endWirePoint.transform.position);
                    var wire = _newWire.GetComponent<Wire>();
                    wire.StartPoint = this;
                    wire.EndPoint = endWirePoint.GetComponent<WirePoint>();
                    OnWireAdded?.Invoke(this, wire);
                    //_wireManager.CreatedWire(wire);
                    //wireConnections.Add(wire);
                    //wire.endPoint.wireConnections.Add(wire);
                    _newWire = null;
                    return;
                }
            }
    
            Destroy(_newWire.gameObject);
        }
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        if (!_newWire)
        {
            //GetComponent<SpriteRenderer>().color = Color.blue;
            _newWire = Instantiate(wirePrefab, _wireManager.transform).GetComponent<LineRenderer>();
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
}
