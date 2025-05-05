using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ControlBombasUI_PAI : ControlBombasUI
{
    public bool bombaPrendida;
    
    public GameObject bombaGreen;
    public GameObject bombaRed;
    public GameObject bombaBlue;
    
    public GameObject buttonEncender;
    public GameObject buttonApagar;
    public GameObject buttonConfirmar;
    
    public GameObject msgMantenimiento;
    public GameObject msgEjecutando;
    
    public TMPro.TMP_Text textStatusPerilla;
    
    public override void UpdateUISitio(SitioGPS _sitio)
    {
        bombaGreen.SetActive(false);
        bombaRed.SetActive(false);
        bombaBlue.SetActive(false);
        buttonEncender.SetActive(false);
        buttonApagar.SetActive(false);
        msgMantenimiento.SetActive(false);
        
        if (_sitio != null)
        {
            if (_sitio.MyDataSitio.bomba.Count > 0)
            {
                bombaPrendida = _sitio.MyDataSitio.bomba[0].Valor == 1;

                switch (_sitio.MyDataSitio.bomba[_sitio.indexBomba].Valor)
                {
                    case 1:
                        bombaGreen.SetActive(true);
                        break;

                    case 2:
                        bombaRed.SetActive(true);
                        break;

                    case 0:
                        
                        break;

                    case 3:
                        bombaBlue.SetActive(true);
                        msgMantenimiento.SetActive(true);
                        break;
                }
                   
                
                if (_sitio.statusDataInTime == 1)
                {
                    if (_sitio.MyDataSitio.PerillaBomba.Count > 0)
                    {
                        if (_sitio.MyDataSitio.PerillaBomba[_sitio.indexBomba].Valor >= 1 )
                        {
                            switch (_sitio.MyDataSitio.bomba[_sitio.indexBomba].Valor)
                            {
                                case 1:
                                    if (Time.time - _sitio.timeLastCommand > 60)
                                        buttonApagar.SetActive(true);
                                    else
                                        buttonApagar.SetActive(false);
                                    break;

                                case 2:
                                    if (Time.time - _sitio.timeLastCommand > 60)
                                        buttonEncender.SetActive(true);
                                    else
                                        buttonEncender.SetActive(false);
                                    break;
                            }
                        }
                        else
                        {
                            Debug.Log("Perilla : " + _sitio.MyDataSitio.PerillaBomba[_sitio.indexBomba].Valor);
                        }
                    }
                }
            }

            if (textStatusPerilla != null)
            {
                if (_sitio.MyDataSitio.PerillaBomba.Count > 0)
                {
                    switch (_sitio.MyDataSitio.PerillaBomba[_sitio.indexBomba].Valor)
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
        if (SelectedSitio != null)
        {
            if (SelectedSitio.MyDataSitio.PerillaBomba.Count > 0)
            {
                switch (SelectedSitio.MyDataSitio.PerillaBomba[0].Valor)
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
        SelectedSitio.timeLastCommand = Time.time;
    }
}
