using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Timeline.Actions.MenuPriority;

public class UIInventory : MonoBehaviour
{
    public ItemSlot[] slots;

    public GameObject inventoryWindow;
    public Transform slotPanel;
    public Transform dropPosition;

    [Header("Item Tooltip")]
    public GameObject itemTooltip; // ������ ���� �ڽ�
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescriptionText;
    public TextMeshProUGUI itemStatsText;
    public GameObject useButton;
    public GameObject equipUseButton;
    public GameObject unEquipUseButton;

    public GameObject dropButton;

    private ItemSlot selectedItem;   // �̰� ���õ� ���̤�
    private int selectedItemIndex;


    private bool isOpen = false;

    void Start()
    {
        //dropPosition = CharacterManager.Instance.Player.dropPosition;

        InventoryManager.Instance.onInventoryUpdated += UpdateUI;


        CharacterManager.Instance.Player.addItem += AddItem;

        inventoryWindow.SetActive(true);
        itemTooltip.SetActive(false);

        slots = new ItemSlot[slotPanel.childCount];

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].inventory = this;
            slots[i].Clear();  // ��� ���� �ʱ�ȭ
        }
    }

    public void Toggle()
    {
        bool isOpening = !inventoryWindow.activeSelf;
        inventoryWindow.SetActive(isOpening);

        if (!isOpening)
        {
            HideItemTooltip();
        }
    }

    public bool IsOpen()
    {
        return inventoryWindow.activeInHierarchy;
    }

    public void AddItem()
    {
        ItemData data = CharacterManager.Instance.Player.itemData;

        if (data == null) { Debug.Log("�����Ͱ� null"); return; }

        // InventoryManager�� ������ �߰�
        InventoryManager.Instance.AddItem(data);

        // UI ������Ʈ
        UpdateUI();

        CharacterManager.Instance.Player.itemData = null;
    }

    public void UpdateUI()
    {
        List<ItemSlotData> inventory = InventoryManager.Instance.GetInventory();

        // ���� UI ������Ʈ
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.Count)
            {
                slots[i].item = inventory[i].item;
                slots[i].quantity = inventory[i].quantity;
                slots[i].Set();

                slots[i].SetEquipState(inventory[i].isEquipped);
            }
            else
            {
                slots[i].Clear();
            }
        }
    }



    ItemSlot GetItemStack(ItemData data)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == data && slots[i].quantity < data.maxStackAmount)
            {
                return slots[i];
            }
        }
        return null;
    }

    ItemSlot GetEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                return slots[i];
            }
        }
        return null;
    }

    public void ShowItemTooltip(ItemSlot slot)
    {
        if (slot == null || slot.item == null)
        {
            HideItemTooltip(); // �������� ������ ������ ��Ȱ��ȭ
            return;
        }

        itemTooltip.SetActive(true);
        itemTooltip.transform.position = slot.transform.position + new Vector3(75, -75, 0); // ���� ������ ��ġ ����

        itemNameText.text = slot.item.displayName;
        itemDescriptionText.text = slot.item.description;
        itemStatsText.text = "";

        foreach (var consumable in slot.item.consumables)
        {
            itemStatsText.text += $"{consumable.type}: {consumable.value}\n";
        }
    }

    public void HideItemTooltip()
    {
        if (itemTooltip != null)
            itemTooltip.SetActive(false);

        if (itemNameText != null)
            itemNameText.text = "";

        if (itemDescriptionText != null)
            itemDescriptionText.text = "";

        if (itemStatsText != null)
            itemStatsText.text = "";
    }


    public void SelectItem(int index)
    {
        if (slots[index].item == null) return;

        // ���� �����Ѱ� ������ �װ� �ƿ����� ���ְ� �״����� �ƿ����� �ֱ�
        if (selectedItem != null)
        {
            selectedItem.SetOutline(false);
        }

        selectedItem = slots[index];
        selectedItemIndex = index;

        selectedItem.SetOutline(true);

        Debug.Log($"���õ� ������: {selectedItem.item.displayName}, Ÿ��: {selectedItem.item.type}");


        bool isEquipped = InventoryManager.Instance.IsEquipped(selectedItem.item);
        useButton.SetActive(selectedItem.item.type == ItemType.Consumable);
        equipUseButton.SetActive(!isEquipped && selectedItem.item.type == ItemType.Equipable);
        unEquipUseButton.SetActive(isEquipped && selectedItem.item.type == ItemType.Equipable);
        dropButton.SetActive(true);
    }

    public void SetSelectItemClear()
    {
        if (selectedItem != null)
        {
            Debug.Log("�ϰ� �����Ѱ� �̹� null�̴�.");
            selectedItem.SetOutline(false);
        }
        selectedItem = null;
    }

    public void OnUseButton()
    {
        if (selectedItem == null || selectedItem.item == null) return;

        if (selectedItem.item.type == ItemType.Consumable)
        {
            InventoryManager.Instance.UseItem(selectedItem.item);
            UpdateUI();
            SetSelectItemClear();
        }
    }


    public void OnThrowButton()
    {
        if (selectedItem == null || selectedItem.item == null) return;

        Vector3 dropPosition = CharacterManager.Instance.Player.transform.position + CharacterManager.Instance.Player.transform.forward * 1.5f;
        InventoryManager.Instance.ThrowItem(selectedItem.item, dropPosition);

        UpdateUI();

    }

    public void OnEquipButton()
    {
        if (selectedItem == null || selectedItem.item.type != ItemType.Equipable) return;

        CharacterManager.Instance.Player.equipment.EquipItem(selectedItem.item);    // ����
        // selectedItem =null; // �̷��� �ٷ� null�� ���ָ� ������ �ذ������ �ƿ������� �״����
        UpdateUI(); //���������ϱ� ����
        SetSelectItemClear();   // �׷��� �ƿ����α��� ���ָ� �׸���
    }
    public void OnUnEquipButton()
    {
        if (selectedItem == null || selectedItem.item.type != ItemType.Equipable) return;

        CharacterManager.Instance.Player.equipment.UnequipItem(selectedItem.item);  // ���� ����
        UpdateUI();
    }

    void RemoveSelctedItem()
    {
        selectedItem.quantity--;

        if (selectedItem.quantity <= 0)
        {
            if (slots[selectedItemIndex].equipped)
            {

            }

            selectedItem.item = null;
        }

        UpdateUI();
    }

}
