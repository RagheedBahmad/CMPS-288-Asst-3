using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverText;
    public GameObject restartButton; 

    void Start() // hide text and button initially
    {
        gameOverText.SetActive(false);
        restartButton.SetActive(false);
    }

    public void ShowGameOverScreen() // show text and button
    {
        gameOverText.SetActive(true);
        restartButton.SetActive(true);
    }

    public void RestartGame() // restart scene
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}