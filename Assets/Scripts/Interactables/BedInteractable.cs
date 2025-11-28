using UnityEngine;

public class BedInteractable : MonoBehaviour, IInteractable
{
    public string customMessage = "Abrir puerta";

    public void Interact(GameObject interactor)
    {
        Debug.Log("Entrando a segunda simulacion");
        // Aquí puedes agregar animación, sonido, cambio de estado, etc.
    }

    public string GetInteractText()
    {
        return customMessage;
    }
}

