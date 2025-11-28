using UnityEngine;
using System.Collections;

public class Pokemons : MonoBehaviour
{
    public PokemonsState currentState = PokemonsState.Explorando;

    public float speed = 2f;
    public float changeDirTime = 2f;

    private Vector3 destination;
    private float timer;
    private HierbaAlta zonaAsignada;
    private Rigidbody2D rb;
    private bool enCombate = false;

    void Start()
    {
        // Se asegura de que los pokemon esten o busquen hierba alta para estar y moverse
        HierbaAlta[] zonas = FindObjectsByType<HierbaAlta>(FindObjectsSortMode.None);
        float minDist = Mathf.Infinity;

        foreach (var z in zonas)
        {
            float dist = Vector2.Distance(transform.position, z.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                zonaAsignada = z;
            }
        }

        if (zonaAsignada == null)
        {
            Debug.LogWarning("El pokemon no encontró una zona de hierba cercana.");
        }

        SelectNewDestination();

        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 0;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    public void Simulate(float deltaTime)
    {
        if (enCombate) return; // Se mantendra quieto si entra en combate

        timer += deltaTime;
        if (timer >= changeDirTime)
        {
            SelectNewDestination();
            timer = 0f;
        }

        MoveSmooth(deltaTime);
        UpdateColor();
    }

    void MoveSmooth(float deltaTime)
    {
        if (zonaAsignada == null) return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            destination,
            speed * deltaTime
        );

        float distanciaCentro = Vector3.Distance(transform.position, zonaAsignada.transform.position);
        if (Vector3.Distance(transform.position, destination) < 0.1f || distanciaCentro > zonaAsignada.radioZona)
        {
            SelectNewDestination();
        }
    }

    void SelectNewDestination()
    {
        if (zonaAsignada == null) return;

        Vector2 offset = Random.insideUnitCircle * (zonaAsignada.radioZona * 0.9f);
        destination = zonaAsignada.transform.position + new Vector3(offset.x, offset.y, 0);
    }

    void UpdateColor()
    {
        // Verifica que los prefabas de pokemons ya no existan
        if (this == null || gameObject == null) return;

        //Actualiza el color del pokemon segun el estado actual
        var sr = GetComponent<SpriteRenderer>();
        if (sr == null) return;

        switch (currentState)
        {
            case PokemonsState.Peleando:
                sr.color = Color.red;
                break;
            case PokemonsState.Huyendo:
                sr.color = Color.yellow;
                break;
            case PokemonsState.Explorando:
                sr.color = Color.green;
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Entrenador entrenador = collision.gameObject.GetComponent<Entrenador>();
        if (entrenador != null && !enCombate)
        {
            Debug.Log("Un Pokémon salvaje te corta el paso");
            enCombate = true;
            currentState = PokemonsState.Peleando;
            entrenador.EntrarCombate(this);
        }
    }

    public void ResultadoCombate(bool fueCapturado, bool derrotoEntrenador, bool huyo)
    {
        enCombate = false;

        if (fueCapturado)
        {
            Debug.Log("El pokemon ha sido capturado por el entrenado");
            StopAllCoroutines(); // Se asegura de que no se ejecuten co-rutinas tras ser capturados
            Destroy(gameObject);
            return;
        }

        if (derrotoEntrenador)
        {
            Debug.Log("El Pokémon ha derrotado al entrenador.");
            currentState = PokemonsState.Explorando;
        }
        else if (huyo)
        {
            Debug.Log("El Pokémon ha escapado.");
            currentState = PokemonsState.Explorando;
        }
    }

    public IEnumerator ParpadearDuranteCombate(float duracion)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null) yield break;

        float tiempo = 0f;
        while (tiempo < duracion)
        {
            if (sr == null) yield break; // Evita error si el cuando se captura el pokemon
            sr.color = Color.white;
            yield return new WaitForSeconds(0.2f);
            if (sr == null) yield break;
            sr.color = Color.red;
            yield return new WaitForSeconds(0.2f);
            tiempo += 0.4f;
        }
    }
}






