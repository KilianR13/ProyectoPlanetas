using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameController : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private RawImage correctIMG;
    [SerializeField] private RawImage wrongIMG;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI targetNameText;
    public TextMeshProUGUI attemptsLeftText;
    public TextMeshProUGUI timeLeftText;


    private float time;
    private bool allowTimer;
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
        gameOver = false;
        correctIMG.gameObject.SetActive(false);
        wrongIMG.gameObject.SetActive(false);
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
        if (attemptsLeft > 0)
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
            StartCoroutine(returnToMenu());
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
            yield return new WaitForSecondsRealtime(2.0f);
            correctIMG.gameObject.SetActive(false);
        }
        else
        {
            wrongIMG.gameObject.SetActive(true);
            yield return new WaitForSecondsRealtime(2.0f);
            wrongIMG.gameObject.SetActive(false);
        }
        if (!gameOver)
        {
            allowTimer = true;    
        }
    }
    private IEnumerator returnToMenu()
    {
        yield return new WaitForSecondsRealtime(4.0f);
        SceneManager.LoadScene("mainMenu");
    }

    void Update()
    {
        if (!gameOver)
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