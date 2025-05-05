using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UpdateUnitySites
{
    [SerializeField] public float Version;
    [SerializeField] public List<SiteBase> Sites = new();
}
