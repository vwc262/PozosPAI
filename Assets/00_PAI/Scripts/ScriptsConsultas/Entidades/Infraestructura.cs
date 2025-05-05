using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class Infraestructura
{
    [ListDrawerSettings(ShowIndexLabels = true, ListElementLabelName = "smallDescription")]
    [SerializeField] public List<SiteDescription> Sites;

    // public Infraestructura()
    // {
    //     Sites = new List<SiteDescription>();
    // }
}
