using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulateManager : MonoBehaviour
{
    public float secondsPerIteration = 0.01f;
    private float time = 0f;

    [Header("Prefabs")]
    public GameObject pokemonPrefab;

    [Header("Entidades")]
    public List<Pokemons> pokemones = new List<Pokemons>();
    public List<Entrenador> entrenadores = new List<Entrenador>();
    public List<HierbaAlta> zonasHierba = new List<HierbaAlta>();

    void Start()
    {
        StartCoroutine(IniciarSimulacion());
   
    }

    IEnumerator IniciarSimulacion()
    {
        yield return new WaitForSeconds(0.3f); // esperar a que cargue todo

        // Encontrar zonas y entrenadores
        zonasHierba = new List<HierbaAlta>(FindObjectsByType<HierbaAlta>(FindObjectsSortMode.None));
        entrenadores = new List<Entrenador>(FindObjectsByType<Entrenador>(FindObjectsSortMode.None));

        // Crear Pokémon aleatorio
        foreach (var zona in zonasHierba)
        {
            int cantidad = Random.Range(0, 1); // cuántos Pokémon aparecen por zona
            for (int i = 0; i < cantidad; i++)
            {
                Vector2 offset = Random.insideUnitCircle * (zona.radioZona * 0.5f);
                Vector3 pos = zona.transform.position + new Vector3(offset.x, offset.y, 0);
                Instantiate(pokemonPrefab, pos, Quaternion.identity);
            }
        }

        // Esperar un momento para que aparezcan y los detecte el manager
        yield return new WaitForSeconds(0.2f);
        ActualizarListas();

        Debug.Log("Simulación iniciada con {pokemones.Count} Pokémon salvajes, {entrenadores.Count} entrenadores y {zonasHierba.Count} zonas de hierba.");
    }

    void Update()
    {
        time += Time.deltaTime;
        if (time >= secondsPerIteration)
        {
            time = 0f;
            Simulate();
        }
    }

    void Simulate()
    {
        ActualizarListas();

        foreach (var h in zonasHierba)
            if (h != null) h.Simulate(secondsPerIteration);

        foreach (var p in pokemones)
            if (p != null) p.Simulate(secondsPerIteration);

        foreach (var e in entrenadores)
            if (e != null) e.Simulate(secondsPerIteration);
    }

    void ActualizarListas()
    {
        pokemones = new List<Pokemons>(FindObjectsByType<Pokemons>(FindObjectsSortMode.None));
    }
}



