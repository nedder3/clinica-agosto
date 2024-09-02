using System.Collections.Generic;
using UnityEngine;
using TMPro; // que paquete verga

public class TurnManager : MonoBehaviour
{
    public Transform spawnPoint;  // Punto de aparición para las bolas
    public List<Player> turnOrder;  // Lista de jugadores que define el orden de turnos
    public Player playerFin;       // Player que indica el fin del juego
    public GameObject ballFin;     // Bola que se desactiva al final del juego
    private int currentTurnIndex = 0;
    private List<GameObject> activeBalls = new List<GameObject>();  // Lista de bolas activas

    /// <summary>
    /// para que termine esta mierda.
    /// </summary>
    public TextMeshProUGUI scoreText;        // Referencia al TextMeshPro para desactivar la UI de puntuaciones

    public GameObject player1WinUI;         // UI de victoria del jugador 1
    public GameObject player2WinUI;         // UI de victoria del jugador 2
    public GameObject drawUI;               // UI de empate
    public GameObject scoreManager;
    public AudioSource victoryAudioSource; // Referencia al AudioSource
    void Start()
    {
        // Verifica que la lista de turnos tenga al menos un jugador
        if (turnOrder == null || turnOrder.Count == 0)
        {
            Debug.LogError("La lista de turnos está vacía.");
            EndGame();
            return;
        }

        // Desactiva todas las bolas al inicio del juego
        foreach (var player in turnOrder)
        {
            foreach (var ball in player.balls)
            {
                ball.SetActive(false);
            }
        }

        // Inicia el primer turno
        StartTurn();
    }

    void StartTurn()
    {
        try
        {
            // Verifica si el índice de turno es válido
            if (currentTurnIndex < 0 || currentTurnIndex >= turnOrder.Count)
            {
                throw new System.IndexOutOfRangeException("Índice de turno fuera de rango.");
            }

            // Obtén el jugador actual
            Player currentPlayer = turnOrder[currentTurnIndex];

            // Verifica si el jugador actual es el PlayerFin
            if (currentPlayer == playerFin)
            {
                Debug.Log("Finalizando el juego. Activando PlayerFin.");
                // Desactiva la visibilidad de la bola de PlayerFin
                if (ballFin != null)
                {
                    ballFin.SetActive(false);
                }
                EndGame();
                return;
            }

            // Activa y posiciona la bola correspondiente del jugador actual en el SpawnPoint
            GameObject currentBall = currentPlayer.balls[0];  // Asumimos que solo se maneja una bola por turno
            currentBall.transform.position = spawnPoint.position;
            currentBall.SetActive(true);

            // Agrega la bola actual a la lista de bolas activas si aún no está en la lista
            if (!activeBalls.Contains(currentBall))
            {
                activeBalls.Add(currentBall);
            }

            // Configura la cámara para que siga la bola actual
            currentPlayer.cameraFollow.SetTarget(currentBall.transform);

            Debug.Log($"Turno {currentTurnIndex + 1}: Activando bola {currentBall.name} para el jugador {currentPlayer.name}");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error al iniciar el turno: {ex.Message}");
            EndGame();
        }
    }

    public void BallStopped(GameObject ball)
    {
        try
        {
            Debug.Log($"La bola {ball.name} se ha detenido.");

            // Resetea la cámara a la posición inicial
            foreach (var player in turnOrder)
            {
                player.cameraFollow.ResetToInitialPosition();
            }

            // Verifica si el índice de turno es válido
            if (currentTurnIndex < 0 || currentTurnIndex >= turnOrder.Count)
            {
                throw new System.IndexOutOfRangeException("Índice de turno fuera de rango al verificar la bola detenida.");
            }

            Player currentPlayer = turnOrder[currentTurnIndex];
            if (ball == currentPlayer.balls[0])
            {
                // Avanza al siguiente turno
                currentTurnIndex++;
                if (currentTurnIndex < turnOrder.Count)
                {
                    StartTurn();
                }
                else
                {
                    Debug.Log("Fin de todos los turnos.");
                }
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error al procesar la bola detenida: {ex.Message}");
            EndGame();
        }
    }
    // zona segura


    private void EndGame()
    {
        // Obtiene una referencia al ScoreManager
        ScoreManager scoreManagerScript = scoreManager.GetComponent<ScoreManager>();


        //sonido 
        // Reproduce el efecto de sonido
        if (victoryAudioSource != null)
        {
            victoryAudioSource.Play();
        }
        else
        {
            Debug.LogError("AudioSource no está asignado.");
        }





        if (scoreManagerScript == null)
        {
            Debug.LogError("El ScoreManager no está asignado o no tiene el componente ScoreManager.");
            return;
        }

        // Obtiene los puntajes de los equipos
        var (teamAScore, teamBScore) = scoreManagerScript.GetTeamScores();

        // Actualiza el texto del puntaje en la UI
        scoreText.text = $"Puestos:\n" +
                         $"1° Player 1 : {teamAScore}\n" +
                         $"2° Player 2 : {teamBScore}";

        // Desactiva todas las UI de resultado antes de activar la correcta
        if (player1WinUI != null) player1WinUI.SetActive(false);
        if (player2WinUI != null) player2WinUI.SetActive(false);
        if (drawUI != null) drawUI.SetActive(false);

        // Muestra el resultado final basado en los puntajes
        if (teamAScore > teamBScore)
        {
            if (player1WinUI != null)
            {
                player1WinUI.SetActive(true);
            }
        }
        else if (teamBScore > teamAScore)
        {
            if (player2WinUI != null)
            {
                player2WinUI.SetActive(true);
            }
        }
        else
        {
            if (drawUI != null)
            {
                drawUI.SetActive(true);
            }
        }
    }





}