using System.Collections.Generic;
using UnityEngine;

public class ControlBombaParticular : MonoBehaviour
{
    public float updateRate = 5;
    private float countdown;

    public Color colorEncendido;
    public Color colorApagado;
    public Color colorSinDatos;
    public Color colorMantenimiento;
    
    public Material materialBombaParticular;
    //public Rotator rotatorBombaParticular;
    
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
                        materialBombaParticular.SetColor("_Color", colorSinDatos);
                        // rotatorBombaParticular.fill = 0;
                        // rotatorBombaParticular.fillSpeed = 0;
                        // rotatorBombaParticular.rotDirection = new Vector3(0, 0, 0);
                        
                        break;
                    case 1: 
                        materialBombaParticular.SetColor("_Color",colorEncendido);
                        // rotatorBombaParticular.fillSpeed = 0.6f;
                        // rotatorBombaParticular.rotDirection = new Vector3(0, 100, 0);
                        break;
                    case 2: 
                        materialBombaParticular.SetColor("_Color",colorApagado);
                        // rotatorBombaParticular.fill = 0;
                        // rotatorBombaParticular.fillSpeed = 0;
                        // rotatorBombaParticular.rotDirection = new Vector3(0, 0, 0);
                        break;
                    case 3: 
                        materialBombaParticular.SetColor("_Color",colorMantenimiento);
                        // rotatorBombaParticular.fill = 0;
                        // rotatorBombaParticular.fillSpeed = 0;
                        // rotatorBombaParticular.rotDirection = new Vector3(0, 0, 0);
                        break;
                }
            }

            
        }
    }
}
