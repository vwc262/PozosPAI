using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class ControlBombasUI_PAI : ControlBombasUI
{
    //public bool bombaPrendida;
    
    [FormerlySerializedAs("bombaGreen")] public GameObject bombaEncendido;
    [FormerlySerializedAs("bombaRed")] public GameObject bombaApagado;
    [FormerlySerializedAs("bombaBlue")] public GameObject bombaAlarmaArrancador;
    
    public GameObject buttonEncender;
    public GameObject buttonApagar;
    public GameObject buttonConfirmar;
    
    public GameObject msgMantenimiento;
    public GameObject msgEjecutando;
    
    public TMPro.TMP_Text textStatusPerilla;
    
    public override void UpdateUISitio(ControlSitio sitio)
    {
        bombaEncendido.SetActive(false);
        bombaApagado.SetActive(false);
        bombaAlarmaArrancador.SetActive(false);
        buttonEncender.SetActive(false);
        buttonApagar.SetActive(false);
        msgMantenimiento.SetActive(false);
        
        if (sitio != null)
        {
            List<SignalBase> bomba = sitio.dataSitio.GetSignal(SignalBase.TipoSignalEnum.BOMBA);
            List<SignalBase> perillaBomba = sitio.dataSitio.GetSignal(SignalBase.TipoSignalEnum.PERILLA_BOMBA);
            
            if (bomba.Count > 0)
            {
                //bombaPrendida = bomba[0].Valor == 1;

                switch (bomba[sitio.indexBomba].Valor)
                {
                    case 1:
                        bombaEncendido.SetActive(true);
                        break;

                    case 2:
                        bombaApagado.SetActive(true);
                        break;

                    case 0:
                        
                        break;

                    case 3:
                        bombaAlarmaArrancador.SetActive(true);
                        msgMantenimiento.SetActive(true);
                        break;
                }
                   
                
                if (sitio.dataInTime)
                {
                    if (perillaBomba.Count > 0)
                    {
                        if (perillaBomba[sitio.indexBomba].Valor >= 1 )
                        {
                            switch (bomba[sitio.indexBomba].Valor)
                            {
                                case 1:
                                    if (Time.time - sitio.timeLastCommand > 60)
                                        buttonApagar.SetActive(true);
                                    else
                                        buttonApagar.SetActive(false);
                                    break;

                                case 2:
                                    if (Time.time - sitio.timeLastCommand > 60)
                                        buttonEncender.SetActive(true);
                                    else
                                        buttonEncender.SetActive(false);
                                    break;
                            }
                        }
                        else
                        {
                            Debug.Log("Perilla : " + perillaBomba[sitio.indexBomba].Valor);
                        }
                    }
                }
            }

            if (textStatusPerilla != null)
            {
                if (perillaBomba.Count > 0)
                {
                    switch (perillaBomba[sitio.indexBomba].Valor)
                    {
                        default:
                            textStatusPerilla.text = "Mantenimiento";
                            break;
                        
                        case 1:
                            textStatusPerilla.text = "Remoto";
                            break;
                        
                        case 2:
                            textStatusPerilla.text = "Local";
                            break;
                    }
                }
            }
        }
    }
    
    public void ActivateLoginUI()
    {
        if (ControlLogin._singletonExists)
            ControlLogin.singleton.ActivateLoginPanel();
    }

    public bool ValidateUser()
    {
        //Validar que el usuario pueda operar la bomba
        if (selectedControlSitio != null)
        {
            List<SignalBase> perillaBomba = selectedControlSitio.dataSitio.GetSignal(SignalBase.TipoSignalEnum.PERILLA_BOMBA);
            
            if (perillaBomba.Count > 0)
            {
                switch (perillaBomba[0].Valor)
                {
                    case 0://OFF: no puede ser operada
                        return false;
                        break;
                    
                    case 1://Remoto: Todos los usuarios
                        return true;
                        break;
                    
                    case 2://Local: validar superusuarios
                        if (ControlLogin._singletonExists)
                        {
                            if (ControlLogin.singleton.login.Login.tipUsuario == 1)//--- Super usuario
                                return true;
                        }
                        return false;
                        break;
                }
            }
        }
        
        return false;
    }

    public void SendCommand(ControlBombas.BombaAction _action)
    {
        if (ControlBombas._singletonExists)
            ControlBombas.singleton.SendCommand(_action);
    }

    public void SetTimeLastCommand()
    {
        selectedControlSitio.timeLastCommand = Time.time;
    }
}
