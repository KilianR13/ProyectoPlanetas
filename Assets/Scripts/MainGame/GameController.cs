using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI targetNameText;
    public TextMeshProUGUI attemptsLeftText;

    private string currentTarget;
    private int attemptsLeft = 3;
    private int score = 0;

    private List<string> targets = new List<string>() { "Sun", "Mercury", "Venus", "Earth", "Mars", "Jupiter", "Saturn", "Uranus", "Neptune" }; // Nombre de los imagetarget

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        generateNextTarget();
        updateUI();
    }

    void updateUI()
    {
        // scoreText.text = "Current score: " + score;
        targetNameText.text = "Search for: " + currentTarget;
        attemptsLeftText.text = "Attempts left: " + attemptsLeft;
        // scoreText.text = "Current score: " + score;
    }
    
    public void OnTargetFound(String targetFound)
    {
        if (attemptsLeft > 0)
        {
            scoreText.text = targetFound; // Debug
            if (targetFound == currentTarget)
            {
                // El jugador ha acertado. Suma 1 punto y genera otro target
                score++;
                generateNextTarget();
            }
            else
            {
                // El jugador no ha acertado. Quitar 1 vida al jugador y generar
                attemptsLeft--;
                if (attemptsLeft <= 0)
                {
                    // El jugador pierde.
                    GameOver();
                    return;
                }
            }
            updateUI();    
        }
    }

    void generateNextTarget()
    {
        int randomPos = UnityEngine.Random.Range(0, targets.Count);
        currentTarget = targets[randomPos];
    }
    private void GameOver()
    {
        attemptsLeftText.text = "Attempts left: 0";
        targetNameText.text = "GAME OVER!";
    }
}