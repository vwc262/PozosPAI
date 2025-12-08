using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;

public class ControlStatusConexion : MonoBehaviour
{
    public GameObject statusConectado;
    public GameObject statusConectadoAct;
    public GameObject statusDesconectado;

    public GameObject connectioLocal;
    public GameObject connectioInternet;

    private void Start()
    {
        if (statusConectado != null) statusConectado.SetActive(false);
        if (statusDesconectado != null) statusDesconectado.SetActive(true);
        if (statusConectadoAct != null) statusConectadoAct.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (ControlRequest._singletonExists)
        {
            if (ControlRequest.singleton.errorInfreastructuraHTML() ||
                ControlRequest.singleton.errorUpdateHTML())
            {
                //Error en las comunicaciones
                if (statusConectado != null) statusConectado.SetActive(false);
                if (statusDesconectado != null) statusDesconectado.SetActive(true);
                if (statusConectadoAct != null) statusConectadoAct.SetActive(false);
                
                if (ControlAnimSinConexion._singletonExists)
                    ControlAnimSinConexion.singleton.SetEnableAnimSinConexion(true);
            }
            else if (ControlRequest.singleton.respInfraestructura)
            {
                if (ControlRequest.singleton.GetUpdateStatus())
                {
                    //Datos actualizados
                    if (statusConectado != null) statusConectado.SetActive(true);
                    if (statusDesconectado != null) statusDesconectado.SetActive(false);
                    //if (statusConectadoAct != null) statusConectadoAct.SetActive(false);
                }
                else
                {
                    //Datos no actualizadoss
                    if (statusConectado != null) statusConectado.SetActive(true);
                    if (statusDesconectado != null) statusDesconectado.SetActive(false);
                    if (statusConectadoAct != null) statusConectadoAct.SetActive(true);
                }
                
                if (ControlAnimSinConexion._singletonExists)
                    ControlAnimSinConexion.singleton.SetEnableAnimSinConexion(false);
            }

            if (ControlRequest.singleton.listRequestAPI[0].MyConectionData.useLocalHost)
            {
                if (connectioLocal != null) connectioLocal.SetActive(true);
                if (connectioInternet != null) connectioInternet.SetActive(false);
            }
            else
            {
                if (connectioLocal != null) connectioLocal.SetActive(false);
                if (connectioInternet != null) connectioInternet.SetActive(true);
            }
        }
    }
}
