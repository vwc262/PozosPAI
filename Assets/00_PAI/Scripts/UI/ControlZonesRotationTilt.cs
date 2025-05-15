using UnityEngine;

public class ControlZonesRotationTilt : MonoBehaviour
{
    public VWC_MoveCamera _moveCamera;

    public Vector3 AngleIn;
    public Vector3 AngleOut;

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(Vector3.Lerp(AngleOut, AngleIn, _moveCamera.tiltValue));
    }
}
