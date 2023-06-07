using System.Collections;
using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEditor;
using System.Reflection;

public static class StatusEffectLoader
{
    public static void RegisterStatusEffectFunctions(
        Dictionary<int, StatusEffectData> statusEffectDataDictionary, 
        out Dictionary<string, Action<ICombatable>> statusEffectRemoveEffectDictionary, 
        out Dictionary<string, Func<ICombatable, IEnumerator>> statusEffectCoroutineDictionary
    )
    {
        statusEffectRemoveEffectDictionary = new Dictionary<string, System.Action<ICombatable>>();
        statusEffectCoroutineDictionary = new Dictionary<string, Func<ICombatable, IEnumerator>>();

        MethodInfo[] actionMethods = typeof(StatusEffectActions).GetMethods(BindingFlags.Static | BindingFlags.Public);
        MethodInfo[] coroutineMethods = typeof(StatusEffectCoroutines).GetMethods(BindingFlags.Static | BindingFlags.Public);

        foreach (MethodInfo method in actionMethods)
        {
            string functionName = method.Name;
            Action<ICombatable> function = (Action<ICombatable>)Delegate.CreateDelegate(typeof(Action<ICombatable>), method);

            if (!statusEffectRemoveEffectDictionary.ContainsKey(functionName))
            {
                statusEffectRemoveEffectDictionary.Add(functionName, function);
            }
            else
            {
                statusEffectRemoveEffectDictionary[functionName] = function;
            }
        }

        foreach (MethodInfo method in coroutineMethods)
        {
            string functionName = method.Name;
            Func<ICombatable, IEnumerator> function = (Func<ICombatable, IEnumerator>)Delegate.CreateDelegate(typeof(Func<ICombatable, IEnumerator>), method);

            if (!statusEffectCoroutineDictionary.ContainsKey(functionName))
            {
                statusEffectCoroutineDictionary.Add(functionName, function);
            }
            else
            {
                statusEffectCoroutineDictionary[functionName] = function;
            }
        }

        foreach (StatusEffectData statusEffectData in statusEffectDataDictionary.Values)
        {
            if (statusEffectRemoveEffectDictionary.TryGetValue(statusEffectData.RemoveEffectName, out Action<ICombatable> actionFunction))
            {
                Debug.Log("assigned action function");
                //statusEffectData.RemoveEffect = actionFunction;
            }
            else
            {
                Debug.LogError($"Function with name {statusEffectData.RemoveEffectName} not found in StatusEffectLoader!");
            }

            if (statusEffectCoroutineDictionary.TryGetValue(statusEffectData.EffectCoroutineName, out Func<ICombatable, IEnumerator> coroutineFunction))
            {
                Debug.Log("assigned remove function");
                //statusEffectData.EffectCoroutine = coroutineFunction;
            }
            else
            {
                Debug.LogError($"Function with name {statusEffectData.EffectCoroutineName} not found in StatusEffectLoader!");
            }
        }
    }




    public static void LoadStatusEffectDataFromXml(string xmlPath, out Dictionary<int, StatusEffectData> statusEffectDataDictionary)
    {
        statusEffectDataDictionary = new Dictionary<int, StatusEffectData>();

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(xmlPath);

        XmlNodeList effectNodes = xmlDoc.SelectNodes("//Effects/Effect");
        foreach (XmlNode effectNode in effectNodes)
        {
            int effectId = int.Parse(effectNode.SelectSingleNode("ID").InnerText);
            string effectName = effectNode.SelectSingleNode("Name").InnerText;
            bool stacks = bool.Parse(effectNode.SelectSingleNode("Stacks").InnerText);
            int maxStackSize = int.Parse(effectNode.SelectSingleNode("MaxStackSize").InnerText);
            bool refreshes = bool.Parse(effectNode.SelectSingleNode("Refreshes").InnerText);
            bool showUpInStatusBar = bool.Parse(effectNode.SelectSingleNode("ShowUpInStatusBar").InnerText);
            int priority = int.Parse(effectNode.SelectSingleNode("Priority").InnerText);

            List<int> erasePriorityLevels = new List<int>();
            XmlNode erasePriorityNode = effectNode.SelectSingleNode("ErasePriorityLevels");
            if (erasePriorityNode != null)
            {
                XmlNodeList priorityNodes = erasePriorityNode.SelectNodes("Priority");
                foreach (XmlNode priorityNode in priorityNodes)
                {
                    int erasePriorityLevel = int.Parse(priorityNode.InnerText);
                    erasePriorityLevels.Add(erasePriorityLevel);
                }
            }

            string iconName = effectNode.SelectSingleNode("Icon").InnerText;
            string iconPath = "StatusEffectIcons/" + iconName; // Path relative to the "Resources" folder
            Sprite icon = Resources.Load<Sprite>(iconPath);

            if (icon == null)
            {
                Debug.LogError("Failed to load icon: " + iconName);
            }

            string description = effectNode.SelectSingleNode("Description").InnerText;
            string effectCoroutineName = effectNode.SelectSingleNode("EffectCoroutine").InnerText;
            string removeEffectName = effectNode.SelectSingleNode("RemoveEffect").InnerText;

            StatusEffectData statusEffectData = new StatusEffectData(effectId, effectName, stacks, maxStackSize, refreshes, showUpInStatusBar, priority, erasePriorityLevels, icon, description, effectCoroutineName, removeEffectName);
            statusEffectDataDictionary[effectId] = statusEffectData;
        }
    }
}
