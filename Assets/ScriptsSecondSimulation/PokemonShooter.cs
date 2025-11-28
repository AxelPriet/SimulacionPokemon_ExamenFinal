using UnityEngine;
using System.Collections.Generic;

public class PokemonShooter : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float shootInterval = 0.5f;
    public int burstCount = 20;
    public float projectileSpeed = 6f;

    private float shootTimer = 0f;
    private List<Projectile> proyectiles = new List<Projectile>();
    public enum AttackPattern { Radial, ConeForward, Homing }

    public AttackPattern currentPattern = AttackPattern.Radial;

    public void Simulate(float deltaTime)
    {
        shootTimer += deltaTime;
        if (shootTimer >= shootInterval)
        {
            shootTimer = 0f;

            currentPattern = (AttackPattern)Random.Range(0, 3);

            switch (currentPattern)
            {
                case AttackPattern.Radial: ShootRadialBurst(); break;
                case AttackPattern.ConeForward: ShootConeForward(); break;
                case AttackPattern.Homing: ShootHoming(); break;
            }
        }

        for (int i = proyectiles.Count - 1; i >= 0; i--)
        {
            if (proyectiles[i] != null)
                proyectiles[i].Simulate(deltaTime);
            else
                proyectiles.RemoveAt(i);
        }
    }



    void ShootConeForward()
    {
        Vector2 forward = Vector2.right;
        float spread = 45f;
        int count = burstCount;

        for (int i = 0; i < count; i++)
        {
            float angle = -spread / 2 + (spread / (count - 1)) * i;
            Vector2 dir = Quaternion.Euler(0, 0, angle) * forward;

            SpawnProjectile(dir);
        }
    }

    void ShootHoming()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        Vector2 dir = (player.transform.position - transform.position).normalized;
        SpawnProjectile(dir);
    }

    void ShootRadialBurst()
    {
        for (int i = 0; i < burstCount; i++)
        {
            float angle = i * Mathf.PI * 2f / burstCount; // 0 a 2π
            Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;

            Debug.Log($"Dirección radial #{i}: {dir}");
            SpawnProjectile(dir);
        }

        Debug.Log("Llamarada radial disparada en " + burstCount + " direcciones.");
    }


    void SpawnProjectile(Vector2 dir)
    {
        GameObject obj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Projectile p = obj.GetComponent<Projectile>();
        p.direction = dir;
        p.speed = projectileSpeed;
        int roll = Random.Range(0, 4); // 0 = None, 1 = Paralysis, 2 = Poison, 3 = Burn
        p.effect = (StatusEffect)roll;
        proyectiles.Add(p);
    }
}


