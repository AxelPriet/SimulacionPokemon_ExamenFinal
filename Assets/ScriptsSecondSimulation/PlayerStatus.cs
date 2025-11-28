using UnityEngine;
using System.Collections;

public class PlayerStatus : MonoBehaviour
{
    private SpriteRenderer sr;
    private PlayerMovement movement;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr == null)
            sr = GetComponentInChildren<SpriteRenderer>();

        movement = GetComponent<PlayerMovement>();
    }

    public void ApplyEffect(StatusEffect effect)
    {
        switch (effect)
        {
            case StatusEffect.Paralysis:
                Debug.Log("Estado aplicado: PARÁLISIS");
                StartCoroutine(ApplyTemporaryEffect(Color.yellow, 2f, true));
                break;
            case StatusEffect.Poison:
                Debug.Log("Estado aplicado: VENENO");
                StartCoroutine(ApplyTemporaryEffect(new Color(0.5f, 0f, 0.5f), 2f, false));
                break;
            case StatusEffect.Burn:
                Debug.Log("Estado aplicado: QUEMADURA");
                StartCoroutine(ApplyTemporaryEffect(Color.red, 2f, false));
                break;
            case StatusEffect.None:
                Debug.Log("El proyectil no aplicó ningún estado.");
                break;
        }
    }

    IEnumerator ApplyTemporaryEffect(Color color, float duration, bool disableMovement)
    {
        if (disableMovement && movement != null)
            movement.enabled = false;

        sr.color = color;
        yield return new WaitForSeconds(duration);
        sr.color = Color.white;

        if (disableMovement && movement != null)
            movement.enabled = true;
    }
}



