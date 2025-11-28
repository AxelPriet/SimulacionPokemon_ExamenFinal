using UnityEngine;

public class SimulationEntryMarker : MonoBehaviour
{
    [Header("Índice de simulación (0 a 4)")]
    public int simulationIndex = 0;

    void Start()
    {
        PlayerPrefs.SetInt("Simulation_" + simulationIndex, 1);
        PlayerPrefs.Save();
        Debug.Log($"✔ Simulación {simulationIndex + 1} marcada como completada.");
    }
}


