using System.Collections.Generic;

public class Hit
{
    public List<StatusEffect> StatusEffects { get; set; }
    public float Damage { get; set; }
    public float CritRate { get; set; }
    public float CritMultiplier { get; set; }
    public float TargetHealthModifier { get; set; }
    public float TargetManaModifier { get; set; }
    public int ManaCost { get; set; }
    public List<string> TargetTags { get; set; }

    public Hit(List<StatusEffect> statusEffects, float damage, float critRate, float critMultiplier, float targetHealthModifier, float targetManaModifier, int manaCost, List<string> targetTags)
    {
        StatusEffects = statusEffects;
        Damage = damage;
        CritRate = critRate;
        CritMultiplier = critMultiplier;
        TargetHealthModifier = targetHealthModifier;
        TargetManaModifier = targetManaModifier;
        ManaCost = manaCost;
        TargetTags = targetTags;
    }
}