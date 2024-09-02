using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public List<int> playerScores = new List<int>(); // Lista de puntajes de los jugadores

    public GameObject teamAWinUI; // GameObject para mostrar el mensaje de victoria del equipo A
    public GameObject teamBWinUI; // GameObject para mostrar el mensaje de victoria del equipo B
    public GameObject drawUI;     // GameObject para mostrar el mensaje de empate

    void Start()
    {
        InitializePlayerScores(6); // Ajusta el número según la cantidad de jugadores que tienes
    }

    private void InitializePlayerScores(int numberOfPlayers)
    {
        playerScores.Clear();
        for (int i = 0; i < numberOfPlayers; i++)
        {
            playerScores.Add(0);
        }
    }

    public void AddScore(int playerId, int points)
    {
        if (playerId >= 1 && playerId <= playerScores.Count)
        {
            playerScores[playerId - 1] += points;
            Debug.Log("Player " + playerId + " Score: " + playerScores[playerId - 1]);
        }
        else
        {
            Debug.LogError("Invalid player ID: " + playerId);
        }
    }

    public int GetScore(int playerId)
    {
        if (playerId >= 1 && playerId <= playerScores.Count)
        {
            return playerScores[playerId - 1];
        }
        Debug.LogError("Invalid player ID: " + playerId);
        return 0;
    }

    public (int teamAScore, int teamBScore) GetTeamScores()
    {
        int teamAScore = GetScore(1) + GetScore(3) + GetScore(5);
        int teamBScore = GetScore(2) + GetScore(4) + GetScore(6);
        return (teamAScore, teamBScore);
    }
    public void UpdateScoreText(GameObject textObject)
    {
        if (textObject != null)
        {
            TextMeshProUGUI scoreText = textObject.GetComponent<TextMeshProUGUI>();
            if (scoreText != null)
            {
                // Obtén los puntajes
                int player1Score = GetScore(1) + GetScore(3) + GetScore(5); // Player 1 = Jugadores 1, 3 y 5
                int player2Score = GetScore(2) + GetScore(4) + GetScore(6); // Player 2 = Jugadores 2, 4 y 6

                // Determina el orden basado en los puntajes
                string firstPlace = player1Score >= player2Score ? "Player 1" : "Player 2";
                string secondPlace = player1Score >= player2Score ? "Player 2" : "Player 1";
                int firstScore = player1Score >= player2Score ? player1Score : player2Score;
                int secondScore = player1Score >= player2Score ? player2Score : player1Score;

                // Actualiza el texto de la UI
                scoreText.text = $"Puestos:\n" +
                                 $"1° {firstPlace} : {firstScore}\n" +
                                 $"2° {secondPlace} : {secondScore}";
            }
            else
            {
                Debug.LogError("El GameObject asignado no tiene un componente TextMeshProUGUI.");
            }
        }
        else
        {
            Debug.LogError("El GameObject para el texto es null.");
        }
    }


    public void ShowFinalResult()
    {
        Debug.Log("ShowFinalResult llamado");

        var (teamAScore, teamBScore) = GetTeamScores();

        GameObject scoreTextObject = GameObject.Find("Text (TMP)");
        if (scoreTextObject != null)
        {
            scoreTextObject.SetActive(false);
        }
        else
        {
            Debug.LogError("El GameObject 'Text (TMP)' no se encontró.");
        }

        // Desactiva todas las UI de resultado antes de activar la correcta
        if (teamAWinUI != null) teamAWinUI.SetActive(false);
        if (teamBWinUI != null) teamBWinUI.SetActive(false);
        if (drawUI != null) drawUI.SetActive(false);

        if (teamAScore > teamBScore)
        {
            if (teamAWinUI != null)
            {
                teamAWinUI.SetActive(true);
            }
        }
        else if (teamBScore > teamAScore)
        {
            if (teamBWinUI != null)
            {
                teamBWinUI.SetActive(true);
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


