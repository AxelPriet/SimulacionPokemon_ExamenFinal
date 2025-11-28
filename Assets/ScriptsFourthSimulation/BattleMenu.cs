using UnityEngine;

public class BattleMenu : MonoBehaviour
{
    public bool active = false;

    // Índices esperados:
    // 0 = Luchar (arriba-izq), 1 = Pokémon (arriba-der),
    // 2 = Bolsa (abajo-izq), 3 = Huir (abajo-der)
    public int selectedIndex = 0;
    public RectTransform[] optionPositions;
    public RectTransform selector; // asigna el "Selector" en el mismo Canvas
    public Vector2 selectorOffset = new Vector2(-50, 0);

    public BattleSystem battleSystem;

    // banderas para evitar repeticiones
    private bool consumedX = false;
    private bool consumedY = false;

    public void Open()
    {
        active = true;
        gameObject.SetActive(true);
        selectedIndex = 0;
        consumedX = consumedY = false;
        UpdateSelector();
    }

    public void Close()
    {
        active = false;
        gameObject.SetActive(false);
    }

    public void Simulate()
    {
        if (!active) return;

        Vector2 move = BattleInputBroker.uiMove;

        // ✅ Navegación horizontal
        if (!consumedX)
        {
            if (move.x > 0.5f && selectedIndex % 2 == 0)
            {
                selectedIndex += 1; // derecha
                consumedX = true;
            }
            else if (move.x < -0.5f && selectedIndex % 2 == 1)
            {
                selectedIndex -= 1; // izquierda
                consumedX = true;
            }
        }
        // reset cuando se suelta la tecla
        if (Mathf.Abs(move.x) < 0.2f) consumedX = false;

        // ✅ Navegación vertical
        if (!consumedY)
        {
            if (move.y < -0.5f && selectedIndex < 2)
            {
                selectedIndex += 2; // abajo
                consumedY = true;
            }
            else if (move.y > 0.5f && selectedIndex >= 2)
            {
                selectedIndex -= 2; // arriba
                consumedY = true;
            }
        }
        // reset cuando se suelta la tecla
        if (Mathf.Abs(move.y) < 0.2f) consumedY = false;

        // ✅ Actualizar posición del selector
        UpdateSelector();

        // ✅ Ejecutar acción según índice actual
        if (BattleInputBroker.uiInteract)
        {
            Debug.Log("Interacción con índice: " + selectedIndex);

            switch (selectedIndex)
            {
                case 0: battleSystem.messageBox.ShowMessage("Luchar no implementado"); break;
                case 1: battleSystem.messageBox.ShowMessage("Pokémon no implementado"); break;
                case 2: battleSystem.OpenBag(); break;
                case 3: battleSystem.messageBox.ShowMessage("Huir no implementado"); break;
            }
        }
    }

    void UpdateSelector()
    {
        if (selector == null || optionPositions == null || selectedIndex < 0 || selectedIndex >= optionPositions.Length)
            return;

        RectTransform target = optionPositions[selectedIndex];

        // Asegurar que el selector esté en el mismo padre que los botones
        if (selector.parent != target.parent)
            selector.SetParent(target.parent, worldPositionStays: false);

        // Usar anchoredPosition para UI
        selector.anchoredPosition = target.anchoredPosition + selectorOffset;
    }
}






