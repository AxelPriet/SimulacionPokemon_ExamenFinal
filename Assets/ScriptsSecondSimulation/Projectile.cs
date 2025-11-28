using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector2 direction;
    public float speed = 6f;
    public float lifetime = 4f;
    public StatusEffect effect;

    private float age = 0f;

    public void Simulate(float deltaTime)
    {
        transform.position += (Vector3)(direction.normalized * speed * deltaTime);
        age += deltaTime;

        if (age >= lifetime)
        {
            Destroy(gameObject);
            return;
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            float dist = Vector2.Distance(transform.position, player.transform.position);
            if (dist < 0.5f)
            {
                PlayerStatus status = player.GetComponent<PlayerStatus>();
                if (status != null)
                {
                    Debug.Log("Impacto con jugador. Estado: " + effect);
                    status.ApplyEffect(effect);
                }

                Destroy(gameObject);
            }
        }
    }
}




