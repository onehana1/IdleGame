using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EquipSlotType
{
    Head, 
    Body, 
    Weapon
}

public class PlayerEquipment : MonoBehaviour
{
    public event Action <PlayerEquipment> OnPlayerEquipment;

    public ItemData equipHead;
    public ItemData equipBody;
    public ItemData equipWeapon;

    public GameObject currentWeaponObject;
    private GameObject currentHeadObject;
    private GameObject currentBodyObject;

    public Transform headPos;
    public Transform bodyPos;
    public Transform weaponPos;


    public void EquipItem(ItemData item)
    {
        if (item == null || item.type != ItemType.Equipable) return;

        // ���� ������ �������� �����ϰ� ���ο� ������ ����
        switch (item.equipSlotType)
        {
            case EquipSlotType.Head:
                UnequipItem(equipHead);
                equipHead = item;
                EquipObject(ref currentHeadObject, headPos, item.equipPrefabs);
                break;
            case EquipSlotType.Body:
                UnequipItem(equipBody);
                equipBody = item;
                EquipObject(ref currentBodyObject, bodyPos, item.equipPrefabs);
                break;
            case EquipSlotType.Weapon:
                UnequipItem(equipWeapon);
                equipWeapon = item;
                EquipObject(ref currentWeaponObject, weaponPos, item.equipPrefabs);
                break;
        }

        InventoryManager.Instance.SetEquippedState(item, true);
        ApplyEquipmentStats(item, true);

        FindObjectOfType<UIEquipment>()?.EquipItem(new ItemSlotData(item, 1));
    }

    public void UnequipItem(ItemData item)
    {
        if (item == null) return;

        // ������ ������ ����
        switch (item.equipSlotType)
        {
            case EquipSlotType.Head:
                RemoveObject(ref currentHeadObject);
                equipHead = null;
                break;
            case EquipSlotType.Body:
                RemoveObject(ref currentBodyObject);
                equipBody = null;
                break;
            case EquipSlotType.Weapon:
                RemoveObject(ref currentWeaponObject);
                equipWeapon = null;
                break;
        }
        InventoryManager.Instance.SetEquippedState(item, false);
        ApplyEquipmentStats(item, false);
        FindObjectOfType<UIEquipment>()?.UnequipItem(item.equipSlotType);
    }

    private void EquipObject(ref GameObject currentObject, Transform parentPos, GameObject prefab)
    {
        if (prefab == null) return;

        // ���� ������ ������ ����
        RemoveObject(ref currentObject);

        // ���ο� ��� ���� �� ����
        currentObject = Instantiate(prefab, parentPos.position, parentPos.rotation);
        currentObject.transform.SetParent(parentPos); 
        currentObject.transform.localPosition = Vector3.zero; 
        currentObject.transform.localRotation = Quaternion.identity;
    }

    private void RemoveObject(ref GameObject currentObject)
    {
        if (currentObject != null)
        {
            Destroy(currentObject);
            currentObject = null;
        }
    }

    public float GetCurrentAttackRate()
    {
        if (equipWeapon == null) return 1.0f; 

        EquipTool equipTool = currentWeaponObject?.GetComponent<EquipTool>();
        return equipTool != null ? equipTool.attackRate : 1.0f;
    }

    public float GetCurrentNecessaryStamina()
    {
        if (equipWeapon == null) return 1.0f;

        EquipTool equipTool = currentWeaponObject?.GetComponent<EquipTool>();
        return equipTool != null ? equipTool.useStamina : 1.0f;
    }


    private void ApplyEquipmentStats(ItemData item, bool isEquipped)
    {
      
    }


}



