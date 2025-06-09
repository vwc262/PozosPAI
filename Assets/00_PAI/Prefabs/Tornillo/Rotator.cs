using UnityEngine;

public class Rotator : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float rotSpeed = 3.5f;
    public float fill = 0f;
    public float fillSpeed = 0.6f;
    public Renderer rend;
    public Vector3 rotDirection;
    void Start() { }

    // Update is called once per frame
    void Update()
    {
        //fill += Time.deltaTime * fillSpeed;
        transform.Rotate(rotDirection * rotSpeed * Time.deltaTime);
       // rend.material.SetFloat("_Threshold", fill);
    }
    
}
