using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    public void Interact(GameObject interactor)
    {
        Debug.Log($"{name} fue abierta por {interactor.name}");
    }

    public string GetInteractText()
    {
        return "Abrir puerta";
    }
}
