using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BagMenu : MonoBehaviour
{
    [System.Serializable]
    public class BerryItem
    {
        public string berryName;
        public Sprite icon;
        public StatusCondition cures;
    }

    public bool active = false;
    public int selectedIndex = 0;
    public RectTransform selector;
    public Vector2 selectorOffset = new Vector2(-50, 0);

    public GameObject berryItemTemplate;
    public Transform berryListContainer;
    public BerryItem[] berries;

    public RectTransform[] itemPositions;
    public BattleSystem battleSystem;

    private bool consumedY = false;

    public void Open()
    {
        active = true;
        gameObject.SetActive(true);
        selectedIndex = 0;
        consumedY = false;

        GenerateBerryItems();
        UpdateSelector();
    }

    public void Close()
    {
        active = false;
        gameObject.SetActive(false);
        itemPositions = null;
    }

    public void Simulate()
    {
        if (!active || itemPositions == null || itemPositions.Length == 0) return;

        Vector2 move = BattleInputBroker.uiMove;

        if (!consumedY)
        {
            if (move.y < -0.5f)
            {
                selectedIndex = Mathf.Min(selectedIndex + 1, itemPositions.Length - 1);
                consumedY = true;
            }
            else if (move.y > 0.5f)
            {
                selectedIndex = Mathf.Max(selectedIndex - 1, 0);
                consumedY = true;
            }
        }
        if (Mathf.Abs(move.y) < 0.2f) consumedY = false;

        UpdateSelector();

        if (BattleInputBroker.uiInteract)
        {
            if (selectedIndex >= 0 && selectedIndex < berries.Length)
            {
                BerryItem berry = berries[selectedIndex];
                Debug.Log("Usaste: " + berry.berryName);
                battleSystem.OnItemUsed(berry.cures);
            }
        }

        if (BattleInputBroker.uiCancel)
        {
            Close();
            battleSystem.ReturnToBattleMenu();
        }
    }

    void GenerateBerryItems()
    {
        foreach (Transform child in berryListContainer)
        {
            if (child == selector) continue; // ✅ No destruir el selector
            Destroy(child.gameObject);
        }

        itemPositions = new RectTransform[berries.Length];

        for (int i = 0; i < berries.Length; i++)
        {
            var berry = berries[i];
            GameObject go = Instantiate(berryItemTemplate, berryListContainer);
            itemPositions[i] = go.GetComponent<RectTransform>();

            go.transform.Find("Icon").GetComponent<Image>().sprite = berry.icon;
            go.transform.Find("BerryNameText").GetComponent<TextMeshProUGUI>().text = berry.berryName;
        }
    }

    void UpdateSelector()
    {
        if (selector == null || itemPositions == null || selectedIndex < 0 || selectedIndex >= itemPositions.Length)
            return;

        RectTransform target = itemPositions[selectedIndex];
        if (target == null) return;

        if (selector.parent != target.parent)
            selector.SetParent(target.parent, worldPositionStays: false);

        selector.anchoredPosition = target.anchoredPosition + selectorOffset;
        selector.gameObject.SetActive(true);

        ScrollRect scroll = berryListContainer.GetComponentInParent<ScrollRect>();
        if (scroll != null && itemPositions.Length > 1)
        {
            float normalizedY = 1f - (float)selectedIndex / (itemPositions.Length - 1);
            scroll.verticalNormalizedPosition = Mathf.Clamp01(normalizedY);
        }
    }
}










