using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // [SerializeField] private Scene loadScene;
    public void StartGame()
    {
        SceneManager.LoadScene("mainGame"); 
    }
    
    public void CloseGame()
    {
        Application.Quit(); // Cierra el juego en Android
    }
}
