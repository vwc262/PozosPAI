using System;
#if UNITY_EDITOR
using AmplifyShaderEditor;
#endif
using Unity.VisualScripting;
using UnityEngine;
//using VTabs.Libs;
using Random = UnityEngine.Random;

public class ControlAnimSinConexion : Singleton<ControlAnimSinConexion>
{
    public bool isAnimEnabled;
    public Vector2 movDirection;
    public Vector2 AuxMovDirection;
    public Vector2 maxPosition;
    public Vector2 minPosition;
    public float speed;

    public GameObject rootUI;
    public GameObject rootAnimGO;
    private RectTransform myRectTransform;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myRectTransform = rootAnimGO.GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        movDirection = Rotate(Vector2.right, Random.Range(25f,75f));
        rootUI.SetActive(isAnimEnabled);
    }
    
    public static Vector2 Rotate(Vector2 v, float deg) => Quaternion.AngleAxis(deg, Vector3.forward) * v;

    // Update is called once per frame
    void Update()
    {
        if (myRectTransform.anchoredPosition.x > maxPosition.x) 
            AuxMovDirection.x = -1;
        if (myRectTransform.anchoredPosition.x < minPosition.x) 
            AuxMovDirection.x = 1;
        if (myRectTransform.anchoredPosition.y > maxPosition.y) 
            AuxMovDirection.y = -1;
        if (myRectTransform.anchoredPosition.y < minPosition.y) 
            AuxMovDirection.y = 1;
            
        myRectTransform.anchoredPosition += AuxMovDirection * movDirection * speed * Time.deltaTime;
    }

    public void SetEnableAnimSinConexion(bool _enable)
    {
        if (isAnimEnabled != _enable)
        {
            isAnimEnabled = _enable;
            rootUI.SetActive(_enable);
        }
    }
}
