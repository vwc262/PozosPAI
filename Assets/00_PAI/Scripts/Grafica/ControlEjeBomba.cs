using UnityEngine;

public class ControlEjeBomba : MonoBehaviour
{
    public WMG_Grid gridEjeBomba;
    public RectTransform rectEjeBomba;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 rect = rectEjeBomba.sizeDelta;
        rect.y = gridEjeBomba.gridNumNodesY * (gridEjeBomba.gridLinkLengthY + 1);
        rectEjeBomba.sizeDelta = rect;
    }
}
