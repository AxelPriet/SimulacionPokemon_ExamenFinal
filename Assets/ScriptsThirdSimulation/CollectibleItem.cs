using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    public string itemName;
    public Sprite icon;

    private bool canCollect = false;
    private InventoryController inventory;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inventory = other.GetComponent<InventoryController>();
            canCollect = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canCollect = false;
            inventory = null;
        }
    }

    public void TryCollect()
    {
        if (canCollect && inventory != null)
        {
            inventory.AddItem(itemName, icon);
            Destroy(gameObject);
        }
    }
}


