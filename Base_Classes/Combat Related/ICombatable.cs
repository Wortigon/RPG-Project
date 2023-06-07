using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public abstract class ICombatable : MonoBehaviour
{
    // Private fields
    private IEntityAttribute maxHealth;
    private IEntityAttribute maxMana;
    private IEntityAttribute power;
    private IEntityAttribute critRate;
    private IEntityAttribute critMultiplier;
    private IEntityAttribute attackSpeed;
    private IEntityAttribute movementSpeed;
    private IEntityAttribute defenseModifier;
    private IEntityAttribute critResist;
    
    public ObservableList<StatusEffect> currentStatusEffects = new ObservableList<StatusEffect>();

    // Public properties
    public float Health { get; set; }
    public IEntityAttribute MaxHealth { get; set; }
    public float Mana { get; set; }
    public IEntityAttribute MaxMana { get; set; }
    public IEntityAttribute Power { get; set; }
    public IEntityAttribute CritRate { get; set; }
    public IEntityAttribute CritMultiplier { get; set; }
    public IEntityAttribute AttackSpeed { get; set; }
    public IEntityAttribute MovementSpeed { get; set; }
    public IEntityAttribute DefenseModifier { get; set; }
    public IEntityAttribute CritResist { get; set; }
    public ObservableList<StatusEffect> CurrentStatusEffects { get { return currentStatusEffects; } }

    // the used damage formula can vary for different implementations. Negative damage is handled as healing.
    protected virtual float CalculateIncomingDamage(Hit hit) {
        DefenseModifier = new IEntityAttribute(0.2f, 100, 0, 0, 0, 0);
        float cm = 1;
        if(IsCriticalHit(hit)){
            cm = hit.CritMultiplier;
        }
        Debug.Log("Crit multiplier is: " + cm);
        //Mana -= Math.Min((hit.Damage - DefenseModifier.TotalValue) * cm * hit.TargetManaModifier, Mana);  //for those rare skills that decrease mana
        return (1 - DefenseModifier.TotalValue) * hit.Damage * cm * hit.TargetHealthModifier;
    }

    public bool IsCriticalHit(Hit hit)
    {
        float critChance = hit.CritRate * (1.0f - CritResist.TotalValue);
        float randomValue = UnityEngine.Random.Range(0f, 1f);

        return randomValue < critChance;
    }

    public void TakeHit(Hit hit)
    {
        if (hit.TargetTags.Contains(this.gameObject.tag))
        {
            TakeDamage(CalculateIncomingDamage(hit));
            foreach (StatusEffect x in hit.StatusEffects)
            {
                ApplyStatusEffect(x);
            }
        }
    }

    public void TakeDamage(float amount)
    {
        //Debug.Log("Entity with tag: " + this.gameObject.tag + " took " + amount.ToString() + " damage.");
        Health -= amount;
        if (Health <= 0)
        {
            Health = 0;
            Die();
        }
        else if (Health > MaxHealth)
        {
            Health = MaxHealth;
        }
    }

    public void ApplyStatusEffect(StatusEffect newStatusEffect)
    {
        Debug.Log("Tryint to apply status effect: " + newStatusEffect.EffectData.EffectName);
        StatusEffectData statusEffect = newStatusEffect.EffectData;
        //remove previously applied incompatible status effects ie.: TERA -> Kelsaiks ice and flame debuffs
        //currentStatusEffects.RemoveAll(x => statusEffect.ErasePriorityLevels.Contains(x.EffectData.Priority));
        
        foreach (int p in statusEffect.ErasePriorityLevels) {
            int i = 0;
            while(i < currentStatusEffects.Count)
            {
                if (currentStatusEffects[i].EffectData.Priority == p)
                {
                    currentStatusEffects[i].RemoveEffect();
                }
                else
                {
                    i++;
                }
            }
        }
        

        //if already has the status effect
        if (currentStatusEffects.Exists( x => x.EffectData.EffectId == statusEffect.EffectId))
        {
            //Debug.Log("Found already applied status effect.");
            StatusEffect alreadyApplied = currentStatusEffects.Find(x => x.EffectData.EffectId == statusEffect.EffectId);
            
            //status effect refreshes the timer with a new application, ie.: TERA -> Combative Strike
            if (alreadyApplied.EffectData.Refreshes)
            {
                //Debug.Log("Found status effect refreshes.");
                //refreshes and stacks, ie.: TERA -> Traverse Cut. Stacks up to a certain stack size, and each new application of it refreshes previous stacks duration.
                if (alreadyApplied.EffectData.Stacks)
                {
                    //Debug.Log("Found status effect stacks.");
                    //check for maximum stacksize overflow handling
                    if (alreadyApplied.StackSize + newStatusEffect.StackSize <= alreadyApplied.EffectData.MaxStackSize)
                    {
                        alreadyApplied.StackSize += newStatusEffect.StackSize;
                        //Debug.Log("Gained " + newStatusEffect.StackSize + " new stacks. New stack count is: " + alreadyApplied.StackSize);
                    }
                    else{
                        alreadyApplied.StackSize = alreadyApplied.EffectData.MaxStackSize;
                    }
                }
                alreadyApplied.Duration = newStatusEffect.Duration;
                //Debug.Log("Refreshed Duration to " + alreadyApplied.Duration);
            }
            else
            {
                //TODO: count individual status effect stacks, to take maxStackSize into account
                // status effect has individual stacks that don't share a timer, but can apply the effect multiple times at once.
                if (alreadyApplied.EffectData.Stacks)
                {
                    ApplyAsNewStatusEffect(newStatusEffect);
                }
            }
        }
        //if target isn't affected by the status effect yet
        else
        {
            ApplyAsNewStatusEffect(newStatusEffect);
        }
    }

    private void ApplyAsNewStatusEffect(StatusEffect statusEffect)
    {
        //Debug.Log("Applying as new status effect.");
        StatusEffect temp = new StatusEffect(statusEffect.EffectData.EffectId, statusEffect.Duration, statusEffect.StackSize);
        temp.Affected = this;
        currentStatusEffects.Add(temp);
        //Debug.Log("Starting the new status effect.");
        temp.ApplyEffect();
    }

    

    public virtual void Die()
    {
        //Debug.Log("Entity Died with tag: " + this.gameObject.tag);
        //lock motion
        //erase the status effects that don't need to be kept around
        //play death animation
        //open respawn/game over message window?
    }

    public virtual void Resurrect()                                     
    {
        //optional, if resurrection becomes a thing.
        //play resurrection animation
        //set health to a positive number
        //unlock motion

    }

}
