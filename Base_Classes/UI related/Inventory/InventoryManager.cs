using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class InventoryManager : UIContent
{
    public GameObject inventorySlotPrefab;
    public GameObject inventoryItemPrefab;

    private int size;

    private List<InventoryItem> _inventory;

    public List<InventoryItem> Inventory
    {
        get
        {
            if (_inventory == null)
            {
                _inventory = DataTable.Instance.inventory;
            }
            if (_inventory.Count < size)
            {
                for (int i = _inventory.Count; i < size; i++)
                {
                    _inventory.Add(null);
                }
            }
            return _inventory;
        }
    }

    public override void Initialize()
    {
        this.transform.localPosition = new Vector3(0, -50, 0);
        this.transform.localScale= Vector3.one;
        size = DataTable.Instance.inventorySize;
        // Create inventory slots
        for (int i = 0; i < size; i++)
        {
            GameObject currentSlot = Instantiate(inventorySlotPrefab, transform);

            InventoryItem inventoryItem = Inventory[i];
            if (inventoryItem != null)
            {
                AddAsNewItem(inventoryItem, i);
                Debug.Log("Added item(s) to inventory on setup: " + DataTable.GetItemDataById(inventoryItem.Id).Name + " * " + inventoryItem.Count);
                //inventoryItem.transform.SetParent(currentSlot.transform);
            }
        }
    }

    public override Vector2 GetSize()
    {
        Vector2 size = new Vector2();
        size.x = this.GetComponent<GridLayoutGroup>().constraintCount * (this.GetComponent<GridLayoutGroup>().cellSize.x + this.GetComponent<GridLayoutGroup>().spacing.x);
        int rowcount = Inventory.Count / this.GetComponent<GridLayoutGroup>().constraintCount;
        size.y = rowcount * this.GetComponent<GridLayoutGroup>().cellSize.y;
        return size;
    }
    public List<int> FindIndexesOfItemInInventory(InventoryItem item)
    {
        List<int> indexes = new List<int>();

        for (int i = 0; i < Inventory.Count; i++)
        {
            if (Inventory[i] != null && Inventory[i].Id == item.Id)
            {
                indexes.Add(i);
            }
        }

        return indexes;
    }

    public int LookForFirstEmptySlot()
    {
        for(int i = 0; i<size; i++)
        {
            if (Inventory[i] == null)
            {
                return i;
            }
        }
        return -1;
    }

    public void AddAsNewItem(InventoryItem newItem, int idx = -1)
    {
        if (idx == -1 || Inventory[idx] != null)
        {
            idx = LookForFirstEmptySlot();
        }
        if (idx >= 0)
        {
            GameObject item = Instantiate(inventoryItemPrefab, transform.GetChild(idx));
            InventoryItem iitem = item.GetComponent<InventoryItem>();
            iitem.SetItemData(DataTable.GetItemDataById(newItem.Id),  newItem.Count);
            iitem.itemAttributes = newItem.itemAttributes;  //overwrite default, in case it has NBT

            item.transform.localPosition = new Vector3(0, -50, 0);
            item.transform.localScale= Vector3.one;

            //set up the sprite for the item
            if(DataTable.GetItemDataById(newItem.Id) != null)
            {
                if (DataTable.GetItemDataById(newItem.Id).Icon != null)
                {
                    if (iitem.GetComponent<Image>() != null)
                    {
                        iitem.GetComponent<Image>().sprite = DataTable.GetItemDataById(newItem.Id).Icon;
                    }
                    else
                    {
                        Debug.LogError("Failed to find Image component of object");
                    }
                }
                else
                {
                    Debug.LogError("Failed to find icon");
                }
            }
            else
            {
                Debug.LogError("Datatable entry not found");
            }
            Inventory[idx] = iitem;
            UpdateInventoryUi();
        }
    }

    public void UpdateInventoryUi()
    {
        for(int i = 0; i<size; i++)
        {
            if (Inventory[i]!= null)
            {
                Inventory[i].transform.SetParent(transform.GetChild(i));
                transform.GetChild(i).GetComponent<InventorySlot>().itemInSlot = Inventory[i].GetComponent<InventoryItemDraggable>();
                transform.GetChild(i).GetComponent<InventorySlot>().itemInSlot.GetComponent<InventoryItem>().updateCountDisplay();
            }
        }
    }

    public void UpdateInventoryList()
    {
        for(int i = 0; i<size; i++)
        {
            if(transform.GetChild(i).childCount > 0)
            {
                Inventory[i] = transform.GetChild(i).GetChild(0).GetComponent<InventoryItem>();
            }
            else
            {
                Inventory[i] = null;
            }
        }
    }

    public void AddItemToInventory(InventoryItem newItem)
    {
        ItemData newItemData = DataTable.GetItemDataById(newItem.Id);
        if(newItemData != null)
        {
            if(newItemData.CanHaveNBT || newItemData.MaxStackSize == 1)
            {
                AddAsNewItem(newItem);
            }
            else
            {
                List<int> indexes = FindIndexesOfItemInInventory(newItem);
                if(indexes.Count == 0)
                {
                    AddAsNewItem(newItem);
                }
                else
                {
                    foreach(int i in indexes)
                    {
                        if(newItem.Count > 0)
                        {
                            if ((Inventory[i].Count < newItemData.MaxStackSize) && (Inventory[i].itemAttributes == newItem.itemAttributes))
                            {
                                if (newItemData.MaxStackSize - Inventory[i].Count > newItem.Count)
                                {
                                    Inventory[i].Count += newItem.Count;
                                    newItem.Count = 0;
                                }
                                else
                                {
                                    newItem.Count -= (newItemData.MaxStackSize - Inventory[i].Count);
                                    Inventory[i].Count = newItemData.MaxStackSize;
                                }
                            }
                        }
                    }
                    if(newItem.Count > 0)
                    {
                        AddAsNewItem(newItem);
                    }
                }
            }
            UpdateInventoryUi();
        }
        else
        {
            throw new NotImplementedException("Item is not present in the item database! What even is this?");
        }
    }
}
