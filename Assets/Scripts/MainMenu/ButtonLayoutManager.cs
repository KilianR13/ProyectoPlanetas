using UnityEngine;

public class ButtonLayoutManager : MonoBehaviour
{
    [SerializeField] public GameObject panelHorizontal;
    [SerializeField] public GameObject panelVertical;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() => UpdateUI();

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
    }
    
    void UpdateUI()
    {
        bool isHorizontal = Screen.width > Screen.height;
        panelHorizontal.SetActive(isHorizontal);
        panelVertical.SetActive(!isHorizontal);
    }
}
