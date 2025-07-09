using UnityEngine;

public class ControlGraph : MonoBehaviour
{
    public void OpenGraphFull()
    {
        if (ControlGraphFull._singletonExists)
            ControlGraphFull.singleton.SetEnableGraph(true);
    }
}
