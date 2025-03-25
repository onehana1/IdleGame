using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ItemData item;

    public UIInventory inventory;
    public Button button;
    public Image icon;
    public TextMeshProUGUI quantityText;
    private Outline outline;
    public GameObject equipIndicator;   // ������ ǥ��

    public int index;
    public bool equipped;
    public int quantity;

    private void Awake()
    {
        outline = GetComponent<Outline>();
    }

    private void OnEnable()
    {
        outline.enabled = equipped;
    }

    public void SetOutline(bool state)
    {
        if (outline != null)
        {
            outline.enabled = state;
        }
    }


    public void Setup(ItemData newItem)
    {
        item = newItem;
        if(newItem != null)
        {
            icon.sprite = newItem.icon;
            icon.enabled = true;
        }
        else{
            item = null;
            icon.sprite = null;
            icon.enabled = false;
        }
    }
    public void Set()
    {
        if (item == null)
        {
            return; 
        }

        icon.gameObject.SetActive(true);
        icon.sprite = item.icon;

        if (outline != null)
        {
            outline.enabled = equipped;
        }

        if (quantityText == null)return;

        quantityText.text = quantity > 1 ? quantity.ToString() : string.Empty;

        equipIndicator.SetActive(equipped);

    }
    public void SetEquipState(bool isEquipped)
    {
        equipped = isEquipped;
        equipIndicator.SetActive(isEquipped);
    }
    public void Clear()
    {
        item = null;
        icon.sprite = null;
        // icon.gameObject.SetActive(false);
        if(quantityText == null)return;
        quantityText.text = string.Empty;
        equipIndicator.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (inventory == null || item == null)
        {
            inventory?.HideItemTooltip();
            return;
        }

        inventory.ShowItemTooltip(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (inventory != null)
        {
            inventory.HideItemTooltip();
        }
    }
    public void OnClickButton() // ��ư Ŭ��
    {
        if (inventory == null)
        {
            return;
        }

        if (item == null)
        {
            return;
        }

        inventory.SelectItem(index);
    }

    public void onClickButton2()
    {
        if(inventory == null)
        {
            return;
        }

        StoreUI.instance.SelectItem(index);
    }
}

