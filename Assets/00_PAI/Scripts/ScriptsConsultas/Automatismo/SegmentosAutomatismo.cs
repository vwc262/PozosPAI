using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SegmentosAutomatismo
{
    public List<SegmentoAutomatismo> Segmentos;

    public SegmentosAutomatismo()
    {
        Segmentos = new List<SegmentoAutomatismo>();
    }
}
