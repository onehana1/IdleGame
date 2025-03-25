using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreUI : MonoBehaviour
{
    public ItemSlot[] slots;
    public Transform slotPanel;

    public ItemData selectedItem;

    public static StoreUI instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        slots = GetComponentsInChildren<ItemSlot>();
        
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].index = i;
            if (slots[i].item != null)
            {
                slots[i].Set();
            }

        }
    }

    public void SelectItem(int index)
    {
        selectedItem = slots[index].item;
    }

    public void BuyItem()
    {
        if (selectedItem == null) return;
        InventoryManager.Instance.BuyItem(selectedItem);
    }


    public void OnBuyButton()
    {
        if (selectedItem == null) return;

        Debug.Log($"BuyItem{selectedItem.displayName}");
        InventoryManager.Instance.BuyItem(selectedItem);
    }
}
