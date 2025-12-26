using UnityEngine;

public class DroneSphericalLimit : MonoBehaviour
{
    [Header("Referencia al dron")]
    public Transform drone;

    [Header("Radio del límite esférico")]
    public float radius = 10f;

    void LateUpdate()
    {
        if (drone == null) return;

        Vector3 center = transform.position;
        Vector3 offset = drone.position - center;

        // Si el dron sale del radio, se fuerza al borde de la esfera
        if (offset.magnitude > radius)
        {
            offset = offset.normalized * radius;
            drone.position = center + offset;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}