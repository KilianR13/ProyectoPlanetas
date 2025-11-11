using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Vuforia;


public class GameController : MonoBehaviour
{
    [Header("UI Images")]
    [SerializeField] private RawImage correctIMG;
    [SerializeField] private RawImage wrongIMG;

    [Header("TMP")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI targetNameText;
    public TextMeshProUGUI attemptsLeftText;
    public TextMeshProUGUI timeLeftText;

    [Header("SFX")]
    [SerializeField] private AudioSource correctSFX;
    [SerializeField] private AudioSource wrongSFX;

    [Header("Menus")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject confirmationMenu;


    private float time;
    private bool allowTimer;
    private bool isPaused;
    private bool gameOver;
    private string currentTarget;
    private int attemptsLeft = 3;
    private int score = 0;

    private List<string> targets = new List<string>() { "Sun", "Mercury", "Venus", "Earth", "Mars", "Jupiter", "Saturn", "Uranus", "Neptune" }; // Nombre de los imagetarget

    
    void Start()
    {
        allowTimer = true;
        isPaused = false;
        gameOver = false;
        correctIMG.gameObject.SetActive(false);
        wrongIMG.gameObject.SetActive(false);
        pauseMenu.SetActive(false);
        confirmationMenu.SetActive(false);
        generateNextTarget();
        updateUI();
    }

    void updateUI()
    {
        time = 10f;
        targetNameText.text = "Search for: " + currentTarget;
        attemptsLeftText.text = "Attempts: " + attemptsLeft;
        scoreText.text = "Score: " + score;
    }

    public void OnTargetFound(String targetFound)
    {
        if (attemptsLeft > 0 && allowTimer)
        {
            if (targetFound == currentTarget) // El jugador ha acertado. Suma 1 punto y genera otro target
            {
                score++;
                StartCoroutine(showImage(true));
                generateNextTarget();
                updateUI();
            }
            else // El jugador no ha acertado. Quita 1 vida al jugador y genera otro target
            {   
                playerFailed();
            }

        }
    }

    // Función para cuando el jugador no acierta o se le acaba el tiempo.
    private void playerFailed()
    {
        attemptsLeft--;
        updateUI();
        StartCoroutine(showImage(false));
        if (attemptsLeft <= 0)
        {
            // El jugador pierde.
            allowTimer = false;
            gameOver = true;
            timeLeftText.text = "";
            attemptsLeftText.text = "Attempts: 0";
            targetNameText.text = "GAME OVER!";
            StartCoroutine(PlayerGameOver());
        }
    }

    // Genera un nuevo target de forma aleatoria.
    void generateNextTarget()
    {
        int randomPos = UnityEngine.Random.Range(0, targets.Count);
        currentTarget = targets[randomPos];
    }

    // Maneja las imagenes que hay que enseñar al jugador, si ha acertado o ha fallado.
    private IEnumerator showImage(bool wasCorrect)
    {
        allowTimer = false;
        if (wasCorrect)
        {
            correctIMG.gameObject.SetActive(true);
            correctSFX.Play();
            yield return new WaitForSecondsRealtime(2.0f);
            correctIMG.gameObject.SetActive(false);
        }
        else
        {
            wrongIMG.gameObject.SetActive(true);
            wrongSFX.Play();
            yield return new WaitForSecondsRealtime(2.0f);
            wrongIMG.gameObject.SetActive(false);
        }
        if (!gameOver) // Reactiva el después de enseñar la imagen si el jugador no ha perdido el juego.
        {
            allowTimer = true;
        }
    }

    // Función para cuando el jugador pierda. Guarda la puntuación en GameData y carga la pantalla de fin del juego.
    private IEnumerator PlayerGameOver()
    {
        GameData.FinalScore = score;
        yield return new WaitForSecondsRealtime(2.0f);
        SceneManager.LoadScene("loseScreen");
    }

    // Función que controla cómo se pausa el juego.
    public void pauseGame()
    {
        pauseMenu.SetActive(true);
        allowTimer = false;
        isPaused = true;
        Time.timeScale = 0f; // Detiene el tiempo para contadores u otras funciones.

        // Pausa la cámara y el reconocimiento AR para evitar que el jugador por accidente reconozca una imagen mientras el juego está pausado.
        if (VuforiaBehaviour.Instance != null)
        {
            VuforiaBehaviour.Instance.enabled = false;
        }
    }

    // Función para renaudar el juego al despausarlo.
    public void unPauseGame()
    {
        pauseMenu.SetActive(false);
        isPaused = false;
        allowTimer = true;
        Time.timeScale = 1f; // Reanuda las funciones y contadores.

        // Reactiva la cámara y el tracking
        if (VuforiaBehaviour.Instance != null)
        {
            VuforiaBehaviour.Instance.enabled = true;
        }
    }

    public void leaveConfirmationOpen()
    {
        confirmationMenu.SetActive(true);
    }

    public void leaveConfirmationClose()
    {
        confirmationMenu.SetActive(false);
    }

    public void returnToMainMenu()
    {
        SceneManager.LoadScene("mainMenu");
    }

    public void reloadGame()
    {
        SceneManager.LoadScene("mainGame");
    }
    
    // El Update controla el tiempo que tiene el jugador para reconocer el objetivo correcto.
    void Update()
    {
        if (!gameOver && !isPaused)
        {
            timeLeftText.text = Mathf.Floor(time).ToString();
        }
        if (allowTimer)
        {
            time -= Time.deltaTime;
            if (time <= 0f) // Si se queda sin tiempo, el jugador falla y pierde una vida.
            {
                allowTimer = false;
                playerFailed();
            }
        }
    }
}