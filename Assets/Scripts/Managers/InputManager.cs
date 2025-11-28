using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.XInput;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    [SerializeField] private List<GamepadIconMap> gamepadMaps;

    public static InputManager Instance { get; private set; }
    public InputDevice CurrentDevice { get; private set; }
    public Gamepad CurrentGamepad { get; private set; }

    private PlayerController controls;

    public event Action<Vector2> OnMove;
    public event Action<Vector2> OnCameraMove;
    public event Action<float> OnCameraZoom;
    public event Action OnRecenterCamera;
    public event Action OnInteract;

    public event Action<Vector2> OnUIMove;
    public event Action OnUIInteract;
    public event Action OnToggleInventory;
    public event Action OnUICancel;

    public event Action<float> OnAimMove;
    public event Action OnChargeStart;
    public event Action OnChargeHold;
    public event Action OnChargeRelease;

    public event Action OnPause;

    private void OnSceneChanged(Scene oldScene, Scene newScene)
    {
        OnMove = null;
        OnCameraMove = null;
        OnCameraZoom = null;
        OnRecenterCamera = null;
        OnInteract = null;

        OnUIMove = null;
        OnUIInteract = null;
        OnToggleInventory = null;
        OnUICancel = null;

        OnAimMove = null;
        OnChargeStart = null;
        OnChargeHold = null;
        OnChargeRelease = null;

        OnPause = null;
    }

    void Awake()
    {
        SceneManager.activeSceneChanged += OnSceneChanged;

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        controls = new PlayerController();

        controls.Player.Move.performed += ctx => RouteMove(ctx.ReadValue<Vector2>());
        controls.Player.Move.canceled += ctx => RouteMove(Vector2.zero);

        controls.Player.CameraMove.performed += ctx => OnCameraMove?.Invoke(ctx.ReadValue<Vector2>());
        controls.Player.CameraMove.canceled += ctx => OnCameraMove?.Invoke(Vector2.zero);

        controls.Player.Zoom.performed += ctx => OnCameraZoom?.Invoke(ctx.ReadValue<float>());
        controls.Player.Zoom.canceled += ctx => OnCameraZoom?.Invoke(0);

        controls.Player.RecenterCamera.performed += _ => OnRecenterCamera?.Invoke();

        controls.Player.Interact.performed += _ =>
        {
            if (PauseMenuManager.Instance != null && PauseMenuManager.Instance.IsPaused)
                OnUIInteract?.Invoke();
            else
                OnInteract?.Invoke();
        };

        controls.Player.ToggleInventory.performed += _ => OnToggleInventory?.Invoke();

        controls.Player.Move.performed += ctx =>
        {
            Vector2 input = ctx.ReadValue<Vector2>();
            OnAimMove?.Invoke(input.x);
        };
        controls.Player.Move.canceled += ctx => OnAimMove?.Invoke(0f);

        controls.Player.RecenterCamera.started += _ => OnChargeStart?.Invoke();
        controls.Player.RecenterCamera.performed += _ => OnChargeHold?.Invoke();
        controls.Player.RecenterCamera.canceled += _ => OnChargeRelease?.Invoke();

        controls.Player.Pause.performed += _ => OnPause?.Invoke();

        InputSystem.onActionChange += OnActionChange;
    }

    public void OnUINavigate(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
            BattleInputBroker.OnUIMove(ctx.ReadValue<Vector2>());
        if (ctx.canceled)
            BattleInputBroker.OnUIMove(Vector2.zero); // ← esto es vital
    }


    public void OnUISubmit(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
            BattleInputBroker.OnUIInteract();
    }

    public void HandleUICancel(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            BattleInputBroker.OnUICancel();
            OnUICancel?.Invoke();
        }
    }

    private void RouteMove(Vector2 val)
    {
        if (PauseMenuManager.Instance != null && PauseMenuManager.Instance.IsPaused)
            OnUIMove?.Invoke(val);
        else
            OnMove?.Invoke(val);
    }

    private void OnActionChange(object obj, InputActionChange change)
    {
        if (change == InputActionChange.ActionPerformed && obj is InputAction action)
        {
            InputDevice device = action.activeControl.device;
            if (device is Mouse) return;

            CurrentDevice = device;
            CurrentGamepad = Gamepad.current;
        }
    }

    void OnDestroy()
    {
        InputSystem.onActionChange -= OnActionChange;
        SceneManager.activeSceneChanged -= OnSceneChanged;
    }

    void OnEnable()
    {
        controls.Player.Enable();
    }

    void OnDisable()
    {
        if (controls != null)
            controls.Player.Disable();

        InputSystem.onActionChange -= OnActionChange;
        SceneManager.activeSceneChanged -= OnSceneChanged;
    }

    // ✅ MÉTODOS PARA OBTENER NOMBRE E ÍCONO DE TECLA
    public string GetKeyName(string actionName)
    {
        InputBinding? binding = FindBindingForAction(actionName);
        if (binding == null) return "E";
        return GetBindingName(binding.Value);
    }

    public Sprite GetKeyIcon(string actionName)
    {
        InputBinding? binding = FindBindingForAction(actionName);
        if (binding == null) return null;
        return GetBindingIcon(binding.Value);
    }

    private InputBinding? FindBindingForAction(string actionName)
    {
        var action = controls.asset.FindAction(actionName);
        if (action == null) return null;

        CurrentDevice ??= Keyboard.current;

        foreach (var binding in action.bindings)
        {
            if (binding.isComposite || binding.isPartOfComposite) continue;
            if (BindingMatchesDevice(binding)) return binding;
        }

        return null;
    }

    private bool BindingMatchesDevice(InputBinding binding)
    {
        string path = binding.effectivePath;

        if (CurrentDevice is Keyboard) return path.Contains("Keyboard");
        if (CurrentDevice is Gamepad) return path.Contains("Gamepad");

        return false;
    }

    private string GetBindingName(InputBinding binding)
    {
        GamepadIconMap map = GetMapForCurrentDevice();
        return map?.GetName(binding.effectivePath);
    }

    private Sprite GetBindingIcon(InputBinding binding)
    {
        GamepadIconMap map = GetMapForCurrentDevice();
        return map?.GetIcon(binding.effectivePath);
    }

    private GamepadIconMap GetMapForCurrentDevice()
    {
        if (CurrentDevice is Keyboard) return gamepadMaps.Find(m => m.gamepadType == GamepadType.PC);
        if (CurrentGamepad is DualShockGamepad) return gamepadMaps.Find(m => m.gamepadType == GamepadType.PlayStation);
        if (CurrentGamepad is XInputController or XInputControllerWindows) return gamepadMaps.Find(m => m.gamepadType == GamepadType.Xbox);

        return gamepadMaps.Find(m => m.gamepadType == GamepadType.Generic);
    }
}




