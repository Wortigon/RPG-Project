using System.Collections.Generic;
using UnityEngine;

public class DamageAreaTester : MonoBehaviour
{
    public List<int> effectIds = new List<int>();
    public List<string> tagsToHit = new List<string>();
    public float damage = 10.0f;
    public float critRate = 0.5f;
    public float critMultiplier = 2.0f;
    public float targetHealthModifier = 1.0f;
    public float targetManaModifier = 0.0f;


    private void Start()
    {
        List<StatusEffect> effects = new List<StatusEffect>();
        foreach (int id in effectIds)
        {
            //StatusEffectData effectData = DataTable.Instance.GetStatusEffectData(id);
            StatusEffect effect = new StatusEffect(id, 10, 2);
            effects.Add(effect);
        }
        Hit testHit = new Hit(effects, damage, critRate, critMultiplier, targetHealthModifier, targetManaModifier, 0, tagsToHit);
        DamageArea damageArea = GetComponent<DamageArea>();
        damageArea.Initialize(testHit);
    }
}