using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SFB;
using Sirenix.OdinInspector;
using UnityEngine;

public class BoyAforos : MonoBehaviour
{
    // Hola Boy
    public List<string> lineas;

    public List<dataPozoAforo> aforoPozos;
    
    public string csvPathIn = Application.dataPath + "/AFOROS LERMA SUR RODRIGUEZ 7 NOV 23 TODOS.csv";

    public ControlSitioUI controlSitioUI;
    
    [Button]
    public void ReadUpdateAforos()
    {
        csvPathIn = SelectFile();
        
        ReadAforos(csvPathIn);

        UpdateAforos();
    }

    public string SelectFile()
    {
        var paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", "csv", false);

        if (paths.Length > 0)
        {
            return paths[0];
        }
        
        return "";
    }
    
    //Formato archivo de aforos
    //# - 
    //      - Datos
    //      0 - Numero
    //      1 - Abreviatura
    //      2 - Nombre
    //      3 - Regional
    //      4 - Fecha
    //      5 - Aforo
    //      6 - Observaciones
    public void ReadAforos(String _filePath)
    {
        lineas.Clear();
        aforoPozos.Clear();
        
        if (File.Exists(_filePath))
        {
            lineas.AddRange(File.ReadAllLines(_filePath, Encoding.UTF8));

            for (int i = 0; i < lineas.Count; i++)
            {
                if (lineas[i].Substring(0, 1) != "#")
                {
                    var dato = lineas[i].Split(',');

                    if (dato.Length <= 1)
                        continue;

                    dataPozoAforo aforoPozo = new dataPozoAforo();

                    aforoPozo.Numero = dato[0];
                    aforoPozo.Abreviacion = dato[1];
                    aforoPozo.Nombre = dato[2];
                    aforoPozo.Regional = dato[3];
                    aforoPozo.Fecha = dato[4];
                    aforoPozo.Aforo = dato[5];
                    aforoPozo.Observaciones = dato[6];

                    aforoPozos.Add(aforoPozo);
                }
            }
        }
    }

    public void UpdateAforos()
    {
        if (controlSitioUI != null)
        {
            foreach (var sitio in controlSitioUI.sitios)
            {
                var uiSitio = sitio.GetComponent<ControlSelectSitio>();

                if (uiSitio != null)
                {
                    if (uiSitio.TooltipOverride != null)
                        uiSitio.TooltipOverride.enabled = false;
                        
                    if (uiSitio.toggleOverride != null)
                        uiSitio.toggleOverride.isOn = false;
                    
                    if (aforoPozos.Any(item => item.Abreviacion == uiSitio.sitio.MyDataSitio.abreviacion))
                    {
                        dataPozoAforo dataAforo =
                            aforoPozos.First(item => item.Abreviacion == uiSitio.sitio.MyDataSitio.abreviacion);
                        
                        if (dataAforo.Observaciones != "" && uiSitio.TooltipOverride != null)
                        {
                            uiSitio.TooltipOverride.infoLeft = dataAforo.Observaciones;
                            uiSitio.TooltipOverride.enabled = true;
                        }
                        
                        float valor;
                        
                        try
                        {
                            valor = float.Parse(dataAforo.Aforo);
                            uiSitio.dataAforo.gasto = valor;
                            
                            if (uiSitio.toggleOverride != null)
                                uiSitio.toggleOverride.isOn = true;
                        }
                        catch (Exception e)
                        {
                            valor = 0;
                            uiSitio.dataAforo.gasto = valor;
                        }
                        
                        uiSitio.updateDataAforoUI();
                    }
                    
                    if (uiSitio.toggleOverride != null)
                        uiSitio.toggleOverride.onValueChanged.Invoke(uiSitio.toggleOverride.isOn);
                    
                }
            }
        }
    }

    public void ClearAforos()
    {
        if (controlSitioUI != null)
        {
            foreach (var sitio in controlSitioUI.sitios)
            {
                var uiSitio = sitio.GetComponent<ControlSelectSitio>();

                if (uiSitio != null)
                {
                    if (uiSitio.TooltipOverride != null)
                        uiSitio.TooltipOverride.enabled = false;
                        
                    if (uiSitio.toggleOverride != null)
                        uiSitio.toggleOverride.isOn = false;
                    
                    if (uiSitio.toggleOverride != null)
                        uiSitio.toggleOverride.onValueChanged.Invoke(uiSitio.toggleOverride.isOn);
                }
            }
        }
    }

    public float GetParseStringToFloat(string _valor)
    {
        float valor;
        
        try
        {
            valor = float.Parse(_valor);
        }
        catch (Exception e)
        {
            valor = 0;
        }
        
        return valor;
    }
}

[Serializable]
public class dataPozoAforo
{
    public string Numero;
    public string Abreviacion;
    public string Nombre;
    public string Regional;
    public string Fecha;
    public string Aforo;
    public string Observaciones;
}
