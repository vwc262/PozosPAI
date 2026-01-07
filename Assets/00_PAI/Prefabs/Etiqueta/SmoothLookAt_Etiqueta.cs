using System.Collections;
using UnityEngine;

public class SmoothLookAt_Etiqueta : MonoBehaviour
{
    [Header("Target actual")]
    public Transform currentTarget;

    [Header("Settings")]
    [Tooltip("Velocidad de rotación")]
    public float rotationSpeed = 5f;

    [Tooltip("Duración de la transición al cambiar de target")]
    public float transitionTime = 0.5f;

    private Coroutine transitionCoroutine;

    void LateUpdate()
    {
        if (currentTarget == null) return;

        Vector3 direction = currentTarget.position - transform.position;
        if (direction.sqrMagnitude < 0.001f) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime * rotationSpeed
        );
    }

    /// <summary>
    /// Cambia el target con una transición suave
    /// </summary>
    public void ChangeTarget(Transform newTarget)
    {
        if (newTarget == null) return;

        if (transitionCoroutine != null)
            StopCoroutine(transitionCoroutine);

        transitionCoroutine = StartCoroutine(TransitionToTarget(newTarget));
    }

    private IEnumerator TransitionToTarget(Transform newTarget)
    {
        float elapsed = 0f;
        Quaternion startRotation = transform.rotation;

        while (elapsed < transitionTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / transitionTime;

            Vector3 direction = newTarget.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
            yield return null;
        }

        currentTarget = newTarget;
        transitionCoroutine = null;
    }
}

