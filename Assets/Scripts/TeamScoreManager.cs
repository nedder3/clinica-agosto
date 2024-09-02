using UnityEngine;
using TMPro;

public class TeamScoreManager : MonoBehaviour
{
    public ScoreManager scoreManager; // Referencia al ScoreManager
    public GameObject scoreTextObject; // Referencia al GameObject del texto de puntajes

    void Update()
    {
        if (scoreManager != null && scoreTextObject != null)
        {
            // Actualiza el texto del puntaje
            scoreManager.UpdateScoreText(scoreTextObject);
        }
        else
        {
            Debug.LogError("Referencias no asignadas en TeamScoreManager.");
        }
    }
}
