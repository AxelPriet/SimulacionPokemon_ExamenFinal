// PokemonStatus.cs
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PokemonStatus : MonoBehaviour
{
    public StatusCondition currentStatus = StatusCondition.None;
    public SpriteRenderer spriteRenderer;

    void Reset()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Awake()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void ApplyStatus(StatusCondition status)
    {
        currentStatus = status;
        UpdateTint();
    }

    public void CureStatus()
    {
        currentStatus = StatusCondition.None;
        spriteRenderer.color = Color.white;
    }

    void UpdateTint()
    {
        switch (currentStatus)
        {
            case StatusCondition.Paralizado:
                spriteRenderer.color = Color.yellow;
                break;
            case StatusCondition.Quemado:
                spriteRenderer.color = Color.red;
                break;
            case StatusCondition.Congelado:
                spriteRenderer.color = Color.cyan;
                break;
            case StatusCondition.Envenenado:
                spriteRenderer.color = Color.green;
                break;
            case StatusCondition.Dormido:
                spriteRenderer.color = new Color(0.6f, 0.6f, 1f);
                break;
            case StatusCondition.Confundido:
                spriteRenderer.color = Color.magenta;
                break;
            default:
                spriteRenderer.color = Color.white;
                break;
        }
    }
}

