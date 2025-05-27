using UnityEngine;

public class ControlConfiguration : PersistentSingleton<ControlConfiguration>
{
    public EstructurasAPI.Proyectos proyecto;
    
    public void SetProyecto(EstructurasAPI.Proyectos proyecto)
    {
        this.proyecto = proyecto;
    }
}
