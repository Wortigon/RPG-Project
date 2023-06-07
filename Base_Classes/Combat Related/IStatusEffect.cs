using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class IStatusEffect : MonoBehaviour
{
    //the amount of time a status effect is applied for
    protected float duration;
    public float Duration { get; set; }

    //whether or not the status effect can have multiple stacks. ie.: endurance decrease, power increase, etc.
    protected bool stacks = false;
    public bool Stacks { get; }

    //the amount of stacks this status effect increases the stack size of the one applied to the target. 1 by default.
    protected int stacksToGive = 1;
    public int StacksToGive { get; }

    //the number of stacks the status effect has
    protected int stackSize;
    public int StackSize { get; set; }

    //the maximum number of stacks the status effect can have
    protected int maxStackSize;
    public int MaxStackSize { get; }
    
    //whether or not the status effect refreshes the timer of already applied version of itself.
    protected bool refreshes = true;
    public bool Refreshes { get; }

    protected bool showUpInStatusBar;
    public bool ShowUpInStatusBar { get; }
    
    //the priority assigned to a status effect. 
    protected int priority;
    public int Priority { get; }

    //priority levels of other status effects that the status effect erases when applied. (ie.: different skill applying stun/root would be removed when applying a knockdown, etc. Empty by default.
    protected List<int> erasePriorityLevels = new List<int>();
    public List<int> ErasePriorityLevels { get; }

    //the identifier of a specific status effect
    protected int effectId;
    public int EffectId { get; }
    
    //the target that the status affect is applied to
    protected ICombatable affected;
    public ICombatable Affected {get; set; }

    //The effect coroutine
    private IEnumerator effect;
    private IEnumerator timer;

    public void StartEffect()
    {
        effect = Effect();
        timer = TimerCountdown();
        StartCoroutine(effect);
        StartCoroutine(timer);
    }

    public void removeStatusEffect()
    {
        removeEffects();
        //affected.CurrentStatusEffects.Remove(this);
        Destroy(this.gameObject);
    }

    protected virtual void removeEffects()
    {
        //virtual, overrideable function in case a buff/debuff works by spawning an entity, such as a bigger hitbox around the character that buffs players by using an aura, or if we just have to remove the effects of a buff.
    }

    protected IEnumerator TimerCountdown()
    {
        float timerTickDelay = 0.1f;  //the delay between ticks in seconds
        while (duration > 0)
        {
            //string cucc = str(duration);
            duration -= timerTickDelay;
            yield return new WaitForSeconds(timerTickDelay);
        }
        //Debug.log("status effect with id of " + EffectId + " was removed.");
        removeStatusEffect();
    }

    protected abstract IEnumerator Effect();

    protected abstract void Init();

}
