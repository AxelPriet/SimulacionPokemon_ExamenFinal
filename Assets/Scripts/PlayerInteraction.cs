using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interacción")]
    public float interactRange = 1.5f;
    public LayerMask interactableLayer;

    private IInteractable currentTarget;

    void Start()
    {
        if (InputManager.Instance != null)
            InputManager.Instance.OnInteract += HandleInteract;
    }

    void Update()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, interactRange, interactableLayer);

        if (hit != null)
        {
            IInteractable interactable = hit.GetComponent<IInteractable>();

            if (interactable != null)
            {
                currentTarget = interactable;

                Sprite icon = GetInteractIcon();
                string text = icon
                    ? interactable.GetInteractText()
                    : $"[{GetInteractKey()}] {interactable.GetInteractText()}";

                if (InteractionUIManager.Instance != null)
                    InteractionUIManager.Instance.Show(text, icon);

                return;
            }
        }

        currentTarget = null;
        if (InteractionUIManager.Instance != null)
            InteractionUIManager.Instance.Hide();
    }

    void HandleInteract()
    {
        currentTarget?.Interact(gameObject);
    }

    string GetInteractKey()
    {
        // ✅ Corrección: usar InputManager.Instance
        return InputManager.Instance != null
            ? InputManager.Instance.GetKeyName("Player/Interact") ?? "E"
            : "E";
    }

    Sprite GetInteractIcon()
    {
        // ✅ Corrección: usar InputManager.Instance
        return InputManager.Instance != null
            ? InputManager.Instance.GetKeyIcon("Player/Interact")
            : null;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}
