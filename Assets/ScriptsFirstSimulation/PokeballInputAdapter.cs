using UnityEngine;
using UnityEngine.InputSystem;

public class PokeballInputAdapter : MonoBehaviour
{
    [Header("Referencias")]
    public ThrowController throwController;

    [Header("Configuración Alternativa (Sin Input System)")]
    public bool useDirectInput = true;
    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;
    public KeyCode chargeKey = KeyCode.Space;
    public string horizontalAxis = "Horizontal";
    public string chargeButton = "Jump";

    private bool wasCharging = false;
    private bool isCharging = false;

    void Start()
    {
        if (throwController == null)
        {
            throwController = ThrowController.Instance;
            if (throwController == null)
            {
                Debug.LogError("ThrowController no encontrado!");
                return;
            }
        }

        if (!useDirectInput && InputManager.Instance != null)
        {
            Debug.Log("Usando Input System con eventos");
            InputManager.Instance.OnAimMove += HandleAimMove;
            InputManager.Instance.OnChargeStart += HandleChargeStart;
            InputManager.Instance.OnChargeHold += HandleChargeHold;
            InputManager.Instance.OnChargeRelease += HandleChargeRelease;
        }
        else
        {
            useDirectInput = true;
        }
    }

    void Update()
    {
        if (throwController == null) return;
        if (useDirectInput) ProcessDirectInput();
    }

    void ProcessDirectInput()
    {
        float direction = 0f;

        if (Input.GetKey(leftKey)) direction -= 1f;
        if (Input.GetKey(rightKey)) direction += 1f;

        try
        {
            float axisInput = Input.GetAxis(horizontalAxis);
            if (Mathf.Abs(axisInput) > 0.1f) direction = axisInput;
        }
        catch { }

        throwController.SetDirection(direction);

        isCharging = Input.GetKey(chargeKey);
        try
        {
            if (Input.GetButton(chargeButton)) isCharging = true;
        }
        catch { }

        if (isCharging && !wasCharging)
        {
            throwController.PressCharge();
            Debug.Log("Carga iniciada ");
        }
        else if (isCharging && wasCharging)
        {
            throwController.HoldCharge();
        }
        else if (!isCharging && wasCharging)
        {
            throwController.ReleaseCharge();
            Debug.Log("Lanzamiento ");
        }
        else
        {
            // Evitar limpiar inputs si está lanzado para no interferir en el reset
            if (!throwController.IsLaunched())
                throwController.ResetChargeInputs();
        }

        wasCharging = isCharging;
    }

    void HandleAimMove(float direction)
    {
        if (throwController != null) throwController.SetDirection(direction);
    }
    void HandleChargeStart()
    {
        if (throwController != null)
        {
            throwController.PressCharge();
        }
    }
    void HandleChargeHold()
    {
        if (throwController != null) throwController.HoldCharge();
    }
    void HandleChargeRelease()
    {
        if (throwController != null)
        {
            throwController.ReleaseCharge();
        }
    }

    void OnDestroy()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnAimMove -= HandleAimMove;
            InputManager.Instance.OnChargeStart -= HandleChargeStart;
            InputManager.Instance.OnChargeHold -= HandleChargeHold;
            InputManager.Instance.OnChargeRelease -= HandleChargeRelease;
        }
    }
}


