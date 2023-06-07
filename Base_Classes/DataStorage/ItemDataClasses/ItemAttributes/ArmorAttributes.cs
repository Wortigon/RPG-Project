﻿using System;
using System.Diagnostics;
using System.Globalization;
using System.Xml;
using System.Xml.Serialization;

[XmlRoot("Attributes")]
public class ArmorAttributes : ItemAttributes
{
    [XmlAttribute("DefenseModifier")]
    public float defenseModifier;
    [XmlAttribute("MaxDurability")]
    public float maxDurability;
    [XmlAttribute("Durability")]
    public float durability;
    

    public void Parse(string attributes)
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(attributes);
        XmlNode node;
        try
        {
            node = xmlDoc.SelectSingleNode("/Attributes/DefenseModifier");
            if (node != null)
            {
                defenseModifier = StringToFloat.Convert(node.InnerText);
            }
            node = xmlDoc.SelectSingleNode("/Attributes/MaxDurability");
            if (node != null)
            {
                maxDurability = StringToFloat.Convert(node.InnerText);
            }
            node = xmlDoc.SelectSingleNode("/Attributes/Durability");
            if (node != null)
            {
                durability = StringToFloat.Convert(node.InnerText);
            }
            //UnityEngine.Debug.Log("Defense Modifier read correctly. It's value is: " + defenseModifier.ToString());
            //UnityEngine.Debug.Log("MaxDurability read correctly. It's value is: " + maxDurability.ToString());
            //UnityEngine.Debug.Log("Durability read correctly. It's value is: " + durability.ToString());
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError(e.GetType().ToString() + e.Message);
        }
    }
}

