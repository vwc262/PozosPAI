using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.UI;

public class SitiosOrdenados : MonoBehaviour
{
    // Hola Boy
    
    [TabGroup("UI")] public Sprite foldInRegion;
    [TabGroup("UI")] public Sprite foldOutRegion;
    [TabGroup("UI")] public Image ordenGastoImage;
    [TabGroup("UI")] public Image ordenPresionImage;
    [TabGroup("UI")] public Image ordenTotalizadoImage;
    [TabGroup("UI")] public int currentOrderIndex;
    [TabGroup("UI")] public float[] heights;
    [TabGroup("UI")] public float AlphaSelected = 0.7f;
    [TabGroup("UI")] public float AlphaNoSelected = 0.5f;
    [TabGroup("UI")] public Color[] selectedColors;
    
    [ShowInInspector] [TabGroup("Sitios")] public int totalRegiones => ControlDatosAux._singletonExists? ControlDatosAux.singleton.regionales : 0;
    [TabGroup("Sitios")] public SpriteRenderer[] colorRegiones;
    
    [ShowInInspector]
    public Dictionary<int, List<ControlSelectSitio>> dictionaryListSitios = new Dictionary<int, List<ControlSelectSitio>>();
    
    [Button]
    private void Start()
    {
	    Init();
    }

    public virtual void Init() { }
    
    public virtual void clearListas() { }
    
    public virtual void InitListasUI() { }

    public virtual void updateListSitios() { }

    public void SetSelectedColor()
    {
	    for(int i =0; i < dictionaryListSitios.Values.Count; i++)
	    {
		    foreach (var sitio in dictionaryListSitios[i])
		    {
			    sitio.selectedImage.color = selectedColors[i];
		    }
	    }
    }

    [Button][GUIColor(0,0.5f,0.5f)]
    public void SelectRegion(int index, bool val)
    {
	    if (colorRegiones.Length > index)
	    {
		    var color = colorRegiones[index].color;
		    color.a = val ? AlphaSelected : AlphaNoSelected;
		    colorRegiones[index].color = color;
	    }
	    
	    if (dictionaryListSitios.Count > index)
	    {
		    foreach (var sitio in dictionaryListSitios[index])
		    {
			    sitio.SetSelectedInGUI(val);
		    }
	    }
    }
    
    [Button]
    public virtual void ToggleRegion(int index) { }

    [Button]
    public void OrdenGastoPresionTotalizado(int index)
    {
	    currentOrderIndex = currentOrderIndex == index ? 0 : index;
	    var tempColor = new Color(1,1,1,0);
	    ordenGastoImage.color = tempColor;
	    ordenPresionImage.color = tempColor;
	    ordenTotalizadoImage.color = tempColor;

	    tempColor.a = 1;
	    switch (currentOrderIndex)
	    {
		    case 0:// no orden
			    
			    break;
		    case 1:// gasto orden
			    
			    ordenGastoImage.color = tempColor;
			    break;
		    case 2:// presion orden
			    
			    ordenPresionImage.color = tempColor;
			    break;
		    case 3:// totalizado orden
			    
			    ordenTotalizadoImage.color = tempColor;
			    break;
		    default:
			    break;
	    }

	    ReorderSitios(currentOrderIndex);
    }

    [Button]
    public void ReorderSitios(int currentOrderIndex)
    {
	    for(int i =0; i < dictionaryListSitios.Values.Count; i++)
	    {
		    if (currentOrderIndex == 0)
			    dictionaryListSitios[i] = dictionaryListSitios[i].OrderBy(x => x.sitio.MyDataSitio.idSitioUnity).ToList();
		    if (currentOrderIndex == 1)
				dictionaryListSitios[i] = dictionaryListSitios[i].OrderByDescending(x => 
					x.sitio.controlSelectSitio!= null ? x.sitio.GetGastoSitio() : x.sitio.MyDataSitio.idSitioUnity).ToList();
		    if (currentOrderIndex == 2)
			    dictionaryListSitios[i] = dictionaryListSitios[i].OrderByDescending(x => 
				    x.sitio.controlSelectSitio!= null ? x.sitio.GetPresionSitio() : x.sitio.MyDataSitio.idSitioUnity).ToList();
		    if (currentOrderIndex == 3)
			    dictionaryListSitios[i] = dictionaryListSitios[i].OrderByDescending(x =>
				    x.sitio.controlSelectSitio!= null ? x.sitio.GetTotalizadoSitio() : x.sitio.MyDataSitio.idSitioUnity).ToList();

		    foreach (var sitio in dictionaryListSitios[i])
			    sitio.GetComponent<Transform>().SetSiblingIndex(dictionaryListSitios[i].IndexOf(sitio));
	    }
    }
    
    public void ActivateArray(GameObject[] array, bool val)
    {
	    for (int i = 0; i < array.Length; i++)
	    {
		    array[i].SetActive(val);
	    }
    }
}
