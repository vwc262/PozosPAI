using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BalanceGastoGUITeoloyucan : BalanceGastoGUI
{
    public List<ControlBalance> ListBalances;
    public ControlBalance ControlBalanceTotal;
    public ControlBalance ControlBalanceSelected;

    public float BalanceTotal;
    public float BalanceSelected;

    private void Start()
    {
        if (ControlDatos._singletonExists)
            ControlDatos.singleton.DatosInicializados.AddListener(ChangeUIText);
    }
    
    public void ChangeUIText()
    {
        if (RequestAPI.Instance != null)
        {
            for (int i = 0; i < ListBalances.Count; i++)
            {
                if (RequestAPI.Instance.dataRequestAPI.regiones.Count > i)
                    ListBalances[i].SetLabel("Balance gasto " + ControlDatos.singleton.GetNameRegionByIndex(i));
            }
        }
        
        if (ControlBalanceSelected != null)
            ControlBalanceSelected.SetLabel("Balance gasto Seleccionados");

        if (ControlBalanceTotal != null)
            ControlBalanceTotal.SetLabel("Balance gasto Total");
    }

    public override void UpdateUI()
    {
        if (ControlSitiosUI_Lista._singletonExists)
        {
            for (int i = 0; i < ListBalances.Count; i++)
            {
                if (ControlSitiosUI_Lista.singleton.sitiosOrdenados.dictionaryListSitios.ContainsKey(i))
                    ListBalances[i].Balance = ControlSitiosUI_Lista.singleton.sitiosOrdenados.dictionaryListSitios[i]
                        .Select(x => x.sitio.GetGasto()).Sum();

                ListBalances[i].SetValue(ListBalances[i].Balance.ToString());
            }

            BalanceTotal = ListBalances.Sum(item => item.Balance);

            if (ControlBalanceTotal != null)
                ControlBalanceTotal.SetValue(BalanceTotal.ToString());

            BalanceSelected = 0;

            for (int i = 0; i < ListBalances.Count; i++)
            {
                if (ControlSitiosUI_Lista.singleton.sitiosOrdenados.dictionaryListSitios.ContainsKey(i))
                    BalanceSelected += ControlSitiosUI_Lista.singleton.sitiosOrdenados.dictionaryListSitios[i]
                        .Where(x => x.sitio.SelectedForAnalitics)
                        .Select(x => x.sitio.GetGasto()).Sum();
            }

            if (ControlBalanceSelected != null)
                ControlBalanceSelected.SetValue(BalanceSelected.ToString());
        }
    }
}
