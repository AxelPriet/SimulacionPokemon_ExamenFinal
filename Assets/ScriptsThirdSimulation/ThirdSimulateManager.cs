using UnityEngine;
using System.Collections.Generic;

public class ThirdSimulateManager : MonoBehaviour
{
    public static ThirdSimulateManager Instance { get; private set; }

    private readonly List<ISimulatable> simulatableEntities = new List<ISimulatable>();

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        float dt = Time.deltaTime;
        for (int i = 0; i < simulatableEntities.Count; i++)
        {
            simulatableEntities[i].Simulate(dt);
        }
    }

    public void Register(ISimulatable entity)
    {
        if (entity != null && !simulatableEntities.Contains(entity))
            simulatableEntities.Add(entity);
    }

    public void Unregister(ISimulatable entity)
    {
        if (entity != null)
            simulatableEntities.Remove(entity);
    }
}

public interface ISimulatable
{
    void Simulate(float deltaTime);
}

