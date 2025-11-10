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
    [Header("UI Elements")]
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        time = 10f;
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
            // scoreText.text = targetFound; // Debug
            if (targetFound == currentTarget)
            {
                // El jugador ha acertado. Suma 1 punto y genera otro target
                score++;
                StartCoroutine(showImage(true));
                generateNextTarget();
                updateUI();
            }
            else
            {
                // El jugador no ha acertado. Quitar 1 vida al jugador y generar otro target
                playerFailed();
            }
            
        }
    }
    
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

    void generateNextTarget()
    {
        int randomPos = UnityEngine.Random.Range(0, targets.Count);
        currentTarget = targets[randomPos];
    }

    // Este código es estúpido
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
        if (!gameOver)
        {
            allowTimer = true;    
        }
    }
    private IEnumerator PlayerGameOver()
    {
        GameData.FinalScore = score;
        yield return new WaitForSecondsRealtime(2.0f);
        SceneManager.LoadScene("loseScreen");
    }

    public void pauseGame()
    {
        pauseMenu.SetActive(true);
        allowTimer = false;
        isPaused = true;
        Time.timeScale = 0f;

        // 🔹 Pausar la cámara y el reconocimiento AR
        if (VuforiaBehaviour.Instance != null)
        {
            VuforiaBehaviour.Instance.enabled = false; 
        }
    }

    public void unPauseGame()
    {
        pauseMenu.SetActive(false);
        isPaused = false;
        allowTimer = true;
        Time.timeScale = 1f;

        // 🔹 Reactivar la cámara y tracking
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
    

    void Update()
    {
        if (!gameOver && !isPaused)
        {
            timeLeftText.text = Mathf.Floor(time).ToString();
        }
        if (allowTimer)
        {
            time -= Time.deltaTime;
            if (time <= 0f)
            {
                allowTimer = false;
                playerFailed();
            }
        }
    }
}