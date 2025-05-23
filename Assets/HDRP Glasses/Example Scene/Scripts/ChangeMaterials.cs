using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeMaterials : MonoBehaviour
{
    public Material[] materials;
    int currentMaterial = 0;
    public Text matName;
    private void Start()
    {
        StartCoroutine(startChanging());
    }
    IEnumerator startChanging()
    {
        while (true)
        {
            GetComponent<MeshRenderer>().material = materials[currentMaterial];
            yield return new WaitForSeconds(2);
            currentMaterial++;
            if (currentMaterial >= materials.Length) currentMaterial = 0;
        }
    }
    private void Update()
    {
        matName.text = materials[currentMaterial].name;
    }
}
