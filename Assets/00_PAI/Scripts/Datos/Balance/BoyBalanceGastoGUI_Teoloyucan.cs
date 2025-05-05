using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoyBalanceGastoGUI_Teoloyucan : BoyBalanceGastoGUI
{
    public List<ControlBalance> ListBalances;
    public ControlBalance ControlBalanceTotal;
    public ControlBalance ControlBalanceSelected;

    public float BalanceTotal;
    public float BalanceSelected;

    private void Start()
    {
        if (ControlDatosAux._singletonExists)
            ControlDatosAux.singleton.DatosInicializados.AddListener(ChangeUIText);
    }
    
    public void ChangeUIText()
    {
        if (RequestAPI.Instance != null)
        {
            for (int i = 0; i < ListBalances.Count; i++)
            {
                if (RequestAPI.Instance.dataRequestAPI.regiones.Count > i)
                    ListBalances[i].SetLabel("Balance gasto " + ControlDatosAux.singleton.GetNameRegionByIndex(i));
            }
        }
        
        if (ControlBalanceSelected != null)
            ControlBalanceSelected.SetLabel("Balance gasto Seleccionados");

        if (ControlBalanceTotal != null)
            ControlBalanceTotal.SetLabel("Balance gasto Total");
    }

    public override void UpdateUI()
    {
        for (int i = 0; i < ListBalances.Count; i++)
        {
            if (sitiosOrdenados.dictionaryListSitios.ContainsKey(i))
                ListBalances[i].Balance = sitiosOrdenados.dictionaryListSitios[i]
                    .Select(x => x.sitio.GetGastoSitio()).Sum();

            ListBalances[i].SetValue(ListBalances[i].Balance.ToString());
        }

        BalanceTotal = ListBalances.Sum(item => item.Balance);

        if (ControlBalanceTotal != null)
            ControlBalanceTotal.SetValue(BalanceTotal.ToString());

        BalanceSelected = 0;

        for (int i = 0; i < ListBalances.Count; i++)
        {
            if (sitiosOrdenados.dictionaryListSitios.ContainsKey(i))
                BalanceSelected += sitiosOrdenados.dictionaryListSitios[i]
                    .Where(x => x.sitio.GetSitioSelectedForAnalitics())
                    .Select(x => x.sitio.GetGastoSitio()).Sum();
        }

        if (ControlBalanceSelected != null)
            ControlBalanceSelected.SetValue(BalanceSelected.ToString());
    }
}
