using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class StatusEffectData
{
    private readonly int effectId;
    private readonly string effectName;
    private readonly bool stacks;
    private readonly int maxStackSize;
    private readonly bool refreshes;
    private readonly bool showUpInStatusBar;
    private readonly int priority;
    private readonly List<int> erasePriorityLevels;
    private readonly Sprite icon;
    private readonly string description;
    private readonly string effectCoroutineName;
    private readonly string removeEffectName;
    private Action<ICombatable> effectCoroutine;
    private Action<ICombatable> removeEffect;


    public int EffectId { get { return effectId; } }
    public string EffectName { get { return effectName; } }
    public bool Stacks { get { return stacks; } }
    public int MaxStackSize { get { return maxStackSize; } }
    public bool Refreshes { get { return refreshes; } }
    public bool ShowUpInStatusBar { get { return showUpInStatusBar; } }
    public int Priority { get { return priority; } }
    public List<int> ErasePriorityLevels { get { return erasePriorityLevels; } }
    public Sprite Icon { get { return icon; } }
    public string Description { get { return description; } }
    public string EffectCoroutineName { get { return effectCoroutineName; } }
    public string RemoveEffectName { get { return removeEffectName; } }
    public Action<ICombatable> EffectCoroutine { get { return effectCoroutine; } set { effectCoroutine = value; } }
    public Action<ICombatable> RemoveEffect { get { return removeEffect; } set { removeEffect = value; } }
    

    public StatusEffectData(int effectId, string effectName, bool stacks, int maxStackSize, bool refreshes, bool showUpInStatusBar, int priority, List<int> erasePriorityLevels, Sprite icon, string description, string effectCoroutine, string removeEffect)
    {
        this.effectId = effectId;
        this.effectName = effectName;
        this.stacks = stacks;
        this.maxStackSize = maxStackSize;
        this.refreshes = refreshes;
        this.showUpInStatusBar = showUpInStatusBar;
        this.priority = priority;
        this.erasePriorityLevels = erasePriorityLevels;
        this.icon = icon;
        this.description = description;
        this.effectCoroutineName = effectCoroutine;
        this.removeEffectName = removeEffect;
        this.effectCoroutine = null;
        this.removeEffect = null;
    }

}