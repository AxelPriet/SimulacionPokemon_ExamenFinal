using UnityEngine;
using System.Collections.Generic;


public class SimulateManager5 : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform spawnOrigin;
    public float shootInterval = 1.5f;
    public int burstCount = 8;

    private float shootTimer = 0f;
    private List<Projectile> proyectiles = new List<Projectile>();
    private ProjectileSpawner spawner;

    public float secondsPerIteration5 = 0.01f;
    private float time5 = 0f;
    public int simulationIndex = 0;


    void Start()
    {
        spawner = new ProjectileSpawner(projectilePrefab, spawnOrigin, proyectiles);
        
    }

    void Update()
    {
        time5 += Time.deltaTime;
        if (time5 < secondsPerIteration5) return;

        float deltaTime = time5;
        time5 = 0f;

        shootTimer += deltaTime;
        if (shootTimer >= shootInterval)
        {
            shootTimer = 0f;

            AttackPattern pattern = (AttackPattern)Random.Range(0, 3);
            switch (pattern)
            {
                case AttackPattern.Radial:
                    spawner.SpawnRadialBurst(burstCount);
                    break;
                case AttackPattern.ConeForward:
                    spawner.SpawnConeForward(burstCount);
                    break;
                case AttackPattern.Homing:
                    spawner.SpawnHoming();
                    break;
            }
        }

        spawner.Simulate(deltaTime);
    }
}
