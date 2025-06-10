using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class ControlBombaParticular : MonoBehaviour
{
    public float updateRate = 5;
    private float countdown;

    public Color colorEncendido;
    public Color colorApagado;
    public Color colorSinDatos;
    public Color colorMantenimiento;
    
    public List<Renderer>  renderersBomba;
    public Rotator rotatorBombaParticular;
    
    void Start()
    {
        countdown = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0)
        {
            UpdateData();
            countdown = updateRate;
        }
    }

    private void UpdateData()
    {
        if (ParticularManager._singletonExists)
        {
            List<SignalBase> bomba = ParticularManager.singleton.sitio.dataSitio.GetSignal(SignalBase.TipoSignalEnum.BOMBA);
            
            if (bomba.Count > 0)
            {
                switch (bomba[0].Valor)
                {
                    case 0:
                        foreach (var rb in renderersBomba)
                        {
                            rb.material.SetColor("_BaseColor", colorSinDatos);
                            HDMaterial.ValidateMaterial(rb.material);
                        }
                        // rotatorBombaParticular.fill = 0;
                        //rotatorBombaParticular.fillSpeed = 0;
                        rotatorBombaParticular.rotDirection = new Vector3(0, 0, 0);
                        
                        break;
                    case 1: 
                        foreach (var rb in renderersBomba)
                        {
                            rb.material.SetColor("_BaseColor", colorEncendido);
                            HDMaterial.ValidateMaterial(rb.material);
                        }
                        // rotatorBombaParticular.fillSpeed = 0.6f;
                        rotatorBombaParticular.rotDirection = new Vector3(0, 100, 0);
                        break;
                    case 2: 
                        foreach (var rb in renderersBomba)
                        {
                            rb.material.SetColor("_BaseColor", colorApagado);
                            HDMaterial.ValidateMaterial(rb.material);
                        }
                        // rotatorBombaParticular.fill = 0;
                        // rotatorBombaParticular.fillSpeed = 0;
                        rotatorBombaParticular.rotDirection = new Vector3(0, 0, 0);
                        break;
                    case 3: 
                        foreach (var rb in renderersBomba)
                        {
                            rb.material.SetColor("_BaseColor", colorMantenimiento);
                            HDMaterial.ValidateMaterial(rb.material);
                        }
                        // rotatorBombaParticular.fill = 0;
                        // rotatorBombaParticular.fillSpeed = 0;
                        rotatorBombaParticular.rotDirection = new Vector3(0, 0, 0);
                        break;
                }
            }

            
        }
    }
}
