using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ControlEjeBomba : MonoBehaviour
{
    public WMG_Grid gridEjeBomba;
    public RectTransform rectEjeBomba;
    
    public List<WMG_Series> seriesEjeBomba;
    public List<GameObject> marcasEjeBomba;

    public float pos;
    
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
            Vector2 rect = rectEjeBomba.sizeDelta;
            rect.y = (gridEjeBomba.gridNumNodesY - 1) * gridEjeBomba.gridLinkLengthY + 100;
            rectEjeBomba.sizeDelta = rect;
            
            if (serie.gameObject.activeSelf)
            {
                rectEjeBomba.gameObject.SetActive(true);
                
                pos = (gridEjeBomba.gridNumNodesY - 1) * gridEjeBomba.gridLinkLengthY;
                
                for (int i = 0; i < marcasEjeBomba.Count; i++)
                {
                    marcasEjeBomba[i].transform.localPosition = new Vector3(
                        marcasEjeBomba[i].transform.localPosition.x, 
                        i * (pos / (marcasEjeBomba.Count - 1)), 
                        0);
                }
                
                rectEjeBomba.transform.position = serie.transform.position;
            }
        }
    }
}
