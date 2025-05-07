using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class ControlSitiosUIParticular_PAI : MonoBehaviour
{
    [FormerlySerializedAs("sitioSeleccionado")] public ControlMarcadorSitio controlMarcadorSitioSeleccionado;
    public float updateRate = 5;
    private float countdown;

    public int regional;
    public int regionalAnt;

    public int contEnLinea;
    public int contFueraDeLinea;

    public GameObject prefabSitioUI;
    public GameObject contentSitios;

    public List<GameObject> sitiosUIParticular = new List<GameObject>();

    public TMPro.TMP_Text TextRegionalNombre;
    public TMPro.TMP_Text TextEnLinea;
    public TMPro.TMP_Text TextFueraDeLinea;
    
    void Start()
    {
        if (ControlUpdateUI._singletonExists)
            ControlUpdateUI.singleton.SitioSeleccionadoSitioGPS.AddListener(UpdateInfoSitio);
    }
    
    private void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0)
        {
            if (controlMarcadorSitioSeleccionado != null)
                UpdateStatusSitio(controlMarcadorSitioSeleccionado);
            countdown = updateRate;
        }
    }
    
    public void UpdateInfoSitio(ControlMarcadorSitio controlMarcadorSitio)
    {
        controlMarcadorSitioSeleccionado = controlMarcadorSitio;
        UpdateStatusSitio(controlMarcadorSitioSeleccionado);

        SetRegional();
    }

    public void UpdateStatusSitio(ControlMarcadorSitio controlMarcadorSitio)
    {
        contEnLinea =
            sitiosUIParticular.Count(item => item.GetComponent<ControlSelectSitio>().sitio.dataInTime);
        
        contFueraDeLinea =
            sitiosUIParticular.Count(item => item.GetComponent<ControlSelectSitio>().sitio.dataInTime == false);
        
        if (TextEnLinea != null)
            TextEnLinea.text = contEnLinea.ToString();
        
        if (TextFueraDeLinea != null)
            TextFueraDeLinea.text = contFueraDeLinea.ToString();
    }

    public void SetRegional()
    {
        regional = controlMarcadorSitioSeleccionado.sitio.dataSitio.Estructura;

        if (regional != regionalAnt)
        {
            regionalAnt = regional;
            
            if (TextRegionalNombre != null)
                TextRegionalNombre.text = ControlDatos.singleton.GetNameRegionByID(regional);

            foreach (var uiSitio in sitiosUIParticular)
            {
                Destroy(uiSitio);
            }
            
            sitiosUIParticular.Clear();

            if (ControlDatos._singletonExists)
            {
                foreach (var sitio in ControlDatos.singleton.listSitios)
                {
                    //ControlMarcadorSitio controlMarcadorSitio = sitio.controlMarcadorMap;

                    // if (controlMarcadorSitio != null)
                    // {
                    if (sitio.dataSitio.Estructura == regional)
                    {
                        GameObject instance = Instantiate(prefabSitioUI, contentSitios.transform);
                        
                        ControlSelectSitio controlSitioUI = instance.GetComponent<ControlSelectSitio>();
                        if (controlSitioUI != null)
                            controlSitioUI.SetSitio(sitio);
                        
                        sitiosUIParticular.Add(instance);
                    }
                    // }
                }
            }
        }
    }
}
