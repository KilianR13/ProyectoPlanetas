using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class ImageTargetHandler : MonoBehaviour
{
    private ObserverBehaviour observerBehaviour;
    [SerializeField] private GameController gameController;
    private string lastTarget = "";
    private float lastDetectionTime = 0f;
    private float detectionCooldown = 2f;

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
            string targetName = behaviour.TargetName;

            // Evitar repeticiones si es el mismo target demasiado pronto
            if (targetName == lastTarget && Time.time - lastDetectionTime < detectionCooldown)
                return;

            lastTarget = targetName;
            lastDetectionTime = Time.time;

            gameController.OnTargetFound(targetName);
        }
    }
}
