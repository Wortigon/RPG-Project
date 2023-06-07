using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UnityEditor;
using UnityEngine;

public static class ItemLoader {

    private static void RegisterItemFunction(string functionName, Dictionary<string, System.Action> itemFunctionDictionary, System.Action function)
    {
        if (!itemFunctionDictionary.ContainsKey(functionName))
        {
            itemFunctionDictionary.Add(functionName, function);
        }
        else
        {
            itemFunctionDictionary[functionName] = function;
        }
    }
    public static void RegisterItemFunctions(Dictionary<int, ItemData> itemDataDictionary, out Dictionary<string, System.Action> itemFunctionDictionary)
    {
        itemFunctionDictionary = new Dictionary<string, System.Action>();
        System.Reflection.MethodInfo[] methods = typeof(ItemFunctions).GetMethods(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);

        foreach (System.Reflection.MethodInfo method in methods)
        {
            RegisterItemFunction(method.Name, itemFunctionDictionary, (System.Action)System.Delegate.CreateDelegate(typeof(System.Action), method));
            //Debug.Log("Registered function" + method.Name);
        }

        foreach (ItemData itemData in itemDataDictionary.Values)
        {
            if (itemFunctionDictionary.TryGetValue(itemData.FunctionName, out System.Action function))
            {
                itemData.Use = () => function.DynamicInvoke();
            }
            else
            {
                Debug.LogError($"Function with name {itemData.FunctionName} not found in DataTable!");
            }
        }
    }
    public static void LoadItemDataFromXml(string xmlPath, out Dictionary<int, ItemData> itemDataDictionary)
    {
        itemDataDictionary = new Dictionary<int, ItemData>();

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(xmlPath);

        XmlNodeList itemNodes = xmlDoc.SelectNodes("//Items/Item");
        foreach (XmlNode itemNode in itemNodes)
        {
            int id = int.Parse(itemNode.SelectSingleNode("ID").InnerText);
            string name = itemNode.SelectSingleNode("Name").InnerText;
            string iconName = itemNode.SelectSingleNode("Icon").InnerText;

            string iconFullpath = "Assets/Resources/ItemIcons/" + iconName;
            Sprite icon = AssetDatabase.LoadAssetAtPath<Sprite>(iconFullpath);

            if (icon == null)
            {
                Debug.LogError("Failed to Load icon from " + iconFullpath + " The icon name in question was: " + iconName);
            }
            else
            {
                //Debug.Log("Icon loaded successfully");
            }

            string functionName = itemNode.SelectSingleNode("FunctionName").InnerText;
            int maxStackSize = (int)StringToFloat.Convert(itemNode.SelectSingleNode("MaxStackSize").InnerText);
            bool canHaveNBT = bool.Parse(itemNode.SelectSingleNode("CanHaveNBT").InnerText);

            //Debug.Log(id.ToString() + name + iconName + functionName + canHaveNBT.ToString());  //checking a few test values, worked fine.

            ItemType itemType = (ItemType)System.Enum.Parse(typeof(ItemType), itemNode.SelectSingleNode("Type").InnerText);

            XmlNode attributesNode = itemNode.SelectSingleNode("Attributes");

            ItemAttributes itemAttributes = null;

            if (attributesNode != null)
            {
                bool typeRecognised = true;
                switch (itemType)
                {
                    case ItemType.Weapon:
                        itemAttributes = new WeaponAttributes();
                        break;
                    case ItemType.Armor:
                        itemAttributes = new ArmorAttributes();
                        break;
                    case ItemType.Consumable:
                        itemAttributes = new ConsumableAttributes();
                        break;
                    case ItemType.QuestItem:
                        itemAttributes = new QuestItemAttributes();
                        break;
                    default:
                        typeRecognised = false;
                        break;
                }
                if (typeRecognised)
                {
                    itemAttributes.Parse(attributesNode.OuterXml);
                }
            }

            ItemData itemData = new ItemData(id, name, icon, functionName, canHaveNBT, maxStackSize, itemType, itemAttributes);
            itemDataDictionary[id] = itemData;
        }
    }
}
