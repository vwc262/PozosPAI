using UnityEngine;
using UnityEditor;

public class AsignarTexturas : EditorWindow
{
    public Material[] materiales;
    public Texture2D[] texturas;

    [MenuItem("Tools/Asignar Texturas a Materiales")]
    static void Init()
    {
        GetWindow<AsignarTexturas>("Asignar Texturas");
    }

    void OnGUI()
    {
        SerializedObject so = new SerializedObject(this);
        EditorGUILayout.PropertyField(so.FindProperty("materiales"), true);
        EditorGUILayout.PropertyField(so.FindProperty("texturas"), true);

        so.ApplyModifiedProperties();

        if (GUILayout.Button("Asignar"))
        {
            Asignar();
        }
    }

    void Asignar()
    {
        if (materiales.Length != texturas.Length)
        {
            Debug.LogError("La cantidad de materiales y texturas NO coincide.");
            return;
        }

        for (int i = 0; i < materiales.Length; i++)
        {
            materiales[i].mainTexture = texturas[i];
            EditorUtility.SetDirty(materiales[i]);
        }

        AssetDatabase.SaveAssets();
        Debug.Log("✔ Texturas asignadas correctamente");
    }
}