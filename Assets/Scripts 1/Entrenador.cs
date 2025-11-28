using UnityEngine;
using System.Collections;

public class Entrenador : MonoBehaviour
{
    public EntrenadorState currentState = EntrenadorState.Explorando;

    public float speed = 2.5f;
    public float maxX = 9f;
    public float maxY = 5f;

    private Vector3 destination;
    private float timer;
    private bool enCombate = false; 

    void Start()
    {
        SelectNewDestination();

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 0;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    public void Simulate(float deltaTime)
    {
        if (enCombate) return; // No se mueve mientras esta en combate

        timer += deltaTime;
        if (timer >= 2f)
        {
            SelectNewDestination();
            timer = 0f;
        }

        MoveSmooth(deltaTime);
        CheckBounds();
        UpdateColor();
    }

    void MoveSmooth(float deltaTime)
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            destination,
            speed * deltaTime
        );

        if (Vector3.Distance(transform.position, destination) < 0.1f)
        {
            SelectNewDestination();
        }
    }

    void SelectNewDestination()
    {
        Vector3 direction = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            0
        ).normalized;

        destination = transform.position + direction * Random.Range(2f, 4f);
    }

    void CheckBounds()
    {
        Vector3 pos = transform.position;

        if (pos.x > maxX || pos.x < -maxX)
        {
            pos.x = Mathf.Clamp(pos.x, -maxX, maxX);
            SelectNewDestination();
        }

        if (pos.y > maxY || pos.y < -maxY)
        {
            pos.y = Mathf.Clamp(pos.y, -maxY, maxY);
            SelectNewDestination();
        }

        transform.position = pos;
    }

    // Este método es llamado desde el Pokémon cuando ocurre una colisión
    public void EntrarCombate(Pokemons pokemon)
    {
        // No puede entrar en mas combates si ya está en uno
        if (enCombate)
        {
            Debug.Log("El entrenador ya está en combate.");
            return;
        }

        enCombate = true;
        currentState = EntrenadorState.EnCombate;

        Debug.Log("Un Pokémon te ha cortado el paso.");
        StartCoroutine(CombateConRetraso(pokemon));
    }

    private IEnumerator CombateConRetraso(Pokemons pokemon)
    {
        Debug.Log("El combate está en curso...");

        // Animaciones de parpadeo en pokemon y entrenador
        StartCoroutine(ParpadearDuranteCombate(5f));
        StartCoroutine(pokemon.ParpadearDuranteCombate(5f));

        // Tiempo de combate 5 segundos antes de obtener un resultado
        yield return new WaitForSeconds(5f);

        float resultado = Random.value;

        if (resultado < 0.5f)
        {
            Debug.Log("Has capturado al Pokémon");
            pokemon.ResultadoCombate(fueCapturado: true, derrotoEntrenador: false, huyo: false);
        }
        else if (resultado < 0.8f)
        {
            Debug.Log("El Pokémon ha escapado.");
            pokemon.ResultadoCombate(fueCapturado: false, derrotoEntrenador: false, huyo: true);
        }
        else
        {
            Debug.Log("El entrenador ha sido derrotado por el Pokémon salvaje.");
            pokemon.ResultadoCombate(fueCapturado: false, derrotoEntrenador: true, huyo: false);
        }

        // Una vez termina el combate, vuelve a explorar
        enCombate = false;
        currentState = EntrenadorState.Explorando;
    }

    private IEnumerator ParpadearDuranteCombate(float duracion)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null) yield break;

        float tiempo = 0f;
        while (tiempo < duracion)
        {
            sr.color = Color.white;
            yield return new WaitForSeconds(0.2f);
            sr.color = Color.blue;
            yield return new WaitForSeconds(0.2f);
            tiempo += 0.4f;
        }

        sr.color = Color.blue; //  vuelve al color base
    }
    void UpdateColor()
    {
        var sr = GetComponent<SpriteRenderer>();
        if (sr == null) return;

        switch (currentState)
        {
            case EntrenadorState.EnCombate:
                sr.color = Color.red;
                break;
            case EntrenadorState.Explorando:
                sr.color = Color.blue;
                break;
        }
    }
}








