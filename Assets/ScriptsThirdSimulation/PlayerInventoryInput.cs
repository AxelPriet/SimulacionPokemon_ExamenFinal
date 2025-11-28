using UnityEngine;
using System.Collections;

public class PlayerInventoryInput : MonoBehaviour
{

    private IEnumerator Start()
    {
        while (InputManager.Instance == null)
            yield return null;

        InputManager.Instance.OnInteract += HandleInteract;
    }

    private void OnDisable()
    {
        if (InputManager.Instance != null)
            InputManager.Instance.OnInteract -= HandleInteract;
    }

    private void HandleInteract()
    {

        float radius = 1.2f;
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (var hit in hits)
        {
            var item = hit.GetComponent<CollectibleItem>();
            if (item != null)
            {
                item.TryCollect();
                Debug.Log("Has recogido una" + item.itemName);
                break;
            }
        }
    }
}

