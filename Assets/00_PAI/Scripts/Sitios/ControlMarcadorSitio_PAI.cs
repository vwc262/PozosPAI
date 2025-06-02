using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Serialization;

public class ControlMarcadorSitio_PAI : ControlMarcadorSitio
{
    [TabGroup("Interfaz")] public List<MeshRenderer> Bombas;
    [TabGroup("Interfaz")] public List<SpriteRenderer> rendererUIStatus = new List<SpriteRenderer>();
    [TabGroup("Interfaz")] public List<GameObject> listFallaAC;
    [TabGroup("Interfaz")] public List<GameObject> listFallaBomba;
    [TabGroup("Interfaz")] public List<GameObject> listFallaConexion;
    
    public override IEnumerator StatusUI()
    {
        while (true)
        {
            if (sitio.dataInTime)
            {
                statusColor = statusColor1;
                SetFalloConexion(false);
                List<SignalBase> bomba = sitio.dataSitio.GetSignal(SignalBase.TipoSignalEnum.BOMBA);

                if (bomba.Count > sitio.indexBomba)
                {
                    switch (bomba[sitio.indexBomba].Valor)
                    {
                        case 0:
                            SetColorMeshBombas(Color.red);
                            SetColorUIEnlace(Color.red);
                            SetFalloBomba(true);
                            break;
                        case 1:
                            SetColorMeshBombas(Color.green);
                            SetColorUIEnlace(Color.green);
                            SetFalloBomba(false);
                            break;
                        case 2:
                            SetColorMeshBombas(new Color(0.9f, 0.9f, 0.9f, 1f));
                            SetColorUIEnlace(new Color(0.7f, 0.7f, 0.7f, 1f));
                            SetFalloBomba(false);
                            break;
                        case 3:
                            //SetColorMeshBombas(Color.blue);
                            SetColorMeshBombas(Color.red);
                            SetColorUIEnlace(Color.red);
                            SetFalloBomba(true);
                            break;
                    }
                }
            }
            else
            {
                statusColor = statusColor3;
                SetFalloConexion(true);
                SetFalloBomba(false);
                SetColorMeshBombas(Color.red);
                SetColorUIEnlace(Color.red);
            }

            foreach (var go in listFallaAC)
            {
                go.SetActive(sitio.dataSitio.fallaAC);
            }

            yield return new WaitForSeconds(updateRate);
        }
    }
    
    public void SetFalloConexion(bool fallo)
    {
        foreach (var falloConexion in listFallaConexion)
        {
            falloConexion.gameObject.SetActive(fallo);
        }
    }

    public void SetFalloBomba(bool fallo)
    {
        foreach (var falloBomba in listFallaBomba)
        {
            falloBomba.gameObject.SetActive(fallo);
        }
    }

    public void SetColorUIEnlace(Color _statusColor)
    {
        rendererUIStatus.ForEach(item =>
        {
            item.color = _statusColor;
            item.material.SetColor("_BaseColor", _statusColor);
            item.material.SetColor("_EmissiveColorLDR", _statusColor);
            HDMaterial.ValidateMaterial(item.material);
        });
    }
    
    public override void SetDataSitio(ControlSitio _Sitio)
    {
        sitio = _Sitio;

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
            switch ((EstructurasAPI.Proyectos)sitio.dataSitio.Estructura)
            {
                case EstructurasAPI.Proyectos.Teoloyucan:
                    ValidaSelectSitio(ControlAccesoPozosPAI.Proyectos.Teoloyucan);
                    break;
                
                case EstructurasAPI.Proyectos.PozosZumpango:
                    ValidaSelectSitio(ControlAccesoPozosPAI.Proyectos.PozosZumpango);
                    break;
                
                case EstructurasAPI.Proyectos.PozosAIFA:
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
