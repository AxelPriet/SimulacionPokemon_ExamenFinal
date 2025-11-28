using UnityEngine;

public static class BattleInputBroker
{
    public static Vector2 uiMove;   // dirección de este frame (desde eventos o fallback)
    public static bool uiInteract;  // pulso este frame
    public static bool uiCancel;    // pulso este frame

    // Eventos desde InputManager
    public static void OnUIMove(Vector2 v)
    {
        uiMove = v;
        Debug.Log($"[BattleInputBroker] uiMove recibido: {v}");
    }

    public static void OnUIInteract()
    {
        uiInteract = true;
        Debug.Log("[BattleInputBroker] uiInteract recibido");
    }

    public static void OnUICancel()
    {
        uiCancel = true;
        Debug.Log("[BattleInputBroker] uiCancel recibido");
    }

    // Fallback teclado: solo si no llega evento de UI
    public static void UpdateFallback()
    {
        bool moved = false;

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            uiMove = Vector2.left;
            Debug.Log("[BattleInputBroker] Fallback uiMove: Left");
            moved = true;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            uiMove = Vector2.right;
            Debug.Log("[BattleInputBroker] Fallback uiMove: Right");
            moved = true;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            uiMove = Vector2.up;
            Debug.Log("[BattleInputBroker] Fallback uiMove: Up");
            moved = true;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            uiMove = Vector2.down;
            Debug.Log("[BattleInputBroker] Fallback uiMove: Down");
            moved = true;
        }

        // ✅ Si no se presionó ninguna tecla de dirección, resetear uiMove
        if (!moved)
            uiMove = Vector2.zero;

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.E))
        {
            uiInteract = true;
            Debug.Log("[BattleInputBroker] Fallback uiInteract recibido");
        }

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Backspace))
        {
            uiCancel = true;
            Debug.Log("[BattleInputBroker] Fallback uiCancel recibido");
        }
    }


    public static void ClearPulse()
    {
        uiInteract = false;
        uiCancel = false;
        // uiMove no se borra aquí para permitir navegación sostenida
    }
}




