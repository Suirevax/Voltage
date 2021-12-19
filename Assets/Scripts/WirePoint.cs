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
    public event EventHandler OnTransformChanged; 
    
    public bool Power
    {
        get => power;
        set
        {
            power = value;
            if(_spriteRenderer)
                _spriteRenderer.color = value || sourcing ? Color.green : Color.red;
        }
    }

    public bool Sourcing
    {
        get => sourcing;
        set
        {
            sourcing = value;
            if(_spriteRenderer)
                _spriteRenderer.color = value || power ? Color.green : Color.red;
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

    private void Update()
    {
        if (!transform.hasChanged) return;
        transform.hasChanged = false;
        OnTransformChanged?.Invoke(this, EventArgs.Empty);
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

        return wireList.All(wire => !wire.wirePoints.Contains(this) || !wire.wirePoints.Contains(endWirePoint));

        //TODO:
        //Wil ik checken of het hetzelfde component is? opzich zou dat mogen.
        //Al wil ik in de toekomst wss wel dat kabels niet onder/ door component heen kunnen lopen.
        //Dus ook nog losse wirepoints implementeren
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        //Needs to be here to make OnPointerUp function
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        if (!_newWire) return;
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
                wire.SetEvents();
                wire.UpdateLine();
                _newWire = null;
                return;
            }
        }
    
        Destroy(_newWire.gameObject);
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        if (!_newWire)
        {
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

    private void OnDestroy()
    {
        _wireManager.FindWiresConnectedToPoint(this).ForEach(wire => Destroy(wire.gameObject));
    }
}
