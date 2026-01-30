using System;
using UnityEngine;

public class FuncAuxDateTime
{
    public static string GetDateFormat_DMAH(string dateString)
    {
        DateTime parsedDate;

        if (DateTime.TryParse(dateString, out parsedDate))
        {
            return parsedDate.ToString("dd/MM/yyyy  HH:mm") + " hrs.";
        }

        return "00/00/0000  00:00 hrs";
    }
    
    public static string GetDateFormat_DMA(string dateString)
    {
        DateTime parsedDate;

        if (DateTime.TryParse(dateString, out parsedDate))
        {
            return parsedDate.ToString("dd/MM/yyyy");
        }

        return "00/00/0000";
    }
}
