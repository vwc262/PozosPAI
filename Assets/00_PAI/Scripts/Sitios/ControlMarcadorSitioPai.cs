using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class ControlMarcadorSitioPai : ControlMarcadorSitio
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

                if (DateTime.TryParse(sitio.dataSitio.fecha, out parsedDate))
                {
                    diferencia = (float)(DateTime.Now - parsedDate).TotalMinutes;

                    if (diferencia < umbralGreen)
                    {
                        dataInTime = true;
                        statusColor = statusColor1;
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
                    }

                    if (sitio.dataSitio.bomba.Count > 0)
                    {
                        // if (MyDataSitio.bomba[indexBomba].DentroRango)
                        // {
                            switch (sitio.dataSitio.bomba[sitio.indexBomba].Valor)
                            {
                                case 0:
                                    SetColorMeshBombas(new Color(0.9f,0.9f,0.9f,1f));
                                    foreach (var falloBomba in listFallaBomba)
                                    {
                                        falloBomba.gameObject.SetActive(true);
                                    }
                                    break;
                                case 1:
                                    SetColorMeshBombas(Color.green);
                                    foreach (var falloBomba in listFallaBomba)
                                    {
                                        falloBomba.gameObject.SetActive(false);
                                    }
                                    break;
                                case 2:
                                    SetColorMeshBombas(Color.red);
                                    foreach (var falloBomba in listFallaBomba)
                                    {
                                        falloBomba.gameObject.SetActive(false);
                                    }
                                    break;
                                case 3:
                                    SetColorMeshBombas(Color.blue);
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
                        // }
                    }
                    else
                    {
                        SetColorMeshBombas(new Color(0.9f,0.9f,0.9f,1f));
                    }
                    
                    rendererUIStatus.ForEach(item =>
                    {
                        item.color = statusColor;
                        item.material.SetColor("_BaseColor", statusColor);
                        item.material.SetColor("_EmissiveColorLDR", statusColor);
                        HDMaterial.ValidateMaterial(item.material);

                    });
                }
                else
                {
                    Debug.Log("Invalid date format");
                }

                foreach (var go in listFallaAC_GO)
                {
                    go.SetActive(sitio.dataSitio.fallaAC);
                }

                yield return new WaitForSeconds(updateRate);
            }
        }
    }
    
    public override void SetDataSitio(ControlSitio _Sitio)
    {
        sitio = _Sitio;
        
        textoNombre.text = sitio.dataSitio.nombre;
        textoFecha.text = sitio.dataSitio.fecha;
        textoIdSitioUnity.text = $"{GetIDSitiosPAI(sitio.dataSitio.abreviacion)}";
        textoAlias.text = $"{sitio.dataSitio.abreviacion}";
        
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
                {
                    renderer.material.color = _color;
                    
                }
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
        if (sitio.dataSitio != null)
        {
            switch ((RequestAPI.Proyectos)sitio.dataSitio.Estructura)
            {
                case RequestAPI.Proyectos.Teoloyucan:
                    ValidaSelectSitio(ControlAccesoPozosPAI.Proyectos.Teoloyucan);
                    break;
                
                case RequestAPI.Proyectos.PozosZumpango:
                    ValidaSelectSitio(ControlAccesoPozosPAI.Proyectos.PozosZumpango);
                    break;
                
                // case RequestAPI.Proyectos.PozosReyesFerrocarril:
                //     ValidaSelectSitio(ControlAccesoPozosPAI.Proyectos.PozosReyesFerrocarril);
                //     break;
                
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
