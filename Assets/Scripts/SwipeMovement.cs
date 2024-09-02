using UnityEngine;
using System.Collections;

public class SwipeMovement : MonoBehaviour
{
    private Vector2 startPos;
    private Vector2 endPos;
    public float swipeThreshold = 5f;
    public float fuerzaDeTiro = 1200f;
    public float fuerzaDeCurva = 15f;
    public float torqueEnderezador = 3.0f;
    public float delayBeforeLaunch = 0.15f; // Retraso antes de lanzar el objeto
    public float curveDuration = 0.5f; // Duración de la curva antes de que se enderece
    private Rigidbody m_Rigidbody;
    private Vector3 launchForce;
    private bool isDragging = false;
    private Camera mainCamera;
    private Vector3 initialCameraPosition;
    private bool hasBeenLaunched = false; // Variable para controlar si ya ha sido lanzado

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
        m_Rigidbody.isKinematic = true; // Hacer que el objeto sea agarrable
    }

    void Update()
    {
        if (hasBeenLaunched) return; // No permitir interacción si ya fue lanzado

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Ray ray = mainCamera.ScreenPointToRay(touch.position);
            RaycastHit hit;

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    if (Physics.Raycast(ray, out hit) && hit.transform == transform)
                    {
                        isDragging = true;
                        startPos = touch.position;
                        initialCameraPosition = mainCamera.transform.position;
                    }
                    break;

                case TouchPhase.Moved:
                    if (isDragging)
                    {
                        // Evitar que la cámara se mueva
                        mainCamera.transform.position = initialCameraPosition;

                        // Mover el objeto siguiendo el dedo dentro de límites razonables
                        Vector3 touchPosition = mainCamera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, mainCamera.WorldToScreenPoint(transform.position).z));
                        touchPosition.z = transform.position.z; // Mantener la posición z constante
                        transform.position = touchPosition;
                    }
                    break;

                case TouchPhase.Ended:
                    if (isDragging)
                    {
                        endPos = touch.position;
                        isDragging = false;
                        StartCoroutine(LaunchAfterDelay());
                    }
                    break;
            }
        }
    }

    IEnumerator LaunchAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeLaunch);
        hasBeenLaunched = true; // Marcar que ha sido lanzado
        DetectSwipe();
    }

    void DetectSwipe()
    {
        Vector2 swipeDelta = endPos - startPos;

        if (swipeDelta.magnitude >= swipeThreshold)
        {
            float x = swipeDelta.x;
            float y = swipeDelta.y;

            if (y > 0)  // Swipe hacia adelante
            {
                CalculateLaunchForce(x);
                StartCoroutine(ApplyCurvedLaunchForce(x));
            }
        }
    }

    void CalculateLaunchForce(float curve)
    {
        // Calcular la fuerza inicial de lanzamiento sin curva
        launchForce = Vector3.up * fuerzaDeTiro + Vector3.forward * fuerzaDeTiro / 2;
        launchForce += Vector3.right * curve * fuerzaDeCurva / Screen.width;
    }

    IEnumerator ApplyCurvedLaunchForce(float curve)
    {
        // Cambiar el Rigidbody a modo dinámico
        m_Rigidbody.isKinematic = false;

        // Aplicar la fuerza de lanzamiento inicial
        m_Rigidbody.AddForce(launchForce);

        // Variación de la curva a lo largo del tiempo
        float elapsedTime = 0f;
        Vector3 initialPosition = transform.position;
        while (elapsedTime < curveDuration)
        {
            // Reducir la fuerza de curvatura con el tiempo
            float curveFactor = Mathf.Lerp(1f, 0f, elapsedTime / curveDuration);
            Vector3 curveForce = Vector3.right * launchForce.x * curveFactor * torqueEnderezador;
            m_Rigidbody.AddForce(curveForce);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Al final del tiempo de curvatura, aplicar un torque de enderezamiento hacia la dirección de lanzamiento
        Vector3 enderezamiento = Vector3.left * launchForce.x * torqueEnderezador;
        m_Rigidbody.AddTorque(enderezamiento);
    }
}