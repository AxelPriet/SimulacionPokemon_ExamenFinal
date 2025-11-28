using UnityEngine;
using System.Collections.Generic;

public class ProjectileSpawner
{
    private GameObject prefab;
    private Transform origin;
    private List<Projectile> proyectiles;

    public ProjectileSpawner(GameObject prefab, Transform origin, List<Projectile> proyectiles)
    {
        this.prefab = prefab;
        this.origin = origin;
        this.proyectiles = proyectiles;
    }

    public void SpawnProjectile(Vector2 dir)
    {
        GameObject obj = Object.Instantiate(prefab, origin.position, Quaternion.identity);
        Projectile p = obj.GetComponent<Projectile>();
        p.direction = dir;
        p.speed = 6f;

        int roll = Random.Range(0, 4); // 0 = None, 1 = Paralysis, etc.
        p.effect = (StatusEffect)roll;

        proyectiles.Add(p);
        Debug.Log("Proyectil disparado en dirección: " + dir + " con estado: " + p.effect);
    }

    public void SpawnRadialBurst(int count)
    {
        for (int i = 0; i < count; i++)
        {
            float angle = i * Mathf.PI * 2f / count;
            Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
            SpawnProjectile(dir);
        }

        Debug.Log("Llamarada radial disparada en " + count + " direcciones.");
    }

    public void SpawnConeForward(int count)
    {
        float spread = 45f;
        Vector2 forward = Vector2.right;

        for (int i = 0; i < count; i++)
        {
            float angle = -spread / 2 + (spread / (count - 1)) * i;
            Vector2 dir = Quaternion.Euler(0, 0, angle) * forward;
            SpawnProjectile(dir.normalized);
        }

        Debug.Log("Disparo en abanico frontal.");
    }

    public void SpawnHoming()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        Vector2 dir = (player.transform.position - origin.position).normalized;
        SpawnProjectile(dir);

        Debug.Log("Disparo guiado al jugador.");
    }

    public void Simulate(float deltaTime)
    {
        for (int i = proyectiles.Count - 1; i >= 0; i--)
        {
            if (proyectiles[i] != null)
                proyectiles[i].Simulate(deltaTime);
            else
                proyectiles.RemoveAt(i);
        }
    }
}
