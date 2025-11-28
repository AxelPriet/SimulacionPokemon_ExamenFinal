using UnityEngine;
using UnityEngine.UI;

public class PauseMenuSelectionIndicator : MonoBehaviour
{
    [Header("Indicador visual")]
    public RectTransform indicator;
    public Vector3 offset = new Vector3(-50, 0, 0);

    void Awake()
    {
        // Evitar que el indicador bloquee los clicks/raycasts
        var img = indicator != null ? indicator.GetComponent<Image>() : null;
        if (img != null) img.raycastTarget = false;
    }

    void Update()
    {
        if (PauseMenuManager.Instance == null || indicator == null) return;

        int currentIndex = PauseMenuManager.Instance.GetCurrentIndex();
        Button[] menuButtons = PauseMenuManager.Instance.menuButtons;

        if (menuButtons != null && menuButtons.Length > 0 && currentIndex >= 0 && currentIndex < menuButtons.Length)
        {
            RectTransform selected = menuButtons[currentIndex].GetComponent<RectTransform>();
            if (selected != null)
            {
                indicator.position = selected.position + offset;
            }
        }
    }
}




