using System;
using System.Collections;
using UnityEngine;

public class ControlDateTime_PAI : MonoBehaviour
{
    private Coroutine corrutinaTime;
    public DateTime currentDate;
    public float chargeRate;

    public string dateString;
    public string timeString;

    public TMPro.TMP_Text textFecha;
    public TMPro.TMP_Text textHora;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        this.corrutinaTime = StartCoroutine(getDateTime());
    }

    private IEnumerator getDateTime()
    {
        while (true)
        {
            currentDate = DateTime.Now;
            dateString = $"{currentDate.Day.ToString("00")}/" +
                         $"{currentDate.Month.ToString("00")}/" +
                         $"{currentDate.Year}";

            if (textFecha != null)
                textFecha.text = dateString;

            timeString = $"{currentDate.Hour.ToString("00")}:" +
                         $"{currentDate.Minute.ToString("00")} hrs";

            if (textHora != null)
                textHora.text = timeString;
            
            yield return new WaitForSeconds(chargeRate);
        }
    }
    
    public static string GetDateFormat_DMAH(string dateString)
    {
        DateTime parsedDate;

        if (DateTime.TryParse(dateString, out parsedDate))
        {
            return parsedDate.ToString("dd/MM/yyyy  hh:mm") + " hrs";
        }

        return "00/00/0000  00:00 hrs";
    }
}
