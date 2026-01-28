using System;
using System.Linq;
using UnityEngine;

public class ResumenPozos : MonoBehaviour
{
    public float updateRate = 5;
    private float countdown;

    public int idRegion = 0;

    private int sumatoriaFalla;
    public TMPro.TMP_Text nameRegion;
    //public TMPro.TMP_Text total;
    public TMPro.TMP_Text noActRegion;
    public TMPro.TMP_Text actRegion;
    public TMPro.TMP_Text bombasEncendidas;
    public TMPro.TMP_Text bombasApagadas;
    public TMPro.TMP_Text bombasFalla;

    private void Start()
    {
        UpdateData();
    }

    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0)
        {
            UpdateData();
            countdown = updateRate;
        }
    }

    public void UpdateData()
    {
        if (ControlDatos._singletonExists)
        {
            //Nombre Ramal
            nameRegion.text = ControlDatos.singleton.GetNameRegionByID(ControlDatos.singleton.GetIDRegionByIndex(idRegion), 0);
            
            //Total Pozos
            // total
            //     .text = ControlDatos.singleton.listSitios.Where(x =>
            //     x.dataSitio.Estructura == ControlDatos.singleton.regiones[idRegion].idRegion).Count().ToString();
            
            //Sitios En linea
            actRegion.text = ControlDatos.singleton.listSitios.Where(x =>
                x.dataSitio.Estructura == ControlDatos.singleton.regiones[idRegion].idRegion).Count(y=>y.dataInTime == true).ToString();
            
            //Sitios Fuera de linea
            noActRegion.text = ControlDatos.singleton.listSitios.Where(x =>
                x.dataSitio.Estructura == ControlDatos.singleton.regiones[idRegion].idRegion).Count(y=>y.dataInTime == false).ToString();
            
            //Bomba Encendida
            bombasEncendidas.text = ControlDatos.singleton.listSitios.Where(x =>
                x.dataSitio.Estructura == ControlDatos.singleton.regiones[idRegion].idRegion).Count(y=>y.GetBomba() == 1 && y.dataInTime == true).ToString();
            
            //Bomba Apagada
            bombasApagadas.text = ControlDatos.singleton.listSitios.Where(x =>
                x.dataSitio.Estructura == ControlDatos.singleton.regiones[idRegion].idRegion).Count(y=>y.GetBomba() == 2 && y.dataInTime == true).ToString();
            
            
            //Bomba Falla
            
            //Bomba 0 No disponible
            sumatoriaFalla = ControlDatos.singleton.listSitios.Where(x =>
                    x.dataSitio.Estructura == ControlDatos.singleton.regiones[idRegion].idRegion).Count(y => y.GetBomba() == 0 && y.dataInTime == true) + 
                             
                    //Bomba 3 Fallo arrancador
                    ControlDatos.singleton.listSitios.Where(x =>
                         x.dataSitio.Estructura == ControlDatos.singleton.regiones[idRegion].idRegion).Count(y => y.GetBomba() == 3 && y.dataInTime == true) +
                             
                        //Sitios Fuera de linea
                        ControlDatos.singleton.listSitios.Where(x =>
                             x.dataSitio.Estructura == ControlDatos.singleton.regiones[idRegion].idRegion).Count(y=>y.dataInTime == false) ;
        
            
            //Bomba Falla
            bombasFalla.text = sumatoriaFalla.ToString();
        }
    }
}
