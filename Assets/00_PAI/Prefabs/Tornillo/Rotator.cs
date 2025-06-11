using UnityEngine;

public class Rotator : MonoBehaviour
{

    public float rotSpeed = 3.5f;
    public Vector3 rotDirection;
    void Start() { }

    // Update is called once per frame
    void Update()
    {
  
        transform.Rotate(rotDirection * rotSpeed * Time.deltaTime);

    }
    
}
