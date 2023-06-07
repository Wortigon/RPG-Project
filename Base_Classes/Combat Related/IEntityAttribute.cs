using System;
using System.Diagnostics;
using UnityEngine;

public class IEntityAttribute
{
    private float baseValue;
    private float preFlatMultiplicativeBonus = 0;
    private float additiveBonus = 0;
    private float postFlatMultiplicativeBonus = 0;
    private float maxValue;
    private float minValue;

    public IEntityAttribute(float _base, float _maxValue, float _minValue = 0, float _preFlatMultiplicativeBonus = 0, float _additiveBonus = 0, float _postFlatMultiplicativeBonus = 0)
    {
        baseValue = _base;
        maxValue = _maxValue;
        minValue = _minValue;
        preFlatMultiplicativeBonus = _preFlatMultiplicativeBonus;
        additiveBonus = _additiveBonus;
        postFlatMultiplicativeBonus = _postFlatMultiplicativeBonus;
        TotalValueFunction = CalculateTotalValue;
    }

    public float BaseValue { get { return baseValue; } set { baseValue = Math.Max(minValue, Math.Min(value, maxValue)); } }
    public float PreFlatMultiplicativeBonus { get { return preFlatMultiplicativeBonus; } set { preFlatMultiplicativeBonus = value; } }
    public float AdditiveBonus { get { return additiveBonus; } set { additiveBonus = value; } }
    public float PostFlatMultiplicativeBonus { get { return postFlatMultiplicativeBonus; } set { postFlatMultiplicativeBonus = value; } }
    public float MinValue { get { return minValue; } set { minValue = value; } }
    public float MaxValue { get { return maxValue; } set { maxValue = value; } }

    public Func<float, float, float, float, float> TotalValueFunction { get; set; }

    public float TotalValue { get { return Math.Max(minValue, Math.Min(TotalValueFunction(baseValue, preFlatMultiplicativeBonus, additiveBonus, postFlatMultiplicativeBonus), maxValue)); } }

    public static implicit operator float(IEntityAttribute obj)
    {
        if (obj == null)
        {
            // Handle null case here
            UnityEngine.Debug.LogError("Implicit IEntityAttribut to float conversion failed.");
            return 0f; // Or any default value you prefer
        }

        return obj.TotalValue;
    }

    private static float CalculateTotalValue(float baseValue, float preFlatMultiplicativeBonus, float additiveBonus, float postFlatMultiplicativeBonus)
    {
        return (baseValue * (1.0f + preFlatMultiplicativeBonus) + additiveBonus) * (1.0f + postFlatMultiplicativeBonus);
    }
}

    /*

    //how to assign a custom calculation (f.e.: adding an extra flat bonus variable based on the total of the original function):
    
    //create an attribute
    IEntityAttribute attribute = new IEntityAttribute(10, 100, 0, 0, 0, 0);

    //alter the attribute'scalculation format.
    attribute.TotalValueFunction = (baseValue, preFlatMultiplicativeBonus, additiveBonus, postFlatMultiplicativeBonus) =>
    {
        // Calculate the total value using the class variables
        float totalValue = (baseValue * (1.0f + preFlatMultiplicativeBonus) + additiveBonus) * (1.0f + postFlatMultiplicativeBonus);

        // Add an additional bonus based on the current total value
        if (totalValue < 50)
        {
            totalValue += 10;
        }
        else
        {
            totalValue += 20;
        }

        // Return the final total value
        return totalValue;
    };

    */

