using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect
{
    private StatusEffectData effectData;
    private float duration;
    private int stackSize;
    private Coroutine timerCoroutine;
    private ICombatable affectedEntity;
    private float timerTickDelay;
    private Coroutine startedEffectCoroutine;

    // Add getters and setters for the duration and stackSize variables
    public float Duration { get { return duration; } set { duration = value; } }
    public int StackSize { get { return stackSize; } set { stackSize = value; } }
    public StatusEffectData EffectData { get { return effectData; } }
    public ICombatable Affected { get { return affectedEntity; } set { affectedEntity = value; } }

    public StatusEffect(int effectId, float duration, int stackSize, float timerTickDelay = 0.1f)
    {
        this.effectData = DataTable.Instance.GetStatusEffectData(effectId);
        this.duration = duration;
        this.stackSize = stackSize;
        this.timerTickDelay = timerTickDelay;
    }

    public IEnumerator TimerCountdown()
    {
        while (duration > 0)
        {
            duration -= timerTickDelay;
            yield return new WaitForSeconds(timerTickDelay);
        }
        RemoveEffect();
    }

    public void ApplyEffect()
    {
        Debug.Log("Trying to apply effect of " + effectData.EffectName);
        if (effectData.EffectCoroutine != null)
        {
            //startedEffectCoroutine = affectedEntity.StartCoroutine(effectData.EffectCoroutine);
        }
        timerCoroutine = affectedEntity.StartCoroutine(TimerCountdown());
        //Debug.Log("Started Timer Coroutine");
    }

    public void RemoveEffect()
    {
        //Debug.Log("Removing self from applied status effects.");
        Affected.CurrentStatusEffects.Remove(this);
        if(startedEffectCoroutine!= null)
        {
            affectedEntity.StopCoroutine(startedEffectCoroutine);
            startedEffectCoroutine= null;
        }
        //Debug.Log("Stopping timer coroutine at " + Duration);
        if (timerCoroutine != null)
        {
            affectedEntity.StopCoroutine(timerCoroutine);
            timerCoroutine= null;
        }
        if (effectData.RemoveEffect != null)
        {
            effectData.RemoveEffect.DynamicInvoke(affectedEntity);
        }
        //Debug.Log("Removed self, an stopped every coroutine, and ran custome remove effect function");
    }

    
}
