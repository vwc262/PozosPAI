using System;
using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class ControlPanelBarrientos :  MonoBehaviour
{
    ControlSitio sitio;
    public float updateRate = 5;
    private float _countdown;

    public TMPro.TMP_Text textNivel;
    public TMPro.TMP_Text textAlerta;
    
    public int indexNivel;
    public Image imageNivel;
    
    public List<Sprite> SprteNivelSitio;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
 
    }

    void Start()
    {
        if (!ControlAccesoPozosPAI.singleton.configuration.habilitarBarrientos)
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        _countdown -= Time.deltaTime;
        if(_countdown <= 0)
        {
            UpdatePanelBarrientos();
            _countdown = updateRate;
        }            
    }
    
    public void UpdatePanelBarrientos()
    {
        for (int i = 0; i < ControlDatos.singleton.listSitios.Count; i++)
        {
            if ((ControlDatos.singleton.listSitios[i].dataSitio.idSitio == 1421)) //ID Barrientos
            
            {
               float nivel = ControlDatos.singleton.listSitios[i].GetNivel(0);
                
                if (textNivel != null)
                { 
                    textNivel.text = $"Nivel: {nivel} m";
                }else
                {
                    textNivel.text = $"Nivel: N/A m";
                }
                
                if (textAlerta != null)
                {
                    if (nivel <= 0f)
                    {
                        textAlerta.text = $"N/A";
                    }
                    if (nivel > 0f && nivel < 1.8f)
                    {
                        textAlerta.text = $"Bajo";
                    }
                    if (nivel > 1.8f && nivel < 4.0f)
                    {
                        textAlerta.text = $"Medio";
                    }
                    if (nivel > 4.0f)
                    {
                        textAlerta.text = $"Alto";
                    }
                }
                
            }
            
            if (imageNivel != null)
            {
                indexNivel = ControlDatos.singleton.listSitios[i].GetIndiceNivel(0);

                imageNivel.sprite = SprteNivelSitio[indexNivel];
                
            }
        }
    }
}

