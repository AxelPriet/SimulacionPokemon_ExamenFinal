using UnityEngine;

public class HierbaAlta : MonoBehaviour
{
    public HierbaState currentState = HierbaState.Tranquila;
    [Header("Zona de hierba")]
    public float radioZona = 3f; // Radio de movimiento para el pokemon

    public void Simulate(float deltaTime)
    {
        switch (currentState)
        {
            case HierbaState.Tranquila:
                if (Random.value < 0.02f)
                    currentState = HierbaState.Encuentro;
                break;

            case HierbaState.Encuentro:
                currentState = HierbaState.Tranquila;
                break;
        }

        // Pequeño efecto visual (mecerse)
        float scale = 1 + Mathf.Sin(Time.time * 4f) * 0.05f;
        transform.localScale = new Vector3(scale, scale, 1);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0f, 1f, 0f, 0.2f);
        Gizmos.DrawSphere(transform.position, radioZona);
    }
}


