using UnityEngine;


public class SimulationManager : MonoBehaviour
{
    [Header("Referencias")]
    public ThrowController throwController;
    public PokeballCollision2D pokeballCollision;
    public PokeballInputAdapter inputAdapter;

    [Header("Debug")]
    public bool showDebugInfo = true;

    void Awake()
    {
        Physics2D.simulationMode = SimulationMode2D.Script;
    }

    void Start()
    {

        // Unificar referencias al singleton
        if (throwController == null)
            throwController = ThrowController.Instance;

        if (pokeballCollision == null)
            pokeballCollision = Object.FindFirstObjectByType<PokeballCollision2D>();

        if (pokeballCollision != null && throwController != null)
            pokeballCollision.throwController = throwController;

        if (inputAdapter == null)
        {
            inputAdapter = Object.FindFirstObjectByType<PokeballInputAdapter>();
            if (inputAdapter == null)
            {
                GameObject adapterObj = new GameObject("PokeballInputAdapter");
                inputAdapter = adapterObj.AddComponent<PokeballInputAdapter>();
            }
        }
        inputAdapter.throwController = throwController;
        inputAdapter.useDirectInput = true;
    }

    void Update()
    {
        float dt = Time.deltaTime;

        if (throwController != null)
            throwController.SimulateStep(dt);

        Physics2D.Simulate(dt);
    }

    void OnGUI()
    {
        if (!showDebugInfo || throwController == null) return;

        GUIStyle style = new GUIStyle { fontSize = 14, normal = { textColor = Color.white } };

        int y = 10;
        int h = 20;

        GUI.Label(new Rect(10, y, 400, h), "Controles: A/D - Rotar | ESPACIO - Cargar/Lanzar", style);
    }

    void OnDestroy()
    {
        // ✅ Al destruir el SimulationManager, restaurar la física normal
        Physics2D.simulationMode = SimulationMode2D.FixedUpdate;
    }

}


