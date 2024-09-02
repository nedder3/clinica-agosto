using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ScoringArea : MonoBehaviour
{
    public int points = 1; // Asigna los puntos correspondientes a esta área
    public List<GameObject> players; // Lista de referencias a los jugadores

    private ScoreManager scoreManager;
    private List<Ball> ballsInArea = new List<Ball>(); // Lista para registrar las bolas que ya han sumado puntos

    void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        Ball ball = other.GetComponent<Ball>();
        if (ball != null && !ballsInArea.Contains(ball))
        {
            ballsInArea.Add(ball); // Agrega la bola a la lista para evitar sumar puntos múltiples

            int playerId = GetPlayerId(ball);
            if (playerId >= 0) // Asegurarse de que el playerId sea válido
            {
                scoreManager.AddScore(playerId, points);
            }
        }
    }

    // Método para obtener el ID del jugador basado en la bola
    private int GetPlayerId(Ball ball)
    {
        // Aquí se supone que cada bola tiene una referencia a su jugador
        // Asegúrate de que Ball tenga una propiedad playerId o algo similar
        if (ball != null)
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].GetComponent<Player>().balls.Contains(ball.gameObject))
                {
                    return i + 1; // El ID del jugador (1 para el primer jugador, 2 para el segundo, etc.)
                }
            }
        }
        return -1; // ID no válido
    }
}
