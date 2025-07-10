using System.Collections.Generic;
using UnityEngine;

public class ControlEjeBomba : MonoBehaviour
{
    public WMG_Grid gridEjeBomba;
    public RectTransform rectEjeBomba;
    
    public List<WMG_Series> seriesEjeBomba; 
    
    //public Vector3 DeltaSerieEjeBomba = new Vector3(0,50,0);
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rectEjeBomba.gameObject.SetActive(false);
        
        foreach (var serie in seriesEjeBomba)
        {
            if (serie.gameObject.activeSelf)
            {
                rectEjeBomba.gameObject.SetActive(true);
                Vector2 rect = rectEjeBomba.sizeDelta;
                rect.y = gridEjeBomba.gridNumNodesY * (gridEjeBomba.gridLinkLengthY + 1);
                rectEjeBomba.sizeDelta = rect;
                
                rectEjeBomba.transform.position = serie.transform.position;
            }
        }
    }
}
