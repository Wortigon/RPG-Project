using System;
using System.Diagnostics;
using System.Globalization;
using System.Xml;
using System.Xml.Serialization;

[XmlRoot("Attributes")]
public class ConsumableAttributes : ItemAttributes
{
    //public int healthRestoreAmount;

    public void Parse(string attributes)
    {
        //throw new NotImplementedException();
    }
}
