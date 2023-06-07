using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.tvOS;
using UnityEngine.UI;

public class StatusEffectManager : UIContent
{

    [SerializeField] public StatusEffectIcon statusEffectIconPrefab;
    [SerializeField] private ObservableList<StatusEffect> _statusEffects;
    [SerializeField] private List<StatusEffectIcon> statusEffectIcons = new List<StatusEffectIcon>(); // Store references to the created icons


    public ObservableList<StatusEffect> StatusEffects
    {
        get
        {
            if (_statusEffects == null)
            {
                _statusEffects = DataTable.Instance.player.CurrentStatusEffects;
                _statusEffects.OnItemAdded += HandleStatusEffectAdded;
                _statusEffects.OnItemRemoved += HandleStatusEffectRemoved;
            }
            return _statusEffects;
        }
    }

    private void HandleStatusEffectAdded(StatusEffect statusEffect)
    {
        // Handle the addition of a new status effect
        CreateStatusEffectIcon(statusEffect);
    }

    private void HandleStatusEffectRemoved(StatusEffect statusEffect)
    {
        for(int i = 0; i< statusEffectIcons.Count; i++)
        {
            if (statusEffectIcons[i].StatusEffect.EffectData.EffectId == statusEffect.EffectData.EffectId)
            {
                RemoveStatusEffectIcon(statusEffectIcons[i]);
                break;
            }
        }
    }

    

    public override void Initialize()
    {
        this.transform.localPosition = new Vector3(0, 0, 0);
        this.transform.localScale = Vector3.one;
    }

    public override Vector2 GetSize()
    {
        Vector2 size = new Vector2();
        size.x = this.GetComponent<RectTransform>().rect.width;
        int rowcount = StatusEffects.Count / this.GetComponent<GridLayoutGroup>().constraintCount;
        size.y = rowcount * this.GetComponent<GridLayoutGroup>().cellSize.y;
        return size;
    }

    private void ResetStatusEffectIcons()
    {
        ClearStatusEffectIcons();
        CreateStatusEffectIcons();
    }

    private void CreateStatusEffectIcons()
    {
        foreach (StatusEffect statusEffect in StatusEffects)
        {
            CreateStatusEffectIcon(statusEffect);
        }
    }

    private void CreateStatusEffectIcon(StatusEffect statusEffect)
    {
        if (statusEffect.EffectData.ShowUpInStatusBar)
        {
            StatusEffectIcon icon = Instantiate(statusEffectIconPrefab, transform);
            icon.Initialize(statusEffect);
            statusEffectIcons.Add(icon);
        }
    }

    private void ClearStatusEffectIcons()
    {
        foreach (StatusEffectIcon icon in statusEffectIcons)
        {
            RemoveStatusEffectIcon(icon);
        }
        //statusEffectIcons.Clear();
    }

    private void RemoveStatusEffectIcon(StatusEffectIcon icon)
    {
        statusEffectIcons.Remove(icon);
        Destroy(icon.gameObject);
    }
}
