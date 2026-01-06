using UnityEngine;

public class ControlNivelParticular_Barrientos : MonoBehaviour
{
    [Range(0, 4)] public float nivel;
    public GameObject offsetNivel;
    
    public float updateRate = 5;
    private float _countdown;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _countdown -= Time.deltaTime;
        if(_countdown <= 0)
        {
            UpdateNivelTanque();
            _countdown = updateRate;
        }            
    }

    public void UpdateNivelTanque()
    {
        if (ParticularManager._singletonExists)
        {
            nivel = ParticularManager.singleton.sitio.GetNivel(0);
            
            if (offsetNivel != null)
            {
                if (ParticularManager.singleton.sitio.dataInTime)
                {
                    if (nivel > 5)
                    {
                        
                    }
                    Vector3 pos = offsetNivel.transform.localPosition;
                    pos.y = nivel;
                    
                    offsetNivel.transform.localPosition = pos;
                }
              
            }
        }
    }
}
