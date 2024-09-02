using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;    // Transform que la c�mara debe seguir
    public Vector3 offset;      // Offset para mantener la distancia deseada de la esfera
    private Vector3 initialPosition;   // Posici�n inicial de la c�mara

    void Start()
    {
        // Guarda la posici�n inicial de la c�mara
        initialPosition = transform.position;
    }

    void LateUpdate()
    {
        // Verifica si hay un objetivo v�lido
        if (target != null)
        {
            // Mant�n la c�mara a una distancia fija del objetivo
            transform.position = target.position + offset;

            // Apunta la c�mara hacia el objetivo
            transform.LookAt(target);
        }
    }

    // Establece el objetivo de la c�mara
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    // Resetea la posici�n de la c�mara a la posici�n inicial
    public void ResetToInitialPosition()
    {
        transform.position = initialPosition;
        // Opcional: puedes resetear la rotaci�n si es necesario
        transform.rotation = Quaternion.identity;
    }
}
