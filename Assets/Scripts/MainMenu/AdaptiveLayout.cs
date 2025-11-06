using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class AdaptiveLayout : MonoBehaviour
{
    private VerticalLayoutGroup vertical;
    private HorizontalLayoutGroup horizontal;

    void Awake()
    {
        vertical = GetComponent<VerticalLayoutGroup>();
        horizontal = GetComponent<HorizontalLayoutGroup>();
        UpdateLayout();
    }

    void Update()
    {
        UpdateLayout();
    }

    void UpdateLayout()
    {
        bool isPortrait = Screen.height > Screen.width;

        if (vertical) vertical.enabled = isPortrait;
        if (horizontal) horizontal.enabled = !isPortrait;
    }
}
