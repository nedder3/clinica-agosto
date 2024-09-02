using UnityEngine;

public class Ball : MonoBehaviour
{
    public int playerId;
    public float stopThreshold = 0.01f; // Umbral para considerar que la bola est� detenida
    public LayerMask groundLayer;       // Capa del suelo
    private Rigidbody rb;
    private bool isStopped = false;
    private TurnManager turnManager;   // A�ade una referencia al TurnManager

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        turnManager = FindObjectOfType<TurnManager>(); // Encuentra el TurnManager en la escena
    }

    void Update()
    {
        // Verificar si la bola est� en el suelo y si est� casi detenida
        if (IsGrounded() && IsStationary())
        {
            if (!isStopped)
            {
                isStopped = true;
                turnManager.BallStopped(gameObject); // Notifica que la bola se ha detenido
            }
        }
        else
        {
            isStopped = false;
        }
    }

    private bool IsGrounded()
    {
        // Verifica si la bola est� tocando el suelo usando un raycast hacia abajo
        return Physics.Raycast(transform.position, Vector3.down, 0.5f, groundLayer);
    }

    private bool IsStationary()
    {
        // Verifica si la velocidad de la bola es menor que el umbral
        return rb.velocity.magnitude < stopThreshold;
    }
}