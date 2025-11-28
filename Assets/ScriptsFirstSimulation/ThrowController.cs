using UnityEngine;
using UnityEngine.UI;

public class ThrowController : MonoBehaviour
{
    public static ThrowController Instance { get; private set; }

    [Header("Referencias")]
    public Transform guideLine;
    public GameObject pokeballObject;
    public Rigidbody2D pokeballRB;

    [Header("Referencias UI")]
    public Slider forceSlider;
    public Transform forceBar;

    [Header("Parámetros")]
    public float rotationSpeed = 100f;
    public float maxForce = 12f;
    public float chargeSpeed = 5f;

    [Header("Valores Simulados")]
    public float simulatedDirection = 0f;
    public bool simulatedChargePressed = false;
    public bool simulatedChargeHeld = false;
    public bool simulatedChargeReleased = false;

    [Header("Estado Actual")]
    [SerializeField] private float currentForce = 0f;
    [SerializeField] private bool launched = false;
    [SerializeField] private bool isCharging = false;

    private Vector3 initialPokeballPos;
    private Quaternion initialGuideRotation;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Otra instancia de ThrowController detectada. Eliminando esta.");
            Destroy(gameObject);
            return;
        }
        Instance = this;

        launched = false;
        isCharging = false;
        currentForce = 0f;
    }

    void Start()
    {
        if (pokeballObject != null)
            initialPokeballPos = pokeballObject.transform.position;

        if (guideLine != null)
            initialGuideRotation = guideLine.rotation;

        if (forceSlider != null)
        {
            forceSlider.minValue = 0f;
            forceSlider.maxValue = 1f;
            forceSlider.value = 0f;
        }
    }

    public void SimulateStep(float deltaTime)
    {
        if (launched) return;

        RotateGuide(deltaTime);
        HandleCharge(deltaTime);
        UpdateForceVisuals();
    }

    void RotateGuide(float deltaTime)
    {
        if (guideLine == null) return;

        guideLine.Rotate(0, 0, -simulatedDirection * rotationSpeed * deltaTime);
        var euler = guideLine.eulerAngles;
        guideLine.eulerAngles = new Vector3(euler.x, euler.y, euler.z);
    }

    void HandleCharge(float deltaTime)
    {
        if (simulatedChargePressed && !isCharging)
        {
            isCharging = true;
            currentForce = 0f;
        }

        if (isCharging && simulatedChargeHeld)
        {
            currentForce += chargeSpeed * deltaTime;
            currentForce = Mathf.Clamp(currentForce, 0, maxForce);
        }

        if (isCharging && simulatedChargeReleased)
        {
            Launch();
        }
    }

    void Launch()
    {
        if (currentForce < 0.5f)
        {
            Debug.Log("Fuerza insuficiente para lanzar");
            isCharging = false;
            currentForce = 0f;
            return;
        }

        isCharging = false;
        launched = true;

        if (guideLine != null)
            guideLine.gameObject.SetActive(false);

        if (pokeballRB != null && guideLine != null)
        {
            float angle = guideLine.eulerAngles.z * Mathf.Deg2Rad;
            Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;

            pokeballRB.linearVelocity = Vector2.zero;      // usar velocity (no linearVelocity) por compatibilidad
            pokeballRB.angularVelocity = 0f;
            pokeballRB.AddForce(dir * currentForce, ForceMode2D.Impulse);
        }
    }

    void UpdateForceVisuals()
    {
        float normalized = maxForce > 0f ? currentForce / maxForce : 0f;

        if (forceSlider != null)
            forceSlider.value = normalized;

        if (forceBar != null)
        {
            Vector3 scale = forceBar.localScale;
            scale.y = normalized;
            forceBar.localScale = scale;
        }
    }

    public void ResetThrow()
    {

        launched = false;
        isCharging = false;
        currentForce = 0f;
        simulatedDirection = 0f;
        ResetChargeInputs();

        if (pokeballObject != null)
        {
            pokeballObject.transform.position = initialPokeballPos;
            pokeballObject.transform.rotation = Quaternion.identity;
        }

        if (pokeballRB != null)
        {
            pokeballRB.linearVelocity = Vector2.zero;
            pokeballRB.angularVelocity = 0f;
            pokeballRB.Sleep(); // asegura reposo
        }

        if (guideLine != null)
        {
            guideLine.rotation = initialGuideRotation;
            guideLine.gameObject.SetActive(true);
        }

        UpdateForceVisuals();
    }

    // Entrada por eventos externos
    public void OnPokeballCaptured() => ResetThrow();
    public void OnPokeballMissed() => ResetThrow();

    // Getters
    public bool IsLaunched() => launched;
    public bool IsCharging() => isCharging;
    public float GetCurrentForce() => currentForce;

    // Input simulado
    public void SetDirection(float direction) => simulatedDirection = Mathf.Clamp(direction, -1f, 1f);
    public void PressCharge() { simulatedChargePressed = true; simulatedChargeHeld = true; simulatedChargeReleased = false; }
    public void HoldCharge() { simulatedChargePressed = false; simulatedChargeHeld = true; simulatedChargeReleased = false; }
    public void ReleaseCharge() { simulatedChargePressed = false; simulatedChargeHeld = false; simulatedChargeReleased = true; }
    public void ResetChargeInputs() { simulatedChargePressed = false; simulatedChargeHeld = false; simulatedChargeReleased = false; }
}









