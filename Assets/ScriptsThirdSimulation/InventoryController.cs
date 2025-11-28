using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro; 

public class InventoryController : MonoBehaviour
{
    [System.Serializable]
    public class InventorySlot
    {
        public string Name;
        public Sprite Icon;
        public int Quantity;
    }

    public List<InventorySlot> inventory = new();

    [Header("UI")]
    public Transform inventoryPanel;
    public GameObject slotPrefab;
    public GameObject inventoryCanvas;

    void OnEnable()
    {
        if (InputManager.Instance != null)
            InputManager.Instance.OnToggleInventory += ToggleInventory;
    }

    void OnDisable()
    {
        if (InputManager.Instance != null)
            InputManager.Instance.OnToggleInventory -= ToggleInventory;
    }

    void Start()
    {
        if (inventoryCanvas != null)
            inventoryCanvas.SetActive(false); 
    }

    public void AddItem(string itemName, Sprite icon)
    {
        var slot = inventory.Find(s => s.Name == itemName);
        if (slot != null)
        {
            slot.Quantity++;
        }
        else
        {
            slot = new InventorySlot { Name = itemName, Icon = icon, Quantity = 1 };
            inventory.Add(slot);
        }

        
        if (inventoryCanvas != null && !inventoryCanvas.activeSelf)
            inventoryCanvas.SetActive(true);

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (inventoryPanel == null || slotPrefab == null)
        {
            Debug.LogWarning("❌ InventoryPanel o SlotPrefab no asignado");
            return;
        }

       
        foreach (Transform child in inventoryPanel)
            Destroy(child.gameObject);

       
        foreach (var slot in inventory)
        {
            GameObject newSlot = Instantiate(slotPrefab, inventoryPanel);

            var iconObj = newSlot.transform.Find("Icon");
            var nameObj = newSlot.transform.Find("Name");
            var quantityObj = newSlot.transform.Find("Quantity");

            
            var iconImg = iconObj != null ? iconObj.GetComponent<Image>() : null;
            if (iconImg != null)
                iconImg.sprite = slot.Icon;
            
                

           
            var nameTMP = nameObj != null ? nameObj.GetComponent<TextMeshProUGUI>() : null;
            if (nameTMP != null)
                nameTMP.text = slot.Name;
      

            
            var quantityTMP = quantityObj != null ? quantityObj.GetComponent<TextMeshProUGUI>() : null;
            if (quantityTMP != null)
                quantityTMP.text = "x" + slot.Quantity;
           
        }
    }

    public void ToggleInventory()
    {
        

        if (inventoryCanvas == null)
        {
            return;
        }

   
    }



    public void ClearInventory()
    {
        inventory.Clear();
        UpdateUI();
    }
}





