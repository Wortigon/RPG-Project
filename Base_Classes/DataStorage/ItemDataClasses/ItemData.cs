using System.Xml.Serialization;
using System.Xml;
using System;
using UnityEngine;
using System.Reflection;

[Serializable]
public class ItemData
{
    // common attributes among every item
    private readonly int id;
    private readonly string name;
    private readonly Sprite icon;
    private readonly string functionName;
    private readonly bool canHaveNBT;
    private readonly int maxStackSize;
    private System.Action use;
    private readonly ItemType itemType;

    // read-only properties
    public int Id { get { return id; } }
    public string Name { get { return name; } }
    public Sprite Icon { get { return icon; } }
    public string FunctionName { get { return functionName; } }
    public bool CanHaveNBT { get { return canHaveNBT; } }
    public int MaxStackSize { get { return maxStackSize; } }
    public ItemType ItemType { get { return itemType; } }
    
    // the only writeable property, since this one isn't set up at the object's generation.
    public System.Action Use { get { return use; } set { use = value; } }

    // type-specific attributes bundled into a single variable, still public until further notice.
    public ItemAttributes itemAttributes;

    // constructor
    public ItemData(int id, string name, Sprite icon, string functionName, bool canHaveNBT, int maxStackSize, ItemType itemType, ItemAttributes itemAttributes)
    {
        this.id = id;
        this.name = name;
        this.icon = icon;
        this.functionName = functionName;
        this.canHaveNBT = canHaveNBT;
        this.maxStackSize = maxStackSize;
        this.itemType = itemType;
        this.itemAttributes = itemAttributes;
    }
}

public enum ItemType
{
    Weapon,
    Armor,
    Consumable,
    QuestItem,
    // add more item types here
}
