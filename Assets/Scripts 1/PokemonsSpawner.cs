using UnityEngine;

public class PokemonSpawner : MonoBehaviour
{
    [Header("Configuración de Spawner")]
    public GameObject prefabPokemon;        // Prefab del Pokémon salvaje
    public int cantidadInicial = 1;         // Cuántos aparecerán al inicio
    public float radioZona = 3f;            // Radio de aparición (por si la HierbaAlta no lo tiene)

    private HierbaAlta hierba;              // Zona de hierba asignada

    void Start()
    {
        hierba = GetComponent<HierbaAlta>();
        if (hierba == null)
        {
            Debug.LogWarning("No tiene componente HierbaAlta, se usará radio local {radioZona}");
        }

        // Crear los Pokémon al inicio
        GenerarPokemons();
    }

    void GenerarPokemons()
    {
        if (prefabPokemon == null)
        {
            Debug.LogError("No hay prefab de Pokémon asignado en el spawner.");
            return;
        }

        int cantidad = Mathf.Max(1, cantidadInicial);

        for (int i = 0; i < cantidad; i++)
        {
            Vector2 offset = Random.insideUnitCircle * (hierba != null ? hierba.radioZona : radioZona);
            Vector3 spawnPos = transform.position + new Vector3(offset.x, offset.y, 0);
            Instantiate(prefabPokemon, spawnPos, Quaternion.identity);
        }

        Debug.Log("Se han generado Pokémon salvajes.");
    }

    
}

