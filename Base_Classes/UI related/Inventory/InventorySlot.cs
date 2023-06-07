using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public InventoryItemDraggable itemInSlot;
    public Image slotImage;

    private InventoryManager inventoryManager;

    private void Start()
    {
        inventoryManager = transform.parent.GetComponent<InventoryManager>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        try
        {
            GameObject dropped = eventData.pointerDrag;
            InventoryItemDraggable draggable = dropped.GetComponent<InventoryItemDraggable>();

            // If there is already an item in this slot, swap the items between the two slots
            if (transform.childCount > 0)
            {
                InventoryItemDraggable current = transform.GetChild(0).GetComponent<InventoryItemDraggable>();
                current.parentAfterDrag = draggable.parentAfterDrag;
                current.transform.SetParent(draggable.parentAfterDrag);
                current.transform.SetSiblingIndex(draggable.transform.GetSiblingIndex());
            }

            draggable.parentAfterDrag = transform;
            draggable.transform.SetParent(transform);
            draggable.transform.localPosition = Vector3.zero;

            InventoryManager inventoryManager = GetComponentInParent<InventoryManager>();

            if (inventoryManager != null)
            {
                inventoryManager.UpdateInventoryList();
            }
            else
            {
                Debug.LogError("Could not find InventoryManager component in parent objects.");
            }
        }
        catch(System.Exception e)
        {
            Debug.LogError("While dropping a moving item a error occured: " + e.ToString());
        }
        
    }
}
