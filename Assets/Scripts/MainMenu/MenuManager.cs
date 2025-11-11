using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject howToPlayMenu;    

    public void StartGame()
    {
        SceneManager.LoadScene("mainGame"); // Carga la parte jugable
    }
    
    public void toggleHowToPlay()
    {
        howToPlayMenu.SetActive(!howToPlayMenu.activeSelf); // Hace aparecer o desaparecer el menú de "Como Jugar"
    }
    
    public void CloseGame()
    {
        Application.Quit(); // Cierra el juego en Android
    }
}
