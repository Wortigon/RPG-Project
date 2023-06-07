using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEditor;
using UnityEngine;

public class DataTable : Singleton<DataTable>
{
    private string itemDatabaseXmlPath = "/Base_Classes/DataStorage/ItemData.xml";
    private static Dictionary<int, ItemData> itemDataDictionary = new Dictionary<int, ItemData>();
    private static Dictionary<string, System.Action> itemFunctionDictionary = new Dictionary<string, System.Action>();

    private string statusEffectDatabaseXmlPath = "/Base_Classes/DataStorage/StatusEffectData.xml";
    private static Dictionary<int, StatusEffectData> statusEffectDataDictionary = new Dictionary<int, StatusEffectData>();
    private static Dictionary<string, Func<ICombatable, IEnumerator>> statusEffectEffectCoroutineDictionary = new Dictionary<string, Func<ICombatable, IEnumerator>>();
    private static Dictionary<string, Action<ICombatable>> statusEffectRemoveEffectFunctionDictionary = new Dictionary<string, Action<ICombatable>>();

    private bool isInitialized = false;

    public int inventorySize = 20;
    public List<InventoryItem> inventory = new List<InventoryItem>();

    public PlayerCharacter player;

    [RuntimeInitializeOnLoadMethod]
    protected override void Awake()
    {
        base.Awake();
        itemDatabaseXmlPath = Application.dataPath + itemDatabaseXmlPath;
        statusEffectDatabaseXmlPath = Application.dataPath + statusEffectDatabaseXmlPath;

        if (!isInitialized)
        {
            LoadItems();
            LoadStatusEffects();
            isInitialized = true;
        }
        //Debug.Log("Is initialized: " + isInitialized.ToString());
    }

    private void LoadItems()
    {
        ItemLoader.LoadItemDataFromXml(itemDatabaseXmlPath, out itemDataDictionary);
        //Debug.Log("Number of items registered: " + itemDataDictionary.Keys.Count);
        ItemLoader.RegisterItemFunctions(itemDataDictionary, out itemFunctionDictionary);
        //Debug.Log("Number of functions registered: " + itemFunctionDictionary.Keys.Count);
    }

    private void LoadStatusEffects()
    {
        StatusEffectLoader.LoadStatusEffectDataFromXml(statusEffectDatabaseXmlPath, out statusEffectDataDictionary);
        //Debug.Log("Number of Status Effects registered: " + statusEffectDataDictionary.Count);
        //StatusEffectLoader.RegisterStatusEffectFunctions(statusEffectDataDictionary, out statusEffectRemoveEffectFunctionDictionary, out statusEffectEffectCoroutineDictionary );
        //Debug.Log("Number of Status Effect Effects and removals registered: " + statusEffectEffectCoroutineDictionary.Count + ", " + statusEffectRemoveEffectFunctionDictionary.Count);
    }

    public static ItemData GetItemDataById(int itemId)
    {
        if (itemDataDictionary.TryGetValue(itemId, out ItemData itemData))
        {
            return itemData;
        }

        Debug.LogError($"Item with ID {itemId} not found in DataTable!");
        return null;
    }
    public static System.Action GetItemFunctionByName(string functionName)
    {
        if (itemFunctionDictionary.TryGetValue(functionName, out System.Action function))
        {
            return function;
        }

        Debug.LogError($"Function with name {functionName} not found in DataTable!");
        return null;
    }
    

    private void OnDestroy()
    {
        //Debug.Log("DataTable is destroyed.");
    }

    public StatusEffectData GetStatusEffectData(int id) 
    {
        return statusEffectDataDictionary[id]; 
    }

}
