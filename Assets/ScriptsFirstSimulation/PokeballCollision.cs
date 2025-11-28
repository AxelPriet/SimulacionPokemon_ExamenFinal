using UnityEngine;

public class PokeballCollision2D : MonoBehaviour
{
    [Header("Configuración")]
    public float reboundForce = 3f;
    [Range(0f, 1f)] public float captureSuccessRate = 0.7f;

    [Header("Referencias")]
    public ThrowController throwController; // Asignar en Inspector o usa singleton

    private Rigidbody2D rb;
    private bool hasCollided = false;
    private GameObject targetPokemon = null;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (throwController == null)
            throwController = ThrowController.Instance; // unificar referencia
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!col.transform.CompareTag("Pokemon") || hasCollided) return;

        hasCollided = true;
        targetPokemon = col.gameObject;

        Debug.Log("¡Colisión con Pokémon detectada! Rebote + captura inmediata...");

        if (rb != null)
        {
            Vector2 reboundDir = (transform.position - col.transform.position).normalized;
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.AddForce(reboundDir * reboundForce, ForceMode2D.Impulse);
        }

        ResolveCapture();
    }

    private void ResolveCapture()
    {
        bool success = Random.value <= captureSuccessRate;

        // 1) Primero resetear lanzamiento SIEMPRE
        if (throwController != null)
        {
            if (success) throwController.OnPokeballCaptured();
            else throwController.OnPokeballMissed();
        }

        // 2) Luego manejar el Pokémon
        if (success)
        {
            Debug.Log("✓ Pokémon capturado");
            if (targetPokemon != null) Destroy(targetPokemon);

            // 3) Finalmente, instanciar el nuevo Pokémon
            if (SimulationSceneManager.Instance != null)
                SimulationSceneManager.Instance.ReloadWithNewPokemon();
        }
        else
        {
            Debug.Log("✗ Pokémon escapó");
        }

        ResetCollisionState();
        Debug.Log("✔ Resolución de captura completada. Estado reseteado.");
    }

    private void ResetCollisionState()
    {
        hasCollided = false;
        targetPokemon = null;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.Sleep();
        }
    }

    void OnBecameInvisible()
    {
        // Si salió de cámara durante un lanzamiento y no hubo colisión, contamos como "missed"
        if (!hasCollided && throwController != null && throwController.IsLaunched())
        {
            Debug.Log("⚠️ Pokébola salió de la escena - reiniciando");
            throwController.OnPokeballMissed();
            ResetCollisionState();
        }
    }
}






