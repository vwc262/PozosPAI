using UnityEngine;

public class Line_Controller : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    
    [SerializeField] Transform[] _lineTransform;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        _lineRenderer.positionCount = _lineTransform.Length;
        for (int i = 0; i < _lineTransform.Length; i++)
        {
            _lineRenderer.SetPosition(i, _lineTransform[i].position);
        }
        
    }
}
