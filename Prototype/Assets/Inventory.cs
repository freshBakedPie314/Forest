using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu]
public class Inventory : ScriptableObject
{
    public List<ItemSlot> inventory = new List<ItemSlot>();
    
    public void AddItenm(ItemData _item , int _amount)
    {
        bool hasItem = false;
        foreach (ItemSlot slot in inventory)
        {
            if(slot.itemData == _item)
            {
                slot.amount += _amount;
                hasItem = true;
                break;
            }
        }

        if (!hasItem)
        {
            ItemSlot slot = new ItemSlot(_item, _amount);
            inventory.Add(slot);
        }
    }
}

[System.Serializable]
public class ItemSlot
{
    public ItemData itemData;
    public int amount;

    public ItemSlot(ItemData _itemData , int _amount)
    {
        itemData = _itemData;
        amount = _amount;
    }

    public void AddAmount(int _amount)
    {
        amount += _amount;
    }
}

