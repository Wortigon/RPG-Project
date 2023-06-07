using TMPro;
using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    private int id;
    public int Id { get { return id; } }

    private int count;
    public int Count { get { return count; } set { count = value; } }

    [SerializeField] private TextMeshProUGUI countDisplay;

    public ItemAttributes itemAttributes;


    public InventoryItem(int id, ItemAttributes itemAttributes, int count = 1)
    {
        this.id = id;
        this.itemAttributes = itemAttributes;
        this.count = count;
        //this.countDisplay = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    public void Center()
    {
        this.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        this.transform.localScale = new Vector3(1, 1, 1);
    }

    private void RanOut()
    {
        if(Count == 0)
        {
            InventoryManager inventoryManager = GetComponentInParent<InventoryManager>();

            if (inventoryManager != null)
            {
                inventoryManager.Inventory.Remove(this);
                inventoryManager.UpdateInventoryUi();
                Destroy(this.gameObject);
            }
            else
            {
                Debug.LogError("Could not find InventoryManager component in parent objects.");
            }
            
        }
    }
    public void SetItemData(ItemData itemData, int count = 1)
    {
        id = itemData.Id;
        itemAttributes = itemData.itemAttributes;
        Count = count;
    }

    public void updateCountDisplay()
    {
        if (countDisplay != null)
        {
            Debug.Log("Counter found, update count display called. Count is at: " + Count);
            if(count > 1)
            {
                countDisplay.text = count.ToString();
            }
            else
            {
                countDisplay.text = "";
            }
        }
    }

    public System.Action Use()
    {
        return DataTable.GetItemDataById(id).Use;
    }
}