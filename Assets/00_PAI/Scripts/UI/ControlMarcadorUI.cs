using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlMarcadorUI : MonoBehaviour
{
    public GameObject bomba;
    public GameObject bombaON;
    public GameObject bombaOFF;
    public GameObject bombaFAIL;

    public SitioGPS sitio;

    public void SetSitio(SitioGPS _sitio)
    {
        sitio = _sitio;

        sitio.controlMarcadorUI = this;
    }

    public void SetColorBomba(int _color)
    {
        if (bomba != null) bomba.SetActive(false);
        if (bombaON != null) bombaON.SetActive(false);
        if (bombaOFF != null) bombaOFF.SetActive(false);
        if (bombaFAIL != null) bombaFAIL.SetActive(false);
        
        switch (_color)
        {
            case 0:
                if (bomba != null) bomba.SetActive(true);
                break;
            case 1:
                if (bombaON != null) bombaON.SetActive(true);
                break;
            case 2:
                if (bombaOFF != null) bombaOFF.SetActive(true);
                break;
            default:
                if (bombaFAIL != null) bombaFAIL.SetActive(true);
                break;
        }
    }

    public void SelectSitio()
    {
        if (sitio != null)
            sitio.SetSelectedSitio();
    }
}
