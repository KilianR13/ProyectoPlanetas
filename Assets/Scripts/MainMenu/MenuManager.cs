using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject howToPlayMenu;
    // [SerializeField] private Scene loadScene;

    

    public void StartGame()
    {
        SceneManager.LoadScene("mainGame");
    }
    
    public void toggleHowToPlay()
    {
        howToPlayMenu.SetActive(!howToPlayMenu.activeSelf);

        // howToPlayMenu.activeSelf
    }
    
    public void CloseGame()
    {
        Application.Quit(); // Cierra el juego en Android
    }
}
