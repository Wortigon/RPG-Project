using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.UI;

public class TestButtons : UIContent
{

    private InventoryManager IM;
    public GameObject iiprefab;

    public override void Initialize()
    {
        this.transform.localPosition = new Vector3(0, -20, 0);
        this.transform.localScale = Vector3.one;
    }

    public void Awake()
    {
        
        IM = MenuController.Instance.uiElements["Inventory"].Content.GetComponent<InventoryManager>();
    }

    public void addItem(int i)
    {

        GameObject itemToAdd = Instantiate(iiprefab);
        InventoryItem tmp = itemToAdd.GetComponent<InventoryItem>();
        tmp.SetItemData(DataTable.GetItemDataById(i), 1);
        tmp.itemAttributes = DataTable.GetItemDataById(i).itemAttributes;
        if(IM != null)
        {
            if(tmp != null)
            {
                IM.AddItemToInventory(tmp);
                Debug.Log("Added item with id of " + tmp.Id + " to inventory.");
            }
            else
            {
                Debug.LogError("Item is null");
            }
        }
        else
        {
            Debug.LogError("InventoryManager not found");
        }
        Destroy(tmp.gameObject);
        
    }

    public void addSword()
    {
        int i = 1;
        addItem(i);
    }
    public void addAxe()
    {
        int i = 5;
        addItem(i);
    }
    public void addBow()
    {
        int i = 6;
        addItem(i);
    }
    public void addGun()
    {
        int i = 7;
        addItem(i);
    }
    public void addSpear()
    {
        int i = 4;
        addItem(i);
    }

    public override Vector2 GetSize()
    {
        Vector2 size = new Vector2();
        size.x = this.GetComponent<RectTransform>().rect.width;
        size.y = this.GetComponent<RectTransform>().rect.height;
        return size;
    }
}
