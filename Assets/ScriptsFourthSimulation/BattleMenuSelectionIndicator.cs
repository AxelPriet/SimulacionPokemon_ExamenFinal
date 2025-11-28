using UnityEngine;
using UnityEngine.UI;

public class BattleMenuSelectionIndicator : MonoBehaviour
{
    [Header("Indicador visual")]
    public RectTransform indicator;
    public Vector3 offset = new Vector3(-50, 0, 0);

    public BattleMenu battleMenu; // referencia al menú de batalla

    void Awake()
    {
        // Evitar que el indicador bloquee clicks/raycasts
        var img = indicator != null ? indicator.GetComponent<Image>() : null;
        if (img != null) img.raycastTarget = false;
    }

    void Update()
    {
        if (battleMenu == null || indicator == null) return;

        int currentIndex = battleMenu.selectedIndex;
        RectTransform[] options = battleMenu.optionPositions;

        if (options != null && options.Length > 0 && currentIndex >= 0 && currentIndex < options.Length)
        {
            RectTransform selected = options[currentIndex];
            if (selected != null)
            {
                indicator.position = selected.position + offset;
            }
        }
    }
}
