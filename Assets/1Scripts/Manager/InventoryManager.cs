using System;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEditor.UIElements;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }


    [SerializeField] private List<ItemSlotData> inventory = new List<ItemSlotData>();
    public event Action onInventoryUpdated; // �κ��� �� ������ ui�߰����ִ� �̺�Ʈ �߰�

    public void AddItem(ItemData newItem)
    {
        if (newItem == null) return;

        if (newItem.canStack)
        {
            ItemSlotData existingSlot = inventory.Find(slot => slot.item == newItem);
            if (existingSlot != null && existingSlot.quantity < newItem.maxStackAmount)
            {
                existingSlot.quantity++;
                onInventoryUpdated?.Invoke();
                Debug.Log($"{newItem.displayName} ���� ����: {existingSlot.quantity}");
                return;
            }
        }

        inventory.Add(new ItemSlotData(newItem, 1));
        onInventoryUpdated?.Invoke();
        Debug.Log($"�� ������ �߰�: {newItem.displayName}");
    }

    public void UseItem(ItemData selectedItem)
    {
        if (selectedItem == null) return;
        ItemSlotData item = inventory.Find(slot => slot.item== selectedItem);
        if(item== null) return;
        item.quantity--;
        Debug.Log($"{selectedItem.displayName} ����߼�");

        if (item.quantity <= 0)
        {
            inventory.Remove(item);
        }
        onInventoryUpdated?.Invoke();

    }


    public void ThrowItem(ItemData selectedItem, Vector3 dropPos)
    {
        if(selectedItem==null) return;
        ItemSlotData item = inventory.Find(slot => slot.item == selectedItem);

        if(item==null) return;

        item.quantity--;
        if(item.quantity <= 0) inventory.Remove(item);

        GameObject droppedItem = Instantiate(item.item.dropPrefab, dropPos, Quaternion.identity);

        onInventoryUpdated?.Invoke();

    }

    public void RemoveItem(ItemData item)
    {
        ItemSlotData slot = inventory.Find(slot => slot.item == item);
        if (slot != null)
        {
            slot.quantity--;
            if (slot.quantity <= 0)
            {
                inventory.Remove(slot);
            }

            onInventoryUpdated?.Invoke();
        }
    }

    public void SetEquippedState(ItemData item, bool equipped)
    {
        ItemSlotData slot = inventory.Find(slot => slot.item == item);
        if (slot != null)
        {
            slot.isEquipped = equipped;
            onInventoryUpdated?.Invoke();
        }
    }
    public bool IsEquipped(ItemData item)       // ���� �������� �������̳�?
    {
        ItemSlotData slot = inventory.Find(slot => slot.item == item);
        return slot != null && slot.isEquipped;
    }
    public void InvokeInventoryUpdated()    // ȣ�� �� �� �հ�.. �ٵ� �̷��� �ǳ�?
    {
        onInventoryUpdated?.Invoke();
    }

    public List<ItemSlotData> GetInventory()
    {
        return inventory;
    }


}

