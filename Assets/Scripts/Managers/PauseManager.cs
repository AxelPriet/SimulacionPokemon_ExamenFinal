using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class PauseMenuManager : MonoBehaviour
{
    public static PauseMenuManager Instance { get; private set; }

    [Header("Referencias UI")]
    public GameObject pausePanel;
    public Button[] menuButtons;

    // Estado
    private int currentIndex = 0;
    private bool isPaused = false;
    public bool IsPaused => isPaused;

    // Bloqueo tras confirmación
    private bool justConfirmed = false;

    // Navegación con histéresis (un paso por pulso)
    private float navPressThreshold = 0.6f;   // sensibilidad de pulsación
    private float navReleaseThreshold = 0.2f; // umbral para soltar
    private bool axisHeld = false;            // evita múltiples pasos mientras se mantiene

    public int GetCurrentIndex() => currentIndex;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnPause += TogglePause;

            // Subscribirse SOLO a eventos de UI cuando el menú esté activo
            InputManager.Instance.OnUIMove += HandleNavigation;
            InputManager.Instance.OnUIInteract += HandleConfirm;
        }

        if (pausePanel != null)
            pausePanel.SetActive(false);
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        if (isPaused) ActivatePause();
        else ResumeGame();
    }

    public void ActivatePause()
    {
        Time.timeScale = 0f;
        if (pausePanel != null) pausePanel.SetActive(true);

        // Estado inicial consistente
        currentIndex = 0;
        axisHeld = false;
        justConfirmed = false;

        SelectButton(currentIndex);
        Debug.Log("⏸ Juego en pausa");
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        if (pausePanel != null) pausePanel.SetActive(false);

        isPaused = false;
        axisHeld = false;
        justConfirmed = false;

        Debug.Log("▶ Juego reanudado");
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        if (pausePanel != null) pausePanel.SetActive(false);

        isPaused = false;
        axisHeld = false;
        justConfirmed = false;

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);

        Debug.Log("🔄 Escena reiniciada");
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        Debug.Log("❌ Juego cerrado");

#if UNITY_EDITOR
        // Esto detiene el Play Mode en el editor
        UnityEditor.EditorApplication.isPlaying = false;
#else
    // Esto cierra la aplicación en una build real
    Application.Quit();
#endif
    }


    // ✅ Nuevo método: volver a la escena inicial
    public void ReturnToStartScene()
    {
        Time.timeScale = 1f;
        if (pausePanel != null) pausePanel.SetActive(false);

        isPaused = false;
        axisHeld = false;
        justConfirmed = false;

        // Cambia "MainMenu" por el nombre real de tu escena inicial
        SceneManager.LoadScene("SampleScene");

        Debug.Log("🏠 Volviendo a la escena inicial");
    }

    // ================== NAVEGACIÓN (UI) ==================
    void HandleNavigation(Vector2 input)
    {
        if (!isPaused || justConfirmed) return;

        float y = input.y;

        // Pulsación: solo avanza una vez cuando supera el umbral
        if (!axisHeld && Mathf.Abs(y) >= navPressThreshold)
        {
            if (y > 0f)
                currentIndex = Mathf.Max(0, currentIndex - 1);
            else
                currentIndex = Mathf.Min(menuButtons.Length - 1, currentIndex + 1);

            SelectButton(currentIndex);
            axisHeld = true;
        }
        // Liberación: permitir otro paso cuando el eje vuelve a casi cero
        else if (axisHeld && Mathf.Abs(y) <= navReleaseThreshold)
        {
            axisHeld = false;
        }
    }

    // ================== CONFIRMACIÓN (UI) ==================
    void HandleConfirm()
    {
        if (!isPaused || justConfirmed) return;

        int indexToExecute = currentIndex; // congelar índice en este frame

        if (indexToExecute >= 0 && indexToExecute < menuButtons.Length)
        {
            Button button = menuButtons[indexToExecute];
            if (button != null)
            {
                Debug.Log($"✔ Confirmado índice {indexToExecute} → {button.name}");
                button.onClick.Invoke();
                StartCoroutine(LockInputsFor(0.1f)); // bloquear rebote
            }
        }
    }

    IEnumerator LockInputsFor(float seconds)
    {
        justConfirmed = true;
        yield return new WaitForSecondsRealtime(seconds);
        justConfirmed = false;
    }

    void SelectButton(int index)
    {
        if (menuButtons == null || menuButtons.Length == 0) return;

        // Reflejar visualmente, pero la ejecución siempre usa currentIndex
        EventSystem.current.SetSelectedGameObject(menuButtons[index].gameObject);
        Debug.Log("➡ Botón seleccionado (index): " + menuButtons[index].name);
    }
}






