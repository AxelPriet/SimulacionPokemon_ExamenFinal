using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleMenuUI : MonoBehaviour
{
    public RectTransform selector;

    public Button[] buttons;
    private int index = 0;

    private Vector2[] selectorPositions;

    void Start()
    {
        // Guardamos las posiciones de cada botón para mover el selector
        selectorPositions = new Vector2[buttons.Length];

        for (int i = 0; i < buttons.Length; i++)
        {
            selectorPositions[i] = new Vector2(
                selector.anchoredPosition.x,
                buttons[i].GetComponent<RectTransform>().anchoredPosition.y
            );
        }

        MoveSelector();
    }

    public void Simulate()
    {
        Vector2 move = BattleInputBroker.uiMove;

        if (move.y > 0.5f)
        {
            index = Mathf.Max(0, index - 1);
            MoveSelector();
        }

        if (move.y < -0.5f)
        {
            index = Mathf.Min(buttons.Length - 1, index + 1);
            MoveSelector();
        }

        if (BattleInputBroker.uiInteract)
        {
            buttons[index].onClick.Invoke();
        }
    }

    void MoveSelector()
    {
        selector.anchoredPosition = selectorPositions[index];
    }
}

