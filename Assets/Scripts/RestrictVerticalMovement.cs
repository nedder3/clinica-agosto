using UnityEngine;

public class RestrictVerticalMovement : MonoBehaviour
{
    public float floorHeight = 0.5f;  // Altura mínima permitida (nivel del suelo)

    void LateUpdate()
    {
        // Restringir la posición Y del objeto para que no baje del suelo
        Vector3 position = transform.position;

        if (position.y < floorHeight)
        {
            position.y = floorHeight;
            transform.position = position;
        }
    }
}
