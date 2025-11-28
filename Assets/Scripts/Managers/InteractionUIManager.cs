using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractionUIManager : MonoBehaviour
{
    public static InteractionUIManager Instance { get; private set; }

    [Header("Referencias")]
    public CanvasGroup canvasGroup;
    public TextMeshProUGUI interactText;
    public Image interactIcon;

    [Header("Animación")]
    public float fadeSpeed = 5f;

    private bool isVisible = false;
    private string currentText = "";

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Update()
    {
        float targetAlpha = isVisible ? 1 : 0;
        canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, targetAlpha, fadeSpeed * Time.deltaTime);
    }

    public void Show(string text, Sprite icon = null)
    {
        isVisible = true;

        if (interactIcon.sprite == icon && text == currentText && isVisible) return;

        currentText = text;
        interactText.text = text;
        interactText.enabled = true;

        if (icon != null)
        {
            interactIcon.enabled = true;
            interactIcon.sprite = icon;
        }
        else
        {
            interactIcon.enabled = false;
        }
    }

    public void Hide()
    {
        isVisible = false;
    }
}
