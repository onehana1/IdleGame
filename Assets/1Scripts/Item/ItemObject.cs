using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public ItemData data;

    public string GetInteractPrompt()
    {
        string str = $"{data.displayName}\n{data.description}";
        return str;
    }

    public void OnInteract()
    {
        InventoryManager.Instance.AddItem(data); 
        Destroy(gameObject);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("Item 충돌");
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.inventory.AddItem(data); 
                Debug.Log($"{data.name} added to inventory.");
                Destroy(gameObject);
            }
        }
    }
}