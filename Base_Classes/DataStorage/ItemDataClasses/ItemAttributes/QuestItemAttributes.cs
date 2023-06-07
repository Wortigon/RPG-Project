using System;
using System.Diagnostics;
using System.Globalization;
using System.Xml;
using System.Xml.Serialization;

[XmlRoot("Attributes")]
public class QuestItemAttributes : ItemAttributes
{
    [XmlAttribute("QuestName")]
    public string questName;
    [XmlAttribute("Description")]
    public string description;

    public void Parse(string attributes)
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(attributes);
        XmlNode node;
        try
        {
            node = xmlDoc.SelectSingleNode("/Attributes/QuestName");
            if (node != null)
            {
                questName = node.InnerText;
            }
            node = xmlDoc.SelectSingleNode("/Attributes/Description");
            if (node != null)
            {
                description = node.InnerText;
            }
            //UnityEngine.Debug.Log("Quest name read correctly, and it's value is: " + questName);
            //UnityEngine.Debug.Log("Description read correctly, and it's value is: " + description);
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError(e.GetType().ToString() + e.Message);
        }
    }
}
