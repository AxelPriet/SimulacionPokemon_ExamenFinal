using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimulationSceneManager : MonoBehaviour
{
    public static SimulationSceneManager Instance { get; private set; }

    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject[] pokemonPrefabs;

    private GameObject lastSpawned;


    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject); // si manejas múltiples escenas
    }

    void Start()
    {
        ReloadWithNewPokemon();
    }

    public void ReloadWithNewPokemon()
    {
        StartCoroutine(ReloadRoutine());
    }

    IEnumerator ReloadRoutine()
    {
        yield return new WaitForSeconds(0.1f);

        if (lastSpawned != null)
            Destroy(lastSpawned);

        if (pokemonPrefabs.Length > 0 && spawnPoint != null)
        {
            int index = UnityEngine.Random.Range(0, pokemonPrefabs.Length);
            GameObject newPokemon = Instantiate(pokemonPrefabs[index], spawnPoint.position, Quaternion.identity);
            newPokemon.tag = "Pokemon";
            lastSpawned = newPokemon;
        }
        else
        {
            Debug.LogError("No hay prefabs o falta el SpawnPoint");
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "InitialScene")
            Physics2D.simulationMode = SimulationMode2D.FixedUpdate;
        else if (scene.name == "FirstSimulation")
            Physics2D.simulationMode = SimulationMode2D.Script;
    }


    public GameObject CurrentPokemon => lastSpawned;
}


