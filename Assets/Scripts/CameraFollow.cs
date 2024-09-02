using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;    // Transform que la cámara debe seguir
    public Vector3 offset;      // Offset para mantener la distancia deseada de la esfera
    private Vector3 initialPosition;   // Posición inicial de la cámara

    void Start()
    {
        // Guarda la posición inicial de la cámara
        initialPosition = transform.position;
    }

    void LateUpdate()
    {
        // Verifica si hay un objetivo válido
        if (target != null)
        {
            // Mantén la cámara a una distancia fija del objetivo
            transform.position = target.position + offset;

            // Apunta la cámara hacia el objetivo
            transform.LookAt(target);
        }
    }

    // Establece el objetivo de la cámara
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    // Resetea la posición de la cámara a la posición inicial
    public void ResetToInitialPosition()
    {
        transform.position = initialPosition;
        // Opcional: puedes resetear la rotación si es necesario
        transform.rotation = Quaternion.identity;
    }
}
