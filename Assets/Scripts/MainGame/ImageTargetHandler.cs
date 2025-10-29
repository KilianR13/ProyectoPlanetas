using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class ImageTargetHandler : MonoBehaviour
{
    private ObserverBehaviour observerBehaviour;
    [SerializeField] private GameController gameController;
    private List<string> targets = new List<string>() { "", "" };
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        observerBehaviour = GetComponent<ObserverBehaviour>();
        if (observerBehaviour)
        {
            observerBehaviour.OnTargetStatusChanged += OnTargetStatusChanged;
        }
    }

    private void OnTargetStatusChanged(ObserverBehaviour behaviour, TargetStatus status)
    {
        if (status.Status == Status.TRACKED)
        {
            gameController.OnTargetFound(behaviour.TargetName); // Detecta la imagen y le pasa el nombre de la imagen
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
