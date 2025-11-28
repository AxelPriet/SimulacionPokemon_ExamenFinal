using UnityEngine;

public class TvInteractable : MonoBehaviour, IInteractable
{
    public string customMessage = "Abrir puerta";

    public void Interact(GameObject interactor)
    {
        Debug.Log("Entrando a la primera simulación");
        // Aquí puedes agregar animación, sonido, cambio de estado, etc.
    }

    public string GetInteractText()
    {
        return customMessage;
    }
}

