using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class loseScreen : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI scoreText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scoreText.text = $"You obtained { GameData.FinalScore.ToString()} points";
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
