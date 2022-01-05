using UnityEngine;

public class Source : MonoBehaviour
{
    private void Start()
    {
        GetComponentInChildren<WirePoint>().Sourcing = true;
    }
}
