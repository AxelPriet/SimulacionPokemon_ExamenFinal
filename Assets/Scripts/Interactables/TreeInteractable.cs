using UnityEngine;

public class TreeInteractable : MonoBehaviour, IInteractable
{
    public string customMessage = "Abrir puerta";

    public void Interact(GameObject interactor)
    {
        Debug.Log("Entrando a la tercer simulación");
        // Aquí puedes agregar animación, sonido, cambio de estado, etc.
    }

    public string GetInteractText()
    {
        return customMessage;
    }
}

