using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class loseScreen : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI scoreText;

    
    void Start()
    {
        scoreText.text = $"You obtained { GameData.FinalScore.ToString()} points"; // Toma los puntos almacenados en el script GameData
    }

    public void reloadGame()
    {
        SceneManager.LoadScene("mainGame");
    }
    
    public void backToMenu()
    {
        SceneManager.LoadScene("mainMenu");
    }
}
