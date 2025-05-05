using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class SitioGPS_PAI : SitioGPS
{
    public List<GameObject> listFallaAC_GO;
    public List<GameObject> listFallaBomba;
    
    public override IEnumerator StatusUI()
    {
        if (rendererUIStatus.Count > 0)
        {
            while (true)
            {
                DateTime parsedDate;

                if (DateTime.TryParse(MyDataSitio.fecha, out parsedDate))
                {
                    diferencia = (float)(DateTime.Now - parsedDate).TotalMinutes;

                    if (diferencia < umbralGreen)
                    {
                        dataInTime = true;
                        statusColor = statusColor1;
                        statusDataInTime = 1;
                    }
                    // else if (diferencia < umbralYellow)
                    // {
                    //     dataInTime = false;
                    //     statusColor = statusColor2;
                    //     statusDataInTime = 2;
                    // }
                    else
                    {
                        dataInTime = false;
                        statusColor = statusColor3;
                        statusDataInTime = 3;
                    }

                    if (MyDataSitio.bomba.Count > 0)
                    {
                        // if (MyDataSitio.bomba[indexBomba].DentroRango)
                        // {
                            switch (MyDataSitio.bomba[indexBomba].Valor)
                            {
                                case 0:
                                    SetColorMeshBombas(new Color(0.9f,0.9f,0.9f,1f));
                                    SetColorBombaMap2D(0);
                                    foreach (var falloBomba in listFallaBomba)
                                    {
                                        falloBomba.gameObject.SetActive(true);
                                    }
                                    break;
                                case 1:
                                    SetColorMeshBombas(Color.green);
                                    SetColorBombaMap2D(1);
                                    foreach (var falloBomba in listFallaBomba)
                                    {
                                        falloBomba.gameObject.SetActive(false);
                                    }
                                    break;
                                case 2:
                                    SetColorMeshBombas(Color.red);
                                    SetColorBombaMap2D(2);
                                    foreach (var falloBomba in listFallaBomba)
                                    {
                                        falloBomba.gameObject.SetActive(false);
                                    }
                                    break;
                                case 3:
                                    SetColorMeshBombas(Color.blue);
                                    SetColorBombaMap2D(3);
                                    foreach (var falloBomba in listFallaBomba)
                                    {
                                        falloBomba.gameObject.SetActive(false);
                                    }
                                    break;
                            }
                        // }
                        // else
                        // {
                        //     SetColorMeshBombas(Color.gray);
                        //     SetColorBombaMap2D(0);
                        // }
                    }
                    else
                    {
                        SetColorMeshBombas(new Color(0.9f,0.9f,0.9f,1f));
                        SetColorBombaMap2D(0);
                    }
                    
                    rendererUIStatus.ForEach(item =>
                    {
                        item.color = statusColor;
                        item.material.SetColor("_BaseColor", statusColor);
                        item.material.SetColor("_EmissiveColorLDR", statusColor);
                        HDMaterial.ValidateMaterial(item.material);

                    });
                    
                    //textUIStatus.ForEach(item => item.color = statusColor);
                }
                else
                {
                    Debug.Log("Invalid date format");
                }

                foreach (var go in listFallaAC_GO)
                {
                    go.SetActive(MyDataSitio.fallaAC);
                }

                yield return new WaitForSeconds(updateRate);
            }
        }
    }
    
    public override void SetDataSitio(DataSitio _DataSitio)
    {
        MyDataSitio.SetDataSitio(_DataSitio);
        
        textoNombre.text = MyDataSitio.nombre;
        textoFecha.text = MyDataSitio.fecha;
        textoIdSitioUnity.text = $"{GetIDSitiosPAI(MyDataSitio.abreviacion)}";
        textoAlias.text = $"{MyDataSitio.abreviacion}";
        
        foreach (var mesh in Bombas)
        {
            mesh.gameObject.SetActive(true);
        }
    }

    public static string GetIDSitiosPAI(string _abreviacion)
    {
        string id = _abreviacion.ToUpper();
        
        return id.Replace("AIFA", "A");
    }
    
    public void SetColorMeshBombas(Color _color)
    {
        foreach (var mesh in Bombas)
        {
            if (mesh != null)
            {
                var renderer = mesh.GetComponent<Renderer>();

                if (renderer != null)
                    renderer.material.color = _color;
            }
        }
    }

    public override void SetSelectedSitio()
    {
        if (ControlAccesoPozosPAI.singleton.isInteractableAllUISitios)
            base.SetSelectedSitio();
        else
            SetSelectedSitioValida();
    }
    
    public void SetSelectedSitioValida()
    {
        if (MyDataSitio != null)
        {
            switch ((RequestAPI.Proyectos)MyDataSitio.Estructura)
            {
                case RequestAPI.Proyectos.Teoloyucan:
                    ValidaSelectSitio(ControlAccesoPozosPAI.Proyectos.Teoloyucan);
                    break;
                
                case RequestAPI.Proyectos.PozosZumpango:
                    ValidaSelectSitio(ControlAccesoPozosPAI.Proyectos.PozosZumpango);
                    break;
                
                case RequestAPI.Proyectos.PozosReyesFerrocarril:
                    ValidaSelectSitio(ControlAccesoPozosPAI.Proyectos.PozosReyesFerrocarril);
                    break;
                
                case RequestAPI.Proyectos.PozosAIFA:
                    ValidaSelectSitio(ControlAccesoPozosPAI.Proyectos.PozosAIFA);
                    break;
            }
        }
    }

    public void ValidaSelectSitio(ControlAccesoPozosPAI.Proyectos proyecto)
    {
        if (ControlAccesoPozosPAI.singleton.proyectos.HasFlag(proyecto))
        {
            Debug.Log("Selected Sitio");
            base.SetSelectedSitio();
        }
        else
        {
            Debug.Log("No Selectable Sitio: " + proyecto);
        }
    }
}
