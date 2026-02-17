using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;

public class BoyOllamaInterprete : MonoBehaviour
{
    // public Sprite[] emosionSprites;
    // public Sprite[] accionSprites;
    // public Sprite[] tareaSprites;
    
    public AIVirtualHostAnserData aIVirtualHostAnserData;
    // public Sprite emosionActual; 
    // public Sprite accionActual; 
    // public Sprite tareaActual; 

    public bool debug;

    public UnityEvent<AIVirtualHostAnserData> AnswerInterpretation;
    public UnityEvent<string> speechTextEvent;
    
    public void Interpretation(string _answer)
    {
        var answer = QuitarAcentos(_answer);
        aIVirtualHostAnserData.dialogo = "";
        if (TryParse(answer, out aIVirtualHostAnserData))
        {
            if(debug) print($"Animo={aIVirtualHostAnserData.animo} \n" +
                            $"Accion={aIVirtualHostAnserData.accion} \n" +
                            $"Tarea={aIVirtualHostAnserData.tares} \n" +
                            $"Dir={aIVirtualHostAnserData.direccionMovimiento} \n" +
                            $"Dist={aIVirtualHostAnserData.distanciaMovimiento}");

            if ((aIVirtualHostAnserData.accion == Acciones.Platicar ||
                 aIVirtualHostAnserData.accion == Acciones.MostrarInfo)
                && aIVirtualHostAnserData.dialogo != "")
            {
                
                speechTextEvent.Invoke(aIVirtualHostAnserData.dialogo);
            }
            
            // emosionActual = emosionSprites[(int)aIVirtualHostAnserData.animo];
            // accionActual = accionSprites[(int)aIVirtualHostAnserData.accion];
            // tareaActual = tareaSprites[(int)aIVirtualHostAnserData.tares];

            AnswerInterpretation.Invoke(aIVirtualHostAnserData);
        }
    }

    public string QuitarAcentos(string texto)
    {
        if (string.IsNullOrEmpty(texto))
            return texto;

        // Separa letras de sus acentos
        string normalizado = texto.Normalize(NormalizationForm.FormD);
        StringBuilder sb = new StringBuilder(normalizado.Length);

        foreach (char c in normalizado)
        {
            UnicodeCategory uc = Char.GetUnicodeCategory(c);

            // Ignora marcas diacriticas (acentos)
            if (uc != UnicodeCategory.NonSpacingMark)
                sb.Append(c);
        }

        // Regresa a forma normal
        return sb.ToString().Normalize(NormalizationForm.FormC);
    }
   
    // Ejemplo de input:
    // ESTADO: Alegria
    // ACCION: SolicitudNoProgramada
    // TAREA: Movimiento
    // PARAMETROS: dir: (0,5), distancia: 5
    // DIALOGO: Moviéndome hacia adelante
    public bool TryParse(string raw, out AIVirtualHostAnserData data)
    {
        data = new AIVirtualHostAnserData
        {
            animo = EstadoAnimo.Alegria,
            accion = Acciones.Platicar,
            tares = Tareas.Movimiento,
            direccionMovimiento = Vector2.zero,
            distanciaMovimiento = 0f
        };

        if (string.IsNullOrWhiteSpace(raw)) return false;

        // Normaliza saltos de línea
        raw = raw.Replace("\r\n", "\n").Replace("\r", "\n");

        // ESTADO / ACCION / TAREA
        if (TryGetLineValue(raw, "ESTADO", out var estadoStr))
            data.animo = ParseEnumFlexible<EstadoAnimo>(estadoStr);

        if (TryGetLineValue(raw, "ACCION", out var accionStr))
            data.accion = ParseEnumFlexible<Acciones>(accionStr);

        if (TryGetLineValue(raw, "TAREA", out var tareaStr))
            data.tares = ParseEnumFlexible<Tareas>(tareaStr);

        // DIALOGO
        if (TryParseDialogo(raw, out var dialogo))
            data.dialogo = dialogo;
        
        // PARAMETROS: dir: (0,5), distancia: 5
        if (TryGetLineValue(raw, "PARAMETROS", out var paramsStr))
            ParseParametros(paramsStr, ref data);

        // Considera "válido" si al menos encontró ESTADO/ACCION/TAREA o PARAMETROS
        return true;
    }

    private bool TryGetLineValue(string raw, string key, out string value)
    {
        // Busca "KEY: ...." hasta fin de línea
        var m = Regex.Match(raw, @"(?im)^\s*" + Regex.Escape(key) + @"\s*:\s*(.+?)\s*$");
        if (m.Success)
        {
            value = m.Groups[1].Value.Trim();
            return true;
        }
        value = "";
        return false;
    }

    private void ParseParametros(string s, ref AIVirtualHostAnserData data)
    {
        // dir: (0,5)  ó dir:(0.2, -1)
        var dirMatch = Regex.Match(s, @"(?i)\bdir(?:eccion)?\s*:\s*\(\s*([+-]?\d+(?:[.,]\d+)?)\s*,\s*([+-]?\d+(?:[.,]\d+)?)\s*\)");
        if (dirMatch.Success)
        {
            float x = ParseFloatFlexible(dirMatch.Groups[1].Value);
            float y = ParseFloatFlexible(dirMatch.Groups[2].Value);
            data.direccionMovimiento = new Vector2(x, y);
        }

        // distancia: 5   ó dist: 5.5
        var distMatch = Regex.Match(s, @"(?i)\b(distancia|dist)\s*:\s*([+-]?\d+(?:[.,]\d+)?)");
        if (distMatch.Success)
        {
            data.distanciaMovimiento = ParseFloatFlexible(distMatch.Groups[2].Value);
        }
    }

    private static bool TryParseDialogo(string raw, out string dialogo)
    {
        dialogo = "";

        if (string.IsNullOrWhiteSpace(raw))
            return false;

        raw = raw.Replace("\r\n", "\n").Replace("\r", "\n");

        // Encuentra la posición de "DIALOGO:"
        var index = CultureInfo.InvariantCulture.CompareInfo
            .IndexOf(raw, "DIALOGO:", CompareOptions.IgnoreCase);

        if (index < 0)
            return false;

        // Toma todo lo que sigue después de "DIALOGO:"
        int start = index + "DIALOGO:".Length;
        dialogo = raw.Substring(start).Trim();

        return !string.IsNullOrEmpty(dialogo);
    }
    
    private float ParseFloatFlexible(string s)
    {
        // Acepta "1.25" o "1,25"
        s = s.Trim().Replace(',', '.');
        if (float.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out var f))
            return f;
        return 0f;
    }

    private  T ParseEnumFlexible<T>(string s) where T : struct
    {
        // Permite cosas tipo "Solicitud No Programada" o "solicitud_no_programada"
        s = (s ?? "").Trim();
        s = s.Replace(" ", "").Replace("_", "").Replace("-", "");

        foreach (var name in Enum.GetNames(typeof(T)))
        {
            var norm = name.Replace(" ", "").Replace("_", "").Replace("-", "");
            if (string.Equals(norm, s, StringComparison.OrdinalIgnoreCase))
                return (T)Enum.Parse(typeof(T), name, true);
        }

        // fallback: intenta directo
        if (Enum.TryParse<T>(s, true, out var parsed))
            return parsed;

        return default;
    }
}
