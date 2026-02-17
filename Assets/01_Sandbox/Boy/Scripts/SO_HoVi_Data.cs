using UnityEngine;

[CreateAssetMenu(fileName = "SO_HoVi_Data", menuName = "Scriptable Objects/SO_HoVi_Data")]
public class SO_HoVi_Data : ScriptableObject
{
    public string HoViModelFileName = "Ollama_ModelSystem_Mika.HoVi";
    public string HoViCatConfigFileName = "Ollama_ModelChat_Config.HoVi";
    public string model = "HoVi_Mika";
    public GameObject prefab;
    
    public Vector3 startPosition;
    public Vector3 startRotation;
}
