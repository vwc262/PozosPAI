using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HutongGames.PlayMaker.Actions;
using Sirenix.OdinInspector;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Serialization;

public class ControlMarcadorSitio_Generic : ControlMarcadorSitio
{
    [TabGroup("Interfaz")] public List<MeshRenderer> rendererUIStatus = new List<MeshRenderer>();
    [TabGroup("Interfaz")] public List<GameObject> listFallaAC_GO;
    [TabGroup("Interfaz")] public List<GameObject> listFallaBomba;
    
    public Vector3 despPosicionSelection;
    
    public override IEnumerator StatusUI()
    {
        if (rendererUIStatus.Count > 0)
        {
            while (true)
            {
                if (sitio.dataInTime)
                {
                    statusColor = statusColor1;
                }
                else
                {
                    statusColor = statusColor3;
                }
                    
                List<SignalBase> bomba = sitio.dataSitio.GetSignal(SignalBase.TipoSignalEnum.BOMBA);

                if (bomba.Count > 0)
                {
                    switch (bomba[sitio.indexBomba].Valor)
                    {
                        case 0:
                            foreach (var falloBomba in listFallaBomba)
                            {
                                falloBomba.gameObject.SetActive(true);
                            }
                            break;
                        case 1:
                        case 2:
                        case 3:
                            foreach (var falloBomba in listFallaBomba)
                            {
                                falloBomba.gameObject.SetActive(false);
                            }
                            break;
                    }
                }
                
                rendererUIStatus.ForEach(item =>
                {
                    item.material.color = statusColor;
                    item.material.SetColor("_BaseColor", statusColor);
                    item.material.SetColor("_EmissiveColorLDR", statusColor);
                    HDMaterial.ValidateMaterial(item.material);
                });
                
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

        textoIdSitioUnity.text = $"{GetIDSitiosPAI(sitio.dataSitio.abreviacion)}";
        textoAlias.text = $"{sitio.dataSitio.abreviacion}";
    }

    public static string GetIDSitiosPAI(string _abreviacion)
    {
        string id = _abreviacion.ToUpper();
        
        return id.Replace("AIFA", "A");
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

    public GameObject prefabSignal;
    public GameObject contentSignal;
    public List<GameObject> signals;

    public float ap;
    public float radio;

    public int ContSignals;

    [Button]
    public void SpawnSignals()
    {
        ap = radio / 1.15f;
        ContSignals = 0;
        
        foreach (var signal in sitio.dataSitio.listSignals.Where(x=> 
                     x.tipoSignal != SignalBase.TipoSignalEnum.TIEMPO &&
                     x.tipoSignal != SignalBase.TipoSignalEnum.FALLAC &&
                     x.tipoSignal != SignalBase.TipoSignalEnum.PERILLA_BOMBA ))
        {
            int index = 0;
            
            foreach (var signalBase in signal.signals)
            {
                GameObject signalInstance = Instantiate(prefabSignal, contentSignal.transform);
                signalInstance.transform.localPosition = GetHexagonalPosition(ContSignals);
                signalInstance.transform.localRotation = Quaternion.identity;
                signalInstance.transform.localScale = Vector3.one;
                signalInstance.name = $"Signal_{ContSignals}";
                
                ControlMarcadorSignal signalController = signalInstance.GetComponent<ControlMarcadorSignal>();
                if (signalController != null)
                    signalController.SetSignal(signal, index);
                
                signals.Add(signalInstance);
                ContSignals++;
                index++;
            }
        }
    }
    
    public void PruebaSpawnSignals()
    {
        ap = radio / 1.15f;
        
        for (int i = 0; i < 36; i++)
        {
            GameObject signal = Instantiate(prefabSignal, contentSignal.transform);
            signal.transform.localPosition = GetHexagonalPosition(i);
            signal.transform.localRotation = Quaternion.identity;
            signal.transform.localScale = Vector3.one;

            signals.Add(signal);
        }
    }

    public Vector3 GetHexagonalPosition(int index)
    {
        Vector3 PositionSignal;
        
        if (index < 6)
        {
            Vector3 posSignal = new Vector3(2 * ap, 0, 0);
            PositionSignal = Quaternion.Euler(0, 30 + (60 * index), 0) * posSignal;
        }
        else if (index < 18) 
        {
            Vector3 posSignal;
                
            if ((index-6) % 2 == 0)
                posSignal = new Vector3(radio * 3, 0, 0);
            else
                posSignal = new Vector3(ap * 4, 0, 0);
                
            PositionSignal = Quaternion.Euler(0, (30 * (index-6)), 0) * posSignal;
        }
        else if (index < 24) 
        {
            Vector3 posSignal = new Vector3(ap * 6, 0, 0);
                
            PositionSignal = Quaternion.Euler(0, 30 + (60 * (index-18)), 0) * posSignal;
        }
        else if (index < 30)
        {
            Vector3 posSignal = new Vector3(radio * 3, 0, 0);
                
            PositionSignal = Quaternion.Euler(0, 60 * (index - 24), 0) * (posSignal + (Quaternion.Euler(0, 30, 0) * new Vector3(2 * ap, 0, 0)));
        }
        else
        {
            Vector3 posSignal = new Vector3(radio * 3, 0, 0);
                
            PositionSignal = Quaternion.Euler(0, 60 * (index - 24), 0) * (posSignal + (Quaternion.Euler(0, -30, 0) * new Vector3(2 * ap, 0, 0)));
        }
        
        return PositionSignal;
    }
    
    [Button]
    public void RemoveSignals()
    {
        foreach (var signal in signals)
        {
            Destroy(signal);
        }
        
        signals.Clear();
    }

    public override void SeleccionarSitio()
    {
        base.SeleccionarSitio();
        
        gameObject.transform.position += despPosicionSelection;

        SpawnSignals();
    }

    public override void DeseleccionarSitio()
    {
        base.DeseleccionarSitio();
        
        gameObject.transform.position -= despPosicionSelection;
        
        RemoveSignals();
    }
}
