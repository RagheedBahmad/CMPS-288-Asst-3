using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverText; // Assign in the Inspector
    public GameObject restartButton; // Assign in the Inspector

    void Start()
    {
        // Hide Game Over text and restart button at the start
        gameOverText.SetActive(false);
        restartButton.SetActive(false);
    }

    // Call this method to display the Game Over UI
    public void ShowGameOverScreen()
    {
        gameOverText.SetActive(true);
        restartButton.SetActive(true);
    }

    // Method linked to the restart button
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}